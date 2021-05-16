using EngineLibrary.Components;

namespace GameLibrary.Components
{
    /// <summary>
    /// Класс перезарядки
    /// </summary>
    public class ReloadComponent : ObjectComponent
    {
        /// <summary>
        /// Состояние перехарядки
        /// </summary>
        public bool IsReload { get => _isReload; }

        private float _reloadTime;
        private float _time;
        private bool _isReload;

        /// <summary>
        /// Конктруктор компонента
        /// </summary>
        /// <param name="reloadTime">Время перезарядки</param>
        public ReloadComponent(float reloadTime)
        {
            _reloadTime = reloadTime;
        }

        /// <summary>
        /// Запуск перезарядки
        /// </summary>
        public void StartReload()
        {
            _time = 0;
            _isReload = true;
        }

        /// <summary>
        /// Обновление состояния перезарядки
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        public void UpdateReload(float delta)
        {
            if (_isReload)
            {
                _time += delta;
                if (_time >= _reloadTime)
                {
                    _isReload = false;
                }
            }
        }
    }
}
