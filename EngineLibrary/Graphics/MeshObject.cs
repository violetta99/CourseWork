using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace EngineLibrary.Graphics
{
    public class MeshObject : IDisposable
    {
        private DirectX3DGraphics _directX3DGraphics;
        private string _name;
        private int _verticesCount;
        private Renderer.VertexDataStruct[] _vertices;
        private Buffer11 _vertexBufferObject;
        private VertexBufferBinding _vertexBufferBinding;
        public VertexBufferBinding VertexBufferBinding { get => _vertexBufferBinding; }

        private int _indicesCount;
        public int IndicesCount { get => _indicesCount; }
        private uint[] _indices;
        private Buffer11 _indicesBufferObject;
        public Buffer11 IndicesBufferObject { get => _indicesBufferObject; }

        private PrimitiveTopology _primitiveTopology;
        public PrimitiveTopology PrimitiveTopology { get => _primitiveTopology; }

        private Material _material;
        public Material Material { get => _material; }

        public Material MaterialNormal { get; set; }

        private bool _visible;
        public bool Visible { get => _visible; set => _visible = value; }

        public bool NormalVisible { get; private set; }

        private VertexBufferBinding _vertexBufferNormalBinding;
        public VertexBufferBinding VertexBufferNormalBinding { get => _vertexBufferNormalBinding; }

        private Buffer11 _indicesBufferNormalObject;
        public Buffer11 IndicesBufferNormalObject { get => _indicesBufferNormalObject; }
        public string Name { get => _name; set => _name = value; }

        public MeshObject(DirectX3DGraphics directX3DGraphics, Renderer.VertexDataStruct[] vertices, uint[] indices, PrimitiveTopology primitiveTopology, Material material, bool visible, string name, bool normalVisible = false)
        {
            _directX3DGraphics = directX3DGraphics;
            _vertices = vertices;
            _verticesCount = _vertices.Length;
            _indices = indices;
            _indicesCount = _indices.Length;
            _primitiveTopology = primitiveTopology;
            _material = material;
            _visible = visible;
            _name = name;
            _vertexBufferObject = Buffer11.Create(
                _directX3DGraphics.Device,
                BindFlags.VertexBuffer,
                _vertices,
                Utilities.SizeOf<Renderer.VertexDataStruct>() * _verticesCount);
            _vertexBufferBinding = new VertexBufferBinding(
                _vertexBufferObject,
                Utilities.SizeOf<Renderer.VertexDataStruct>(),
                0);
            _indicesBufferObject = Buffer11.Create(
                _directX3DGraphics.Device,
                BindFlags.IndexBuffer,
                _indices,
                Utilities.SizeOf<uint>() * _indicesCount);

            NormalVisible = normalVisible;

            if (normalVisible)
            {
                uint[] normalIndex = new uint[vertices.Length * 2];
                for (uint i = 0; i < normalIndex.Length; i++)
                {
                    normalIndex[i] = i;
                }
                _indicesBufferNormalObject = Buffer11.Create(
                _directX3DGraphics.Device,
                    BindFlags.IndexBuffer,
                    normalIndex,
                    Utilities.SizeOf<uint>() * normalIndex.Length);
                int k = 0;
                Renderer.VertexDataStruct[] normal = new Renderer.VertexDataStruct[vertices.Length * 2];
                for (int i = 0; i < vertices.Length; i++)
                {
                    normal[k++] = new Renderer.VertexDataStruct
                    {
                        position = new Vector4(vertices[i].position.X, vertices[i].position.Y, vertices[i].position.Z, 1.0f),
                        color = new Vector4(0.2f, 0.2f, 0.2f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.0f)
                    };
                    normal[k++] = new Renderer.VertexDataStruct
                    {
                        position = new Vector4(vertices[i].position.X + vertices[i].normal.X,
                                vertices[i].position.Y + vertices[i].normal.Y,
                                vertices[i].position.Z + vertices[i].normal.Z, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.0f)
                    };
                }
                _vertexBufferObject = Buffer11.Create(
                    _directX3DGraphics.Device,
                    BindFlags.VertexBuffer,
                    normal,
                    Utilities.SizeOf<Renderer.VertexDataStruct>() * normal.Length);
                _vertexBufferNormalBinding = new VertexBufferBinding(
                    _vertexBufferObject,
                    Utilities.SizeOf<Renderer.VertexDataStruct>(),
                    0);

            }
        }

        public void Dispose()
        {
            Utilities.Dispose(ref _indicesBufferObject);
            Utilities.Dispose(ref _vertexBufferObject);
            Utilities.Dispose(ref _indicesBufferNormalObject);
        }
    }
}


