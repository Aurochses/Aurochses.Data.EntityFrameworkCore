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
        /// The NVarChar max
        /// </summary>
        public const string NVarCharMax = "nvarchar(max)";

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
        /// Gets NVarChar with specified length.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns><cref>System.String</cref>.</returns>
        public static string GetNVarCharWithSpecifiedLength(int length = ColumnLengths.DefaultNVarChar)
        {
            return $"{NVarChar}({length})";
        }
    }
}