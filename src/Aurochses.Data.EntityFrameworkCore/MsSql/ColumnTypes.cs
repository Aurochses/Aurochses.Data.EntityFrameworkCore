namespace Aurochses.Data.EntityFrameworkCore.MsSql
{
    /// <summary>
    /// Column Types.
    /// </summary>
    public static class ColumnTypes
    {
        /// <summary>
        /// The NVarChar
        /// </summary>
        public const string NVarChar = "nvarchar";

        /// <summary>
        /// The Money
        /// </summary>
        public const string Money = "decimal(18,4)";

        /// <summary>
        /// The DateTime
        /// </summary>
        public const string DateTime = "datetime2";

        /// <summary>
        /// The Date
        /// </summary>
        public const string Date = "date";

        /// <summary>
        /// Specifies the length of the n variable character.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string SpecifyNVarCharLength(int length = ColumnLengths.DefaultNVarChar)
        {
            return $"{NVarChar}({length})";
        }
    }
}