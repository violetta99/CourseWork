using EngineLibrary.Graphics;
using EngineLibrary.Scripts;
using GameLibrary.Components;
using GameLibrary.Scripts.Game;
using SoundLibrary;

namespace GameLibrary.Scripts.Bonuses
{
    /// <summary>
    /// Класс бонуса, дающего стрелы игроку
    /// </summary>
    public class StandartArrowBonus : Script
    {
        private SharpAudioVoice _voice;
        private ColliderComponent _colliderComponent;

        private ArrowType _type;
        private int _arrowsCount;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="type">Тип добавляемых стрел</param>
        /// <param name="arrowsCount">Количество добавляемых стрел</param>
        public StandartArrowBonus(SharpAudioVoice voice, ArrowType type, int arrowsCount)
        {
            _voice = voice;
            _type = type;
            _arrowsCount = arrowsCount;
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
            if(_colliderComponent.CheckIntersects(out Game3DObject player, "player"))
            {
                _voice.Stop();
                _voice.Play();
                player.GetComponent<PlayerComponent>().AddArrows(_type, _arrowsCount);
                GameObject.Parent.RemoveChildren(GameObject);
                _colliderComponent.RemoveCollisionFromScene();
            }
        }
    }
}
