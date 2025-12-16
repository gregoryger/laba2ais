using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameApp.Presenter.Dto
{
    /// <summary>
    /// DTO для игры, используемый во ViewModel, с поддержкой уведомлений об изменениях.
    /// </summary>
    public class GameDto : INotifyPropertyChanged
    {
        private int _id;
        private string _name = string.Empty;
        private string _genre = string.Empty;
        private double _rating;

        /// <summary>
        /// Событие об изменении свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Идентификатор игры.
        /// </summary>
        public int Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        /// <summary>
        /// Название игры.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        /// <summary>
        /// Жанр игры.
        /// </summary>
        public string Genre
        {
            get => _genre;
            set => SetField(ref _genre, value);
        }

        /// <summary>
        /// Рейтинг игры в диапазоне 0..10.
        /// </summary>
        public double Rating
        {
            get => _rating;
            set => SetField(ref _rating, value);
        }

        /// <summary>
        /// Создает глубокую копию DTO.
        /// </summary>
        /// <returns>Новая копия.</returns>
        public GameDto Clone()
        {
            return new GameDto
            {
                Id = Id,
                Name = Name,
                Genre = Genre,
                Rating = Rating
            };
        }

        private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value))
            {
                return;
            }

            field = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
