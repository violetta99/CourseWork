using EngineLibrary.Scripts;
using GameLibrary.Components;

namespace GameLibrary.Scripts.Utils
{
    /// <summary>
    /// Класс обновления компонента видимости объекта
    /// </summary>
    public class VisibleScript : Script
    {
        private VisibleComponent _visibleComponent;

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public override void Init()
        {
            _visibleComponent = GameObject.GetComponent<VisibleComponent>();
        }

        /// <summary>
        /// Обновление поведения
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        public override void Update(float delta)
        {
            _visibleComponent.Update(delta);
        }
    }
}
