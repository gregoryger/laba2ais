namespace Models
{
    /// <summary>
    /// Общий контракт для доменных объектов с числовым идентификатором.
    /// </summary>
    public interface IDomainObject
    {
        /// <summary>
        /// Идентификатор сущности.
        /// </summary>
        int Id { get; set; }
    }
}
