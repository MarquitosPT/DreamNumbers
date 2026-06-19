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
            Logger = logger;
        }

        public async Task<List<EuroMillionDraw>> GetAllAsync()
        {
            var result = await Context.EuroMillionDraws.OrderByDescending(d => d.Date).AsNoTracking().ToListAsync();

            return [.. result.Select(e => EuroMillionDrawMapper.ToModel(e))];
        }

        public async Task<EuroMillionDraw?> GetLastDrawAsync()
        {
            var entity = await Context.EuroMillionDraws
                .OrderByDescending(d => d.Date)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return entity == null ? null : EuroMillionDrawMapper.ToModel(entity);
        }

        public async Task<DateTime?> GetLastDrawDateAsync()
        {
            return await Context.EuroMillionDraws
                .OrderByDescending(d => d.Date)
                .AsNoTracking()
                .Select(d => (DateTime?)d.Date)
                .FirstOrDefaultAsync();
        }

        public async Task AddOrUpdateAsync(EuroMillionDraw draw)
        {
            var existingEuroMillionDraw = await Context.EuroMillionDraws.FindAsync(draw.Id);
            if (existingEuroMillionDraw == null)
            {
                var entity = EuroMillionDrawMapper.ToEntity(draw);

                await Context.EuroMillionDraws.AddAsync(entity);

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
            await Context.SaveChangesAsync();
        }

        public async Task InsertAsync(EuroMillionDraw draw)
        {
            var entity = EuroMillionDrawMapper.ToEntity(draw);
            Context.EuroMillionDraws.Add(entity);
            await Context.SaveChangesAsync();

            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Added new draw '{DrawNumber}' with ID {Id}.", entity.DrawNumber, entity.Id);
            }
        }

        public async Task InsertManyAsync(IEnumerable<EuroMillionDraw> draws)
        {
            var entities = draws.Select(EuroMillionDrawMapper.ToEntity);
            Context.EuroMillionDraws.AddRange(entities);
            await Context.SaveChangesAsync();
        }

    }
}
