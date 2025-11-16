namespace Logic
{
    /// <summary>
    /// Источник данных, который следует использовать.
    /// </summary>
    public enum RepositoryProvider
    {
        /// <summary>
        /// Использовать репозиторий на Entity Framework Core.
        /// </summary>
        EntityFramework,

        /// <summary>
        /// Использовать репозиторий на Dapper.
        /// </summary>
        Dapper
    }
}
