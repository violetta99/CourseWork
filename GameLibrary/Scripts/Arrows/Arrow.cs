using EngineLibrary.Graphics;
using EngineLibrary.Scripts;
using GameLibrary.Components;
using SharpDX;

namespace GameLibrary.Scripts.Arrows
{
    /// <summary>
    /// Класс обычной стрелы
    /// </summary>
    public class Arrow : Script
    {
        private ArrowComponent _arrowComponent;
        private ColliderComponent _colliderComponent;
        private PhysicsComponent _physics;
        private Game3DObject _spawnObject;

        private Vector3 _direction;

        private float _speed;
        private float _lifeTime;
        private float _time;
        private bool _isCollided;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="spawn">Объект, относительно которого появляется стрела</param>
        /// <param name="speed">Базовая скорость полёта стрелы</param>
        /// <param name="lifeTime">Время существования стрелы</param>
        public Arrow(Game3DObject spawn, float speed, float lifeTime)
        {
            _spawnObject = spawn;
            _speed = speed;
            _lifeTime = lifeTime;
            _time = 0.0f;
        }

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public override void Init()
        {
            // вращаем стрелу относительно объекта, от которого она появляется
            Vector3 rotation = _spawnObject.Rotation;
            GameObject.SetRotationX(rotation.X);
            GameObject.SetRotationY(rotation.Y);
            GameObject.SetRotationZ(rotation.Z);
            // и формируем направление движения относительно поворота
            Matrix rotationMatrix = Matrix.RotationYawPitchRoll(rotation.Z, rotation.Y, rotation.X);
            _direction = (Vector3)Vector3.Transform(Vector3.ForwardLH, rotationMatrix);

            _physics = GameObject.GetComponent<PhysicsComponent>();
            _arrowComponent = GameObject.GetComponent<ArrowComponent>();
            _colliderComponent = GameObject.GetComponent<ColliderComponent>();
        }

        /// <summary>
        /// Обновление поведения
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        public override void Update(float delta)
        {
            if (!_physics.IsGrounded && !_isCollided)
            {
                var oldPosition = GameObject.Position;
                GameObject.MoveBy(_direction * _speed * _arrowComponent.FiringForceCoef * delta);

                if(_colliderComponent.CheckIntersects(out Game3DObject enemy, "enemy"))
                {
                    HandleHitEnemy(enemy);
                }

                if (_colliderComponent.CheckIntersects(out _, "obstacle"))
                {
                    _isCollided = true;
                    GameObject.MoveTo(oldPosition);
                }
            }

            _time += delta;
            if(_time >= _lifeTime)
            {
                GameObject.Scene.RemoveGameObject(GameObject);
                _colliderComponent.RemoveCollisionFromScene();
            }
        }

        /// <summary>
        /// Обработка попадания стрелы во врага
        /// </summary>
        /// <param name="enemy">Объект врага</param>
        protected virtual void HandleHitEnemy(Game3DObject enemy)
        {
            enemy.GetComponent<EnemyComponent>().RemoveHealth();
            GameObject.Scene.RemoveGameObject(GameObject);
            _colliderComponent.RemoveCollisionFromScene();
        }
    }
}
