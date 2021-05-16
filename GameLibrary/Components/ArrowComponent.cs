using EngineLibrary.Components;

namespace GameLibrary.Components
{
    /// <summary>
    /// Компонент стрелы
    /// </summary>
    public class ArrowComponent : ObjectComponent
    {
        /// <summary>
        /// Коэффициент натяжения лука, влияющий на скорость полёта стрелы
        /// </summary>
        public float FiringForceCoef { get; set; }
    }
}
