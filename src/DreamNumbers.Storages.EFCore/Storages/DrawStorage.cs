using DreamNumbers.Models;
using DreamNumbers.Storages.EFCore.DbContexts;
using DreamNumbers.Storages.EFCore.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DreamNumbers.Storages.EFCore.Storages
{
    internal class DrawStorage<TContext> : IDrawStorage where TContext : DreamNumbersDbContext<TContext>
    {
        /// <summary>
        /// The DbContext.
        /// </summary>
        protected readonly TContext Context;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<DrawStorage<TContext>> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawStorage<TContext>"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public DrawStorage(TContext context, ILogger<DrawStorage<TContext>> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }

        public async Task AddOrUpdateAsync(Draw draw, CancellationToken cancellationToken = default)
        {
            var existingDraw = await Context.Draws.FindAsync([draw.Id], cancellationToken);
            if (existingDraw == null)
            {
                var entity = DrawMapper.ToEntity(draw);

                await Context.Draws.AddAsync(entity, cancellationToken);

                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation("Added new draw '{DrawNumber}' with ID {Id}.", entity.DrawNumber, entity.Id);
                }
            }
            else
            {
                Context.Entry(existingDraw).CurrentValues.SetValues(draw);

                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation("Updated existing draw '{DrawNumber}' with ID {Id}.", draw.DrawNumber, draw.Id);
                }
            }
            await Context.SaveChangesAsync();
        }

        public async Task<List<Draw>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await Context.Draws.OrderByDescending(d => d.Date).AsNoTracking().ToListAsync(cancellationToken);

            return [.. result.Select(DrawMapper.ToModel)];
        }

        public async Task<List<Draw>> GetLastDrawsAsync(int count, CancellationToken cancellationToken = default)
        {
            var result = await Context.Draws.OrderByDescending(d => d.Date).Take(count).AsNoTracking().ToListAsync(cancellationToken);

            return [.. result.Select(DrawMapper.ToModel)];
        }

        public async Task<Draw?> GetLastDrawAsync(CancellationToken cancellationToken = default)
        {
            var entity = await Context.Draws
                .OrderByDescending(d => d.Date)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            return entity == null ? null : DrawMapper.ToModel(entity);
        }

        public async Task<DateTime?> GetLastDrawDateAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Draws
                .OrderByDescending(d => d.Date)
                .AsNoTracking()
                .Select(d => (DateTime?)d.Date)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task InsertAsync(Draw draw, CancellationToken cancellationToken = default)
        {
            var entity = DrawMapper.ToEntity(draw);
            Context.Draws.Add(entity);
            await Context.SaveChangesAsync(cancellationToken);

            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Added new draw '{DrawNumber}' with ID {Id}.", entity.DrawNumber, entity.Id);
            }
        }

        public async Task InsertManyAsync(IEnumerable<Draw> draws, CancellationToken cancellationToken = default)
        {
            var entities = draws.Select(DrawMapper.ToEntity);
            Context.Draws.AddRange(entities);
            await Context.SaveChangesAsync(cancellationToken);
        }

    }
}
