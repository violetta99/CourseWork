using EngineLibrary.Game;
using EngineLibrary.Scripts;
using GameLibrary.Components;
using SharpDX;
using SharpDX.DirectInput;
using System.Windows.Forms;

namespace GameLibrary.Scripts.Game
{
    /// <summary>
    /// Класс поведения игрока
    /// </summary>
    public class PlayerMovement : KeyboardListenerScript
    {
        private PlayerComponent _playerComponent;
        private ColliderComponent _colliderComponent;
        private PhysicsComponent _physicsComponent;

        private Vector3 _moveDirection;

        private float _deltaAngle;
        private float _moveSpeed;
        private float _rotSpeed;
        private bool _isCanRun;

        /// <summary>
        /// Скорость передвижения игрока
        /// </summary>
        private float Speed 
        {
            get
            {
                if(_isCanRun)
                {
                    return _moveSpeed * 2.0f;
                }
                else
                {
                    return _moveSpeed;
                }
            }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="deltaAngle">Разница угла поворота от разрешения экрана</param>
        /// <param name="moveSpeed">Скорость передвижения игрока</param>
        /// <param name="rotSpeed">Скорость вращения камеры игрока</param>
        /// <param name="jumpForce">Сила прыжка</param>
        public PlayerMovement(float deltaAngle, float moveSpeed, float rotSpeed, float jumpForce)
        {
            _deltaAngle = deltaAngle;
            _moveSpeed = moveSpeed;
            _rotSpeed = rotSpeed;


            Actions.Add(Key.W, delta => _moveDirection += Vector3.UnitZ);
            Actions.Add(Key.S, delta => _moveDirection -= Vector3.UnitZ);
            Actions.Add(Key.A, delta => _moveDirection -= Vector3.UnitX);
            Actions.Add(Key.D, delta => _moveDirection += Vector3.UnitX);
            Actions.Add(Key.LeftShift, delta =>
            {
                _isCanRun = true && _physicsComponent.IsGrounded && !_playerComponent.IsAiming;
            });
            Actions.Add(Key.Space, delta =>
            {
                if (_physicsComponent.IsGrounded)
                    _physicsComponent.AddImpulse(Vector3.UnitY * jumpForce);
            });
        }

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public override void Init()
        {
            _playerComponent = GameObject.GetComponent<PlayerComponent>();
            _colliderComponent = GameObject.GetComponent<ColliderComponent>();
            _physicsComponent = GameObject.GetComponent<PhysicsComponent>();
        }

        /// <summary>
        /// Обновления поведения перед нажатием клавиш
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        protected override void BeforeKeyProcess(float delta)
        {
            _isCanRun = false;
            _moveDirection = Vector3.Zero;
        }

        /// <summary>
        /// Обновления поведения после нажатия клавиш
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        protected override void AfterKeyProcess(float delta)
        {
            // обрабатываем мышь
            var inputController = InputController.GetInstance();
            GameObject.RotateZ(_deltaAngle * inputController.MouseRelativePositionX * _rotSpeed);
            GameObject.RotateY(_deltaAngle * inputController.MouseRelativePositionY * _rotSpeed);

            // затем смещаем курсор мыши, чтоб он всегда был посередине, чтоб не вылететь за рамки экрана
            // нужно делать тут, так то код выше берет передвижение мыши по экрану
            var form = GameObject.Scene.Game.RenderForm;
            Cursor.Position = new System.Drawing.Point(form.Location.X + form.ClientSize.Width / 2, form.Location.Y + form.ClientSize.Height / 2);

            // применяем перемещние и вращение
            _moveDirection.Normalize();
            Vector3 rotation = GameObject.Rotation;
            Matrix rotationMatrix = Matrix.RotationYawPitchRoll(rotation.Z, rotation.Y, rotation.X);
            var oldPosition = GameObject.Position;
            GameObject.MoveBy((Vector3)Vector3.Transform(_moveDirection * Speed * delta, rotationMatrix));

            if (_colliderComponent.CheckIntersects(out _, "obstacle"))
            {
                GameObject.MoveTo(oldPosition);
            }
        }
    }
}
