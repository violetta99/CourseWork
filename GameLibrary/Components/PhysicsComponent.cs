using EngineLibrary.Components;
using EngineLibrary.Graphics;
using SharpDX;
using System;
using System.Collections.Generic;

namespace GameLibrary.Components
{
    /// <summary>
    /// Компонент физического поведения
    /// </summary>
    public class PhysicsComponent : ObjectComponent
    {
        private List<Vector3> _impulses = new List<Vector3>();
        private Vector3 _gravity;
        private Vector3 _direction;
        private float _friction;
        private float _minY;
        private bool _isGrounded;

        /// <summary>
        /// Находиться ли объект на поверхности
        /// </summary>
        public bool IsGrounded { get => _isGrounded; }

        /// <summary>
        /// Конструктор компонента
        /// </summary>
        /// <param name="gravity">Сила графитации</param>
        /// <param name="friction">Коэффициент трения</param>
        public PhysicsComponent(float gravity = 1, float friction = 1)
        {
            _gravity = Vector3.UnitY * -gravity;
            _friction = friction;
            _direction = Vector3.Zero;
        }

        /// <summary>
        /// Обновление позиции объекта в зависимости от действующих сил
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        public void UpdateObjectPosition(float delta)
        {
            Vector3 acceleration = _gravity * delta;

            for (int i = 0; i < _impulses.Count; i++)
            {
                acceleration += _impulses[i] * delta;
            }
            _impulses.Clear();

            _direction += acceleration;

            _direction -= (GetSignVector(_direction) * _friction * delta);

            if (MathUtil.NearEqual(_direction.X, 0))
            {
                _direction.X = 0;
            }
            if (MathUtil.NearEqual(_direction.Z, 0))
            {
                _direction.Z = 0;
            }

            GameObject.MoveBy(_direction);

            Vector3 position = GameObject.Position;
            if (position.Y < _minY)
            {
                _direction.Y = 0;
                GameObject.MoveTo(new Vector3(position.X, _minY, position.Z));
            }
        }

        private static Vector3 GetSignVector(Vector3 vector)
        {
            return new Vector3(Math.Sign(vector.X), Math.Sign(vector.X), Math.Sign(vector.Z));
        }

        /// <summary>
        /// Обновление позиции и состояния поверхности
        /// </summary>
        /// <param name="ground">Объект поверхности</param>
        public void UpdateGrounded(Game3DObject ground)
        {
            _isGrounded = ground != null && ground.Position.Y <= GameObject.Position.Y;
            _minY = ground != null ? ground.Position.Y : 0.0f;
        }

        /// <summary>
        /// Добавление силы
        /// </summary>
        /// <param name="impulse">Направление силы</param>
        public void AddImpulse(Vector3 impulse)
        {
            _impulses.Add(impulse);
        }
    }
}
