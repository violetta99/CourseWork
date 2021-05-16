using EngineLibrary.Graphics;
using EngineLibrary.Scripts;
using GameLibrary.Components;
using SoundLibrary;

namespace GameLibrary.Scripts.Bonuses
{
    /// <summary>
    /// Класс бонуса, дающего здоровья игроку
    /// </summary>
    public class HealthBonus : Script
    {
        private SharpAudioVoice _voice;
        private ColliderComponent _colliderComponent;

        private int _healthAddCount;

        /// <summary>
        /// Конструктор класса 
        /// </summary>
        /// <param name="healthAddCount">Добавляемое здоровье</param>
        public HealthBonus(SharpAudioVoice voice, int healthAddCount)
        {
            _voice = voice;
            _healthAddCount = healthAddCount;
        }

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public override void Init()
        {
            _colliderComponent = GameObject.GetComponent<ColliderComponent>();
        }

        /// <summary>
        /// Обновление поведения
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        public override void Update(float delta)
        {
            if (_colliderComponent.CheckIntersects(out Game3DObject player, "player"))
            {
                var playerComponent = player.GetComponent<PlayerComponent>();
                if(!playerComponent.IsFullHealth)
                {
                    _voice.Stop();
                    _voice.Play();
                    playerComponent.AddHealth(_healthAddCount);
                    GameObject.Parent.RemoveChildren(GameObject);
                    _colliderComponent.RemoveCollisionFromScene();
                }
            }
        }
    }
}
