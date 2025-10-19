namespace Models
{
    /// <summary>
    /// Базовый интерфейс для доменных объектов с идентификатором.
    /// </summary>
    public interface IDomainObject
    {
        /// <summary>
        /// Уникальный идентификатор объекта.
        /// </summary>
        int Id { get; set; }
    }
}
