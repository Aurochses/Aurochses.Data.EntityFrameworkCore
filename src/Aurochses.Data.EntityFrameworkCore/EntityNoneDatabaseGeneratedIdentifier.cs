using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurochses.Data.EntityFrameworkCore
{
    /// <summary>
    /// Entity with non database generated identifier for data layer.
    /// </summary>
    /// <typeparam name="TType">The type of the T type.</typeparam>
    /// <seealso>
    ///     <cref>Aurochses.Data.IEntity{TType}</cref>
    /// </seealso>
    public class EntityNoneDatabaseGeneratedIdentifier<TType> : Entity<TType>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override TType Id { get; set; }
    }
}