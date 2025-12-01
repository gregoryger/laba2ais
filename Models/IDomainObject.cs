namespace Models
{
    /// <summary>
    /// Базовый интерфейс доменной сущности с идентификатором.
    /// </summary>
    public interface IDomainObject
    {
        /// <summary>
        /// Идентификатор записи.
        /// </summary>
        int Id { get; set; }
    }
}
