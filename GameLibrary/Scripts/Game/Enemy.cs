using EngineLibrary.Animation;
using EngineLibrary.Graphics;
using EngineLibrary.Scripts;
using GameLibrary.Components;
using SharpDX;
using SoundLibrary;
using System;

namespace GameLibrary.Scripts.Game
{
    /// <summary>
    /// Класс поведения врага
    /// </summary>
    public class Enemy : Script
    {
        private SharpAudioVoice _voice1;
        private SharpAudioVoice _voice2;

        private Game3DObject _target;
        private EnemyComponent _enemyComponent;
        private ColliderComponent _colliderComponent;
        private ReloadComponent _reloadComponent;

        private float _detectRadius;
        private int _damage;
        private bool _isPlayerDetected;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="target">Цель врага</param>
        /// <param name="detectRadius">Радиус обнаружения</param>
        /// <param name="damage">Урон врага</param>
        public Enemy(SharpAudioVoice voice1, SharpAudioVoice voice2, Game3DObject target, float detectRadius, int damage)
        {
            _voice1 = voice1;
            _voice2 = voice2;
            _target = target;
            _detectRadius = detectRadius;
            _damage = damage;
        }

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public override void Init()
        {
            _enemyComponent = GameObject.GetComponent<EnemyComponent>();
            _colliderComponent = GameObject.GetComponent<ColliderComponent>();
            _reloadComponent = GameObject.GetComponent<ReloadComponent>();
        }

        /// <summary>
        /// Обновление поведения класса
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        public override void Update(float delta)
        {
            if (_enemyComponent.IsDeath)
            {
                GameObject.Scene.RemoveGameObject(GameObject);
                _colliderComponent.RemoveCollisionFromScene();
            }

            _enemyComponent.UpdateSlowEffect(delta);

            // проверяем, появился ли игрок в радиусе
            var direction = _target.Position - GameObject.Position;
            direction.Y = 0;
            if (!_isPlayerDetected && direction.Length() < _detectRadius)
            {
                _voice1.Stop();
                _voice1.Play();
                _isPlayerDetected = true;
            }

            // если атака врага перезаряжается или игрок не обнаружен, ничего не делаем
            if (_reloadComponent.IsReload || !_isPlayerDetected) 
                return;

            // двигаемсчя к цели
            MoveToTarget(direction, delta);

            // атакуем, если враг достиг игрока
            if (_colliderComponent.CheckIntersects(out Game3DObject player, "player"))
            {
                Attack(player);
            }
        }

        /// <summary>
        /// От так
        /// </summary>
        /// <param name="player">Игрок</param>
        private void Attack(Game3DObject player)
        {
            _voice1.Stop();
            _voice2.Stop();
            _voice2.Play();
            _reloadComponent.StartReload();
            player.GetComponent<PlayerComponent>().RemoveHealth(_damage);
            new Animation(new float[] { 0f, -0.02f, 0.035f }, 0.5f).AddProcess((value) => {
                var position = GameObject.Children[1].LocalPosition;
                GameObject.Children[1].MoveBy(0, 0, value);
            });
        }

        /// <summary>
        /// Движение к цели
        /// </summary>
        /// <param name="direction">Вектор движения</param>
        /// <param name="delta">Время между кадрами</param>
        private void MoveToTarget(Vector3 direction, float delta)
        {
            direction.Normalize();
            // считаем угол поворота к цели
            float angle = (float)Math.Atan2(-direction.X, -direction.Z);
            // запоминаем старую позицию
            var oldPosition = GameObject.Position;
            // двигаем и крутит объект
            GameObject.SetRotationZ(angle);
            GameObject.MoveBy(direction * _enemyComponent.Speed * delta);

            // проверяем столкновение
            if (_colliderComponent.CheckIntersects(out _, "obstacle"))
            {
                // смещаем объект обратно
                GameObject.MoveTo(oldPosition);
                // угол для направления обхода препятствия 
                if (direction.X < 0)
                    angle += MathUtil.Pi / 2;
                else
                    angle -= MathUtil.Pi / 2;
                // создаем новый вектор направления и двигаем
                Matrix rotationMatrix = Matrix.RotationYawPitchRoll(angle, 0, 0);
                direction = (Vector3)Vector3.Transform(direction, rotationMatrix);
                direction.Normalize();
                GameObject.MoveBy(direction * _enemyComponent.Speed * delta);
            }
        }
    }
}
