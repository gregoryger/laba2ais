namespace Models
{
    /// <summary>
    /// Представление модели игры, включающее ID, Name, Genre, Rating.
    /// </summary>
    public class Game : IDomainObject
    {
        /// <summary>
        /// ID игры.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название игры.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Жанр игры.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Рейтинг игры (0-10).
        /// </summary>
        public double Rating { get; set; }
    }
}
