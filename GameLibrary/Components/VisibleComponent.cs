using EngineLibrary.Components;

namespace GameLibrary.Components
{
    /// <summary>
    /// Компонент видимости объекта
    /// </summary>
    public class VisibleComponent : ObjectComponent
    {
        /// <summary>
        /// Состояние видимости
        /// </summary>
        public bool IsHidden { get => _isHidden; }

        private bool _isHidden;
        private float _duration;
        private float _time;

        /// <summary>
        /// Сделать объект невидимым на время
        /// </summary>
        /// <param name="duration">Время невидимости</param>
        public void MakeInvisible(float duration)
        {
            _duration = duration;
            _time = 0;
            _isHidden = true;
            GameObject.IsHidden = _isHidden;
        }

        /// <summary>
        /// Обновление состояни видимости
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        public void Update(float delta)
        {
            if (_isHidden)
            {
                _time += delta;
                if (_time >= _duration)
                {
                    _isHidden = false;
                    GameObject.IsHidden = _isHidden;
                }
            }
        }
    }
}
