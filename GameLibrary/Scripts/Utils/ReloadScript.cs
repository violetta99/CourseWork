using EngineLibrary.Scripts;
using GameLibrary.Components;

namespace GameLibrary.Scripts.Utils
{
    /// <summary>
    /// Класс обновления компонента перезарядки
    /// </summary>
    public class ReloadScript : Script
    {
        private ReloadComponent _reloadComponent;

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public override void Init()
        {
            _reloadComponent = GameObject.GetComponent<ReloadComponent>();
        }

        /// <summary>
        /// Обновление поведения
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        public override void Update(float delta)
        {
            _reloadComponent.UpdateReload(delta);
        }
    }
}
