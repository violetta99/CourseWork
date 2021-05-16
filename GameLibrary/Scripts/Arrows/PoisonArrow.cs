using EngineLibrary.Graphics;
using GameLibrary.Components;

namespace GameLibrary.Scripts.Arrows
{
    /// <summary>
    /// Класс стрелы с ядом
    /// </summary>
    public class PoisonArrow : Arrow
    {
        private float _slowTime;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="spawn">Объект, относительно которого появляется стрела</param>
        /// <param name="speed">Базовая скорость полёта стрелы</param>
        /// <param name="lifeTime">Время существования стрелы</param>
        /// <param name="slowTime">Время замедления врага</param>
        public PoisonArrow(Game3DObject spawn, float speed, float lifeTime, float slowTime) 
            : base(spawn, speed, lifeTime)
        {
            _slowTime = slowTime;
        }

        /// <summary>
        /// Обработка попадания стрелы во врага
        /// </summary>
        /// <param name="enemy">Объект врага</param>
        protected override void HandleHitEnemy(Game3DObject enemy)
        {
            enemy.GetComponent<EnemyComponent>().MakeSlow(_slowTime);
            base.HandleHitEnemy(enemy);
        }
    }
}
