namespace GameApp.Shared
{
    /// <summary>
    /// Модель данных игры для отображения во View.
    /// </summary>
    public class GameViewModel
    {
        /// <summary>
        /// Идентификатор игры.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название игры.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Жанр игры.
        /// </summary>
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Оценка в диапазоне 0..10.
        /// </summary>
        public double Rating { get; set; }
    }
}
