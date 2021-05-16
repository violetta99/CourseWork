using EngineLibrary.Game;
using EngineLibrary.Scripts;
using EngineLibrary.Utils;
using GameLibrary.Components;
using SharpDX;
using SharpDX.DirectInput;
using System.Collections.Generic;

namespace GameLibrary.Scripts.Game
{
    /// <summary>
    /// Класс поведения лука
    /// </summary>
    public class BowShoot : KeyboardListenerScript
    {
        private PlayerComponent _playerComponent; 
        private VisibleComponent _visibleComponent;
        private Dictionary<ArrowType, CopyableGameObject> _arrowTemplates;

        private Vector3 _originalPosition;

        private int _currentArrowIndex = 0;
        private float _visibilityDuration;
        private float _processCoef;
        private float _process = 0.0f;
        private float _maxProcess = 1.0f;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="processDuration">Зависимость процесса натяжения</param>
        /// <param name="visibilityDuration">Время видимости</param>
        public BowShoot(float processDuration = 5.0f, float visibilityDuration = 1.0f)
        {
            _arrowTemplates = new Dictionary<ArrowType, CopyableGameObject>();
            _processCoef = 1.0f / processDuration * 10.0f;
            _visibilityDuration = visibilityDuration;

            Actions.Add(Key.R, (delta) =>
            {
                if (!_visibleComponent.IsHidden)
                {
                    ChangeArrow();
                }
            });
        }

        /// <summary>
        /// Добавление дублируемого образца стрел
        /// </summary>
        /// <param name="type">Тип стрелы</param>
        /// <param name="arrowTemplate">Образец</param>
        public void AddArrowTemplate(ArrowType type, CopyableGameObject arrowTemplate)
        {
            _arrowTemplates.Add(type, arrowTemplate);
        }

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public override void Init()
        {
            _originalPosition = GameObject.Position;
            _visibleComponent = GameObject.GetComponent<VisibleComponent>();
        }

        /// <summary>
        /// Обновления поведения после нажатий клавиш
        /// </summary>
        /// <param name="delta">Время между кдарами</param>
        protected override void AfterKeyProcess(float delta)
        {
            if (_visibleComponent.IsHidden) return;

            if (_playerComponent == null)
                _playerComponent = GameObject.Parent.GetComponent<PlayerComponent>();

            if (!_playerComponent.IsHasArrows((ArrowType)_currentArrowIndex))
            {
                GameObject.IsHidden = true;
                return;
            }

            UpdateShootingProcess(delta);
        }

        /// <summary>
        /// Обновление процесса натяжения
        /// </summary>
        /// <param name="delta">Время между кадрами</param>
        private void UpdateShootingProcess(float delta)
        {
            var inputController = InputController.GetInstance();
            // если в этом кадре была нажата ЛКМ
            if (inputController.MouseButtons[0])
            {
                // изменяем натяжение
                _process += delta;
                if (_process >= _maxProcess)
                {
                    _process = _maxProcess;
                }

                // двигаем объект стрелы на луке
                var dest = new Vector3(0, 0, -_process / _processCoef);
                GameObject.MoveTo(_originalPosition + dest);
                // изменяем состояние прицеливания
                _playerComponent.IsAiming = true;
            }
            else
            {
                // если стрелу в прошлых кадрах НАТЯГИВАЛИ
                if (_process != 0)
                {
                    GameObject.MoveTo(_originalPosition);
                    _visibleComponent.MakeInvisible(_visibilityDuration);
                    Shoot();
                    _process = 0;
                }
                // изменяем состояние прицеливания
                _playerComponent.IsAiming = false;
            }
        }

        /// <summary>
        /// Смена стрел
        /// </summary>
        private void ChangeArrow()
        {
            _visibleComponent.MakeInvisible(_visibilityDuration);
            var arrows = GameObject.Children;

            _currentArrowIndex++;
            if (_currentArrowIndex >= arrows.Count)
                _currentArrowIndex = 0;

            for(int i = 0; i < arrows.Count; i++)
            {
                arrows[i].IsHidden = _currentArrowIndex != i;
            }
        }

        /// <summary>
        /// Создаем стрелу
        /// </summary>
        private void Shoot()
        {
            // получаем копию из образца
            var arrow = _arrowTemplates[(ArrowType)_currentArrowIndex].Copy();
            // настраиваем объект
            arrow.GetComponent<ArrowComponent>().FiringForceCoef = _process;
            GameObject.Scene.AddGameObject(arrow);
            arrow.MoveTo(GameObject.Position);
            arrow.IsHidden = false;

            _playerComponent.RemoveArrow((ArrowType)_currentArrowIndex);
        }
    }

    /// <summary>
    /// Тип стрелы
    /// </summary>
    public enum ArrowType
    {
        /// <summary>
        /// Обычная стрела
        /// </summary>
        Standart = 0,
        /// <summary>
        /// Стрелы с ядом
        /// </summary>
        Poison,
    }
}
