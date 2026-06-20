using DreamNumbers.Models;
using DreamNumbers.Storages.EFCore.DbContexts;
using DreamNumbers.Storages.EFCore.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DreamNumbers.Storages.EFCore.Storages
{
    internal class EuroMillionDrawStorage<TContext> : IEuroMillionDrawStorage where TContext : DreamNumbersDbContext<TContext>
    {
        /// <summary>
        /// The DbContext.
        /// </summary>
        protected readonly TContext Context;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<EuroMillionDrawStorage<TContext>> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EuroMillionDrawStorage<TContext>"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public EuroMillionDrawStorage(TContext context, ILogger<EuroMillionDrawStorage<TContext>> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<EuroMillionDraw>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await Context.EuroMillionDraws.OrderByDescending(d => d.Date).AsNoTracking().ToListAsync(cancellationToken);

            return [.. result.Select(EuroMillionDrawMapper.ToModel)];
        }

        public async Task<List<EuroMillionDraw>> GetLastDrawsAsync(int count, CancellationToken cancellationToken = default)
        {
            var result = await Context.EuroMillionDraws.OrderByDescending(d => d.Date).Take(count).AsNoTracking().ToListAsync(cancellationToken);

            return [.. result.Select(EuroMillionDrawMapper.ToModel)];
        }

        public async Task<EuroMillionDraw?> GetLastDrawAsync(CancellationToken cancellationToken = default)
        {
            var entity = await Context.EuroMillionDraws
                .OrderByDescending(d => d.Date)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            return entity == null ? null : EuroMillionDrawMapper.ToModel(entity);
        }

        public async Task<DateTime?> GetLastDrawDateAsync(CancellationToken cancellationToken = default)
        {
            return await Context.EuroMillionDraws
                .OrderByDescending(d => d.Date)
                .AsNoTracking()
                .Select(d => (DateTime?)d.Date)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddOrUpdateAsync(EuroMillionDraw draw, CancellationToken cancellationToken = default)
        {
            var existingEuroMillionDraw = await Context.EuroMillionDraws.FindAsync(new object[] { draw.Id }, cancellationToken);
            if (existingEuroMillionDraw == null)
            {
                var entity = EuroMillionDrawMapper.ToEntity(draw);

                await Context.EuroMillionDraws.AddAsync(entity, cancellationToken);

                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation("Added new draw '{DrawNumber}' with ID {Id}.", entity.DrawNumber, entity.Id);
                }
            }
            else
            {
                Context.Entry(existingEuroMillionDraw).CurrentValues.SetValues(draw);

                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation("Updated existing draw '{DrawNumber}' with ID {Id}.", draw.DrawNumber, draw.Id);
                }
            }
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task InsertAsync(EuroMillionDraw draw, CancellationToken cancellationToken = default)
        {
            var entity = EuroMillionDrawMapper.ToEntity(draw);
            Context.EuroMillionDraws.Add(entity);
            await Context.SaveChangesAsync(cancellationToken);

            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Added new draw '{DrawNumber}' with ID {Id}.", entity.DrawNumber, entity.Id);
            }
        }

        public async Task InsertManyAsync(IEnumerable<EuroMillionDraw> draws, CancellationToken cancellationToken = default)
        {
            var entities = draws.Select(EuroMillionDrawMapper.ToEntity);
            Context.EuroMillionDraws.AddRange(entities);
            await Context.SaveChangesAsync(cancellationToken);
        }

    }
}