//using System;
//using SharpDX;
//using SharpDX.Direct3D;
//using SharpDX.Direct3D11;
//using Buffer11 = SharpDX.Direct3D11.Buffer;

//namespace GameEngine
//{
//    class MeshObject : Game3DObject, IDisposable
//    {
//        public BoundingBox OuterBoundingBox
//        {
//            get
//            {
//                var worldMatrix = GetWorldMatrix();
//                var position = Matrix.Multiply(Matrix.Translation((Vector3)_vertices[0].position), worldMatrix).Row4;
//                var min = (Vector3)position;
//                var max = min;

//                for (int i = 1; i < _verticesCount; i++)
//                {
//                    position = Matrix.Multiply(Matrix.Translation((Vector3)_vertices[i].position), worldMatrix).Row4;
//                    UpdateMinMaxPosition(position, ref min, ref max);
//                }

//                return new BoundingBox(min, max);
//            }
//        }

//        public BoundingBox InnerBoundingBox
//        {
//            get
//            {
//                var position = _vertices[0].position;
//                var min = (Vector3)position;
//                var max = min;
//                var objectPosition = (Vector3)_position;

//                for (int i = 1; i < _verticesCount; i++)
//                {
//                    position = _vertices[i].position;
//                    UpdateMinMaxPosition(position, ref min, ref max);
//                }

//                return new BoundingBox(min + objectPosition, max + objectPosition);
//            }
//        }

//        private static void UpdateMinMaxPosition(Vector4 position, ref Vector3 min, ref Vector3 max)
//        {
//            if (position.X > max.X) max.X = position.X;
//            if (position.X < min.X) min.X = position.X;
//            if (position.Y > max.Y) max.Y = position.Y;
//            if (position.Y < min.Y) min.Y = position.Y;
//            if (position.Z > max.Z) max.Z = position.Z;
//            if (position.Z < min.Z) min.Z = position.Z;
//        }

//        private DirectX3DGraphics _directX3DGraphics;

//        private int _verticesCount;
//        private Renderer.VertexDataStruct[] _vertices;
//        private Buffer11 _vertexBufferObject;
//        private VertexBufferBinding _vertexBufferBinding;
//        public VertexBufferBinding VertexBufferBinding { get => _vertexBufferBinding; }

//        private int _indicesCount;
//        public int IndicesCount { get => _indicesCount; }
//        private uint[] _indices;
//        private Buffer11 _indicesBufferObject;
//        public Buffer11 IndicesBufferObject { get => _indicesBufferObject; }

//        private PrimitiveTopology _primitiveTopology;
//        public PrimitiveTopology PrimitiveTopology { get => _primitiveTopology; }

//        private Material _material;
//        public Material Material { get => _material; }

//        private bool _visible;
//        public bool Visible { get => _visible; set => _visible = value; }

//        public MeshObject(DirectX3DGraphics directX3DGraphics, 
//            Vector4 position, float yaw, float pitch, float roll, 
//            Renderer.VertexDataStruct[] vertices, uint[] indices,
//            PrimitiveTopology primitiveTopology, Material material, 
//            bool visible)
//            : base(position, yaw, pitch, roll)
//        {
//            _directX3DGraphics = directX3DGraphics;
//            _vertices = vertices;
//            _verticesCount = _vertices.Length;
//            _indices = indices;
//            _indicesCount = _indices.Length;
//            _primitiveTopology = primitiveTopology;
//            _material = material;
//            _visible = visible;

//            _vertexBufferObject = Buffer11.Create(
//                _directX3DGraphics.Device,
//                BindFlags.VertexBuffer,
//                _vertices,
//                Utilities.SizeOf<Renderer.VertexDataStruct>() * _verticesCount);
//            _vertexBufferBinding = new VertexBufferBinding(
//                _vertexBufferObject,
//                Utilities.SizeOf<Renderer.VertexDataStruct>(),
//                0);
//            _indicesBufferObject = Buffer11.Create(
//                _directX3DGraphics.Device,
//                BindFlags.IndexBuffer,
//                _indices,
//                Utilities.SizeOf<uint>() * _indicesCount);
//        }

//        public void Dispose()
//        {
//            Utilities.Dispose(ref _indicesBufferObject);
//            Utilities.Dispose(ref _vertexBufferObject);
//        }
//    }
//}
