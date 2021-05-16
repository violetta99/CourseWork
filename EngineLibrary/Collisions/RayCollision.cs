using EngineLibrary.Graphics;
using SharpDX;
using SharpDX.Direct3D;

namespace EngineLibrary.Collisions
{
    public class RayCollision : ObjectCollision
    {
        private readonly Ray _ray;

        public RayCollision(Ray ray)
        {
            _ray = ray;
        }

        public override ObjectCollision GetCopy()
        {
            return new RayCollision(_ray);
        }

        public override MeshObject GetMesh()
        {
            var vertices = new Renderer.VertexDataStruct[]
           {
                new Renderer.VertexDataStruct()
                {
                   position = new Vector4(_ray.Position, 1),
                   color = Vector4.One,
                },

                new Renderer.VertexDataStruct()
                {
                   position = new Vector4(_ray.Direction + _ray.Position, 1),
                   color = Vector4.One,
                },
           };

            var indices = new uint[]
            {
                0,
                1,
            };

            var result = new MeshObject(
                DirectX3DGraphics.GetInstance(),
                vertices,
                indices,
                PrimitiveTopology.LineList,
                new Material(null, Vector3.Zero, Vector3.One, Vector3.One, Vector3.One, 1),
                true, "", false);

            return result;
        }

        protected override object GetCollision()
        {
            return _ray;
        }
    }
}
