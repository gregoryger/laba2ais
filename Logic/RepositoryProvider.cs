namespace Logic
{
    /// <summary>
    /// Доступные реализации репозиториев.
    /// </summary>
    public enum RepositoryProvider
    {
        /// <summary>
        /// Entity Framework Core.
        /// </summary>
        EntityFramework,

        /// <summary>
        /// Dapper.
        /// </summary>
        Dapper
    }
}
