using EngineLibrary.Collisions;
using EngineLibrary.Graphics;
using EngineLibrary.Scripts;
using GameLibrary.Components;

namespace GameLibrary.Scripts.Utils
{
    /// <summary>
    /// Класс обновления компонента коллайдера
    /// </summary>
    public class ColliderScript : Script
    {
        private PhysicsComponent _physicsComponent;
        private ColliderComponent _colliderComponent;

        private Game3DObject _ground;

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public override void Init()
        {
            _physicsComponent = GameObject.GetComponent<PhysicsComponent>();
            _colliderComponent = GameObject.GetComponent<ColliderComponent>();
            _colliderComponent.AddCollisionOnScene();
        }

        /// <summary>
        /// Обновление поведения
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        public override void Update(float delta)
        {
            if (_physicsComponent == null) return;

            if (_physicsComponent.IsGrounded)
            {
                if (!ObjectCollision.Intersects(GameObject.Collision, _ground.Collision))
                    _ground = null;
            }
            else
            {
                if (_colliderComponent.CheckIntersects(out Game3DObject ground, "ground"))
                    _ground = ground;
            }

            _physicsComponent.UpdateGrounded(_ground);
        }
    }
}
