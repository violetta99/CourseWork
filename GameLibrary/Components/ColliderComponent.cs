using EngineLibrary.Collisions;
using EngineLibrary.Components;
using EngineLibrary.Graphics;
using System.Collections.Generic;

namespace GameLibrary.Components
{
    /// <summary>
    /// Компонент коллайдера
    /// </summary>
    public class ColliderComponent : ObjectComponent
    {
        private static List<ObjectCollision> _collisions = new List<ObjectCollision>();

        /// <summary>
        /// Добавление коллайдера на сцену
        /// </summary>
        public void AddCollisionOnScene()
        {
            _collisions.Add(GameObject.Collision);
        }

        /// <summary>
        /// Удаление коллайдера на сцене
        /// </summary>
        public void RemoveCollisionFromScene()
        {
            _collisions.Remove(GameObject.Collision);
        }

        /// <summary>
        /// Проверка столкновения коллайдера с другими объектами с тегам
        /// </summary>
        /// <param name="gameObject">Пересекаемый объект или NULL</param>
        /// <param name="tag">Тэг объкта, с которым проверяется пересечение</param>
        /// <returns>Результат пересечения</returns>
        public bool CheckIntersects(out Game3DObject gameObject, string tag = "")
        {
            foreach(var collision in _collisions)
            {
                if (tag == "" || collision.GameObject.Tag != tag)
                    continue;

                if(collision != GameObject.Collision)
                {
                    var result = ObjectCollision.Intersects(GameObject.Collision, collision);
                    if(result)
                    {
                        gameObject = collision.GameObject;
                        return result;
                    }
                }
            }

            gameObject = null;
            return false;
        }
    }
}
