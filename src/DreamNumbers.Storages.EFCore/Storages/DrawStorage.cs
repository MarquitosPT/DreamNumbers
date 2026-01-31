using DreamNumbers.Models;
using DreamNumbers.Storages.EFCore.DbContexts;
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

        public async Task AddOrUpdateAsync(Draw draw)
        {
            var existingDraw = await Context.Draws.FindAsync(draw.Id);
            if (existingDraw == null)
            {
                var entity = new Entities.Draw
                {
                    Id = draw.Id,
                    Date = draw.Date,
                    Numbers = draw.Numbers
                };

                await Context.Draws.AddAsync(entity);

                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation("Added new draw with ID {DrawId}.", draw.Id);
                }
            }
            else
            {
                Context.Entry(existingDraw).CurrentValues.SetValues(draw);

                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation("Updated existing draw with ID {DrawId}.", draw.Id);
                }
            }
            await Context.SaveChangesAsync();
        }

        public async Task<List<Draw>> GetAllAsync()
        {
            var result = await Context.Draws.ToListAsync();

            return result.Select(e => new Draw
            {
                Id = e.Id,
                Date = e.Date,
                Numbers = e.Numbers
            }).ToList();
        }
    }
}
