using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurochses.Data.EntityFrameworkCore
{
    /// <summary>
    /// Entity with non database generated identifier for data layer.
    /// </summary>
    /// <typeparam name="TType">The type of the t type.</typeparam>
    /// <seealso>
    ///     <cref>Aurochses.Data.IEntity{TType}</cref>
    /// </seealso>
    public class EntityNoneDatabaseGeneratedIdentifier<TType> : Entity<TType>
    {
        private bool _isNew;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override TType Id { get; set; }

        /// <summary>
        /// Determines whether this instance is new.
        /// </summary>
        /// <returns><c>true</c> if this instance is new; otherwise, <c>false</c>.</returns>
        public override bool IsNew()
        {
            return _isNew;
        }


        /// <summary>
        /// Marks instance as new.
        /// </summary>
        public void MarkAsNew()
        {
            _isNew = true;
        }
    }
}