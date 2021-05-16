using EngineLibrary.Components;
using EngineLibrary.Graphics;
using EngineLibrary.Scripts;
using System;
using System.Collections.Generic;

namespace EngineLibrary.Utils
{
    public class CopyableGameObject
    {
        public Game3DObject GameObject { get; set; }

        public List<Func<Script>> Scripts { get; set; }

        public List<Func<ObjectComponent>> Components { get; set; }

        public CopyableGameObject(Game3DObject gameObject, List<Func<Script>> scripts, List<Func<ObjectComponent>> components )
        {
            GameObject = gameObject;
            Scripts = scripts;
            Components = components;
        }

        public Game3DObject Copy()
        {
            var copy = GameObject.GetCopy();
            foreach (var component in Components)
            {
                var componentObject = component.Invoke();
                copy.AddComponent(componentObject, componentObject.GetType());
            }

            foreach (var script in Scripts)
            {
                copy.AddScript(script.Invoke());
            }

            return copy;
        }
    }
}
