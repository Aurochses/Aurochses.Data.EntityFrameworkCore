using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aurochses.Data.EntityFrameworkCore
{
    /// <summary>
    /// EntityTypeConfiguration.
    /// </summary>
    /// <typeparam name="TEntity">The type of the T entity.</typeparam>
    /// <typeparam name="TKey">The type of the T key.</typeparam>
    public abstract class EntityTypeConfiguration<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTypeConfiguration{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="schemaName">Name of the schema.</param>
        protected EntityTypeConfiguration(string schemaName)
        {
            SchemaName = schemaName;
        }

        /// <summary>
        /// Gets the name of the schema.
        /// </summary>
        /// <value>The name of the schema.</value>
        public string SchemaName { get; }

        /// <summary>
        /// Maps the specified entity type builder.
        /// </summary>
        /// <param name="entityTypeBuilder">The entity type builder.</param>
        public abstract void Map(EntityTypeBuilder<TEntity> entityTypeBuilder);
    }
}