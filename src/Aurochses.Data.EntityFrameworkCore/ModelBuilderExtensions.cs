using Microsoft.EntityFrameworkCore;

namespace Aurochses.Data.EntityFrameworkCore
{
    /// <summary>
    /// Extensions of ModelBuilder.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Adds the configuration.
        /// </summary>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <typeparam name="TKey">The type of the T key.</typeparam>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="entityTypeConfiguration">The entity type configuration.</param>
        public static void AddConfiguration<TEntity, TKey>(this ModelBuilder modelBuilder, EntityTypeConfiguration<TEntity, TKey> entityTypeConfiguration)
            where TEntity : class, IEntity<TKey>
        {
            entityTypeConfiguration.Map(modelBuilder.Entity<TEntity>());
        }
    }
}