using EngineLibrary.Components;
using GameLibrary.Scenes;
using GameLibrary.Scripts.Game;
using System;
using System.Collections.Generic;

namespace GameLibrary.Components
{
    /// <summary>
    /// Компонент игрока
    /// </summary>
    public class PlayerComponent : ObjectComponent
    {
        /// <summary>
        /// Событие изменения количества стрел
        /// </summary>
        public event Action<ArrowType, int> OnArrowsCountChanged;
        /// <summary>
        /// Событие изменения здоровья
        /// </summary>
        public event Action<int> OnHealthChanged;
        /// <summary>
        /// Состояние прицеливания
        /// </summary>
        public bool IsAiming { get; set; }
        /// <summary>
        /// Проверка полных жизней
        /// </summary>
        public bool IsFullHealth { get => _health == 100; }

        private int _health;

        private Dictionary<ArrowType, int> _arrowsCount;

        /// <summary>
        /// Конструктор компонента
        /// </summary>
        public PlayerComponent()
        {
            _health = 50;
            _arrowsCount = new Dictionary<ArrowType, int>();
        }

        /// <summary>
        /// Добавление здоровья игроку
        /// </summary>
        /// <param name="count">>Количество добавлдяемых пунктов здоровья</param>
        public void AddHealth(int count)
        {
            _health += count;
            if (_health > 100)
                _health = 100;
            OnHealthChanged?.Invoke(_health);
        }

        /// <summary>
        /// Уменьшение здоровья игрока
        /// </summary>
        /// <param name="count">Количество отнимаемых пунктов здоровья</param>
        public void RemoveHealth(int count)
        {
            _health -= count;
            if (_health <= 0)
            {
                var scene = (MenuScene)GameObject.Scene.PreviousScene;
                scene.UpdateMenu(true);
                GameObject.Scene.Game.ChangeScene(scene);
            }
            OnHealthChanged?.Invoke(_health);
        }

        /// <summary>
        /// Добавление стрел
        /// </summary>
        /// <param name="type">Тип стрел</param>
        /// <param name="count">Количество добавляемых стрел</param>
        public void AddArrows(ArrowType type, int count)
        {
            if (!_arrowsCount.ContainsKey(type))
                _arrowsCount.Add(type, 0);

            _arrowsCount[type] += count;
            OnArrowsCountChanged?.Invoke(type, _arrowsCount[type]);
        }

        /// <summary>
        /// Проверка наличия стрел по типу
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Результат сравнения с 0</returns>
        public bool IsHasArrows(ArrowType type)
        {
            return _arrowsCount[type] != 0;
        }

        /// <summary>
        /// Уменьшение стрел по типу
        /// </summary>
        /// <param name="type">Тип</param>
        public void RemoveArrow(ArrowType type)
        {
            _arrowsCount[type]--;
            OnArrowsCountChanged?.Invoke(type, _arrowsCount[type]);
        }
    }
}
