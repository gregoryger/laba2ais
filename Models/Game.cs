namespace Models
{
    /// <summary>
    /// Доменная сущность игры с названием, жанром и рейтингом.
    /// </summary>
    public class Game : IDomainObject
    {
        /// <summary>
        /// Уникальный идентификатор игры.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название игры.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Жанр (например, Action, RPG).
        /// </summary>
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Рейтинг от 0 до 10.
        /// </summary>
        public double Rating { get; set; }
    }
}
