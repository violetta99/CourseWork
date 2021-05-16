using EngineLibrary.Components;
using System;

namespace GameLibrary.Components
{
    /// <summary>
    /// Компонент врага
    /// </summary>
    public class EnemyComponent : ObjectComponent
    {
        /// <summary>
        /// Событие смерти врага
        /// </summary>
        public event Action OnEnemyDeath;

        /// <summary>
        /// Состояние смерти врага 
        /// </summary>
        public bool IsDeath { get => _health <= 0; }
        /// <summary>
        /// Скорость врага
        /// </summary>
        public float Speed { 
            get
            {
                if (_isSlow)
                    return _speed / 2.0f;
                else
                    return _speed;
            }
        }

        private int _health;
        private float _speed;
        private bool _isSlow;
        private float _slowTime;
        private float _time;

        /// <summary>
        /// Конструктор компонента
        /// </summary>
        /// <param name="health">Здоровье врага</param>
        /// <param name="speed">Скорость врага</param>
        public EnemyComponent(int health, float speed)
        {
            _health = health;
            _speed = speed;
        }

        /// <summary>
        /// Сделать врага медленее 
        /// </summary>
        /// <param name="slowTime">Время замедления</param>
        public void MakeSlow(float slowTime)
        {
            _isSlow = true;
            _slowTime = slowTime;
            _time = 0;
        }

        /// <summary>
        /// Обновление состояния замедления
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        public void UpdateSlowEffect(float delta)
        {
            if (_isSlow)
            {
                _time += delta;
                if (_time >= _slowTime)
                {
                    _isSlow = false;
                }
            }
        }

        /// <summary>
        /// Уменьшение здоровья врага
        /// </summary>
        public void RemoveHealth()
        {
            _health--;
            if (_health <= 0)
                OnEnemyDeath?.Invoke();
        }
    }
}
