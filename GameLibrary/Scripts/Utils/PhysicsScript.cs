using GameLibrary.Components;
using EngineLibrary.Scripts;

namespace GameLibrary.Scripts.Utils
{
    /// <summary>
    /// Класс обновления компонента физического поведения 
    /// </summary>
    public class PhysicsScript : Script
    {
        private PhysicsComponent _physics;

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public override void Init()
        {
            _physics = GameObject.GetComponent<PhysicsComponent>();
        }

        /// <summary>
        /// Обновление поведения
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        public override void Update(float delta)
        {
            _physics.UpdateObjectPosition(delta);
        }
    }
}
