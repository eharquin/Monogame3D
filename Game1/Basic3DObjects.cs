using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game1
{
    class Basic3DObjects
    {
        public class Object3D
        {
            public int StartIndex; // where it is in the index buffer
            public int TriangleCount; // how many triangles
            public Rectangle SourceRect; // section xithin texture to sample from
            public Texture2D Texture;
            public Vector3 Rotation; // optional rotation
            public Vector3 Position; // position - used with position translation part of transform matrix (or only it
            public Matrix Transform; // world transform matrix (ie: scale * rotation * position_translation) [in that order]

            // IF POSITION CHANGES (and-or roation)
            public void UpdateTransform()
            {
                if (Rotation == Vector3.Zero) Transform = Matrix.CreateTranslation(Position);
                else Transform = Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
            }

            // INIT
            protected void Init(Vector3 Pos, string file)
            {
                StartIndex = _iBufStart;
                Position = Pos; Transform = Matrix.CreateTranslation(Pos);
                Texture = LoadTexture(file);
                if ((SourceRect.Width < 1) || (SourceRect.Height < 1)) SourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
            }

            // GET UV COORDS
            protected void GetUVCoords(ref float u1, ref float v1, ref float u2, ref float v2)
            {
                u1 = SourceRect.X / (float)Texture.Width;                                                // get the uv coords (texture map coords)
                v1 = SourceRect.Y / (float)Texture.Height;
                u2 = (SourceRect.X + SourceRect.Width) / (float)Texture.Width;
                v2 = (SourceRect.Y + SourceRect.Height) / (float)Texture.Height;
            }

            // ADD QUAD 
            public void AddQuad(Vector3 Pos, float width, float height, Vector3 rotation, string textureFile, Rectangle? sourceRect)
            {
                if (sourceRect.HasValue) sourceRect = sourceRect.Value;
                Init(Pos, textureFile); Rotation = rotation; UpdateTransform();                          // setup intial transform matrix 
                float u1 = 0, v1 = 0, u2 = 1, v2 = 1, hw = width / 2, hl = height / 2;              // uv's, half-width, half-length
                GetUVCoords(ref u1, ref v1, ref u2, ref v2);
                Vector3 norm = Vector3.Up;                                                          // initial normal (before rotated by transform)
                float y = Pos.Y, l = -hw + Pos.X, r = hw + Pos.X, n = -hl + Pos.Z, f = hl + Pos.Z;  // y-coord, left, right, near, far                    
                AddVertex(l, y, f, norm, u1, v1); AddVertex(r, y, f, norm, u2, v1); AddVertex(r, y, n, norm, u2, v2); // (left,y,far),(right,y,far),(right,y,near) [clockwise]
                AddVertex(l, y, n, norm, u1, v2);
                AddTriangle(0, 1, 2); TriangleCount++;                                             // clockwise order
                AddTriangle(2, 3, 0); TriangleCount++;
                _vertexBuffer.SetData<VertexPositionNormalTexture>(_vBufStart * _vBytes, _verts, 0, _vCount, _vBytes); _vBufStart = _vCount; _vCount = 0;
                _indexBuffer.SetData<ushort>(_iBufStart * _iBytes, _indices, 0, _iCount); _iBufStart = _iCount; _iCount = 0;
            }

            // ADD CUBE
            public void AddCube(Vector3 Pos, Vector3 size, Vector3 rotation, string textureFile, Rectangle? sourceRect)
            {
                if (sourceRect.HasValue) SourceRect = sourceRect.Value;
                Init(Pos, textureFile); Rotation = rotation; UpdateTransform();                             // setup intial transform matrix 
                float u1 = 0, v1 = 0, u2 = 1, v2 = 1, hw = size.X / 2, hl = size.Z / 2, hh = size.Y / 2; // uv's, half-width, half-length, half-height
                GetUVCoords(ref u1, ref v1, ref u2, ref v2);
                float t = Pos.Y - hh, b = Pos.Y + hh, l = Pos.X - hw, r = Pos.X + hw, n = Pos.Z - hl, f = Pos.Z + hl;  // y-coord, left, right, near, far                                
                Vector3 norm = Vector3.Up; AddVertex(l, t, f, norm, u1, v1); AddVertex(r, t, f, norm, u2, v1); AddVertex(r, t, n, norm, u2, v2); AddVertex(l, t, n, norm, u1, v2); // (left,y,far),(right,y,far),(right,y,near) [clockwise]
                norm = Vector3.Right; AddVertex(r, b, f, norm, u1, v1); AddVertex(r, b, n, norm, u1, v2);
                norm = Vector3.Down; AddVertex(l, b, f, norm, u2, v1); AddVertex(l, b, n, norm, u2, v2);
                norm = Vector3.Backward; AddVertex(l, t, n, norm, u1, v1); AddVertex(r, t, n, norm, u2, v1); AddVertex(r, b, n, norm, u2, v2); AddVertex(l, b, n, norm, u1, v2);
                norm = Vector3.Forward; AddVertex(r, t, f, norm, u1, v1); AddVertex(l, t, f, norm, u2, v1); AddVertex(l, b, f, norm, u2, v2); AddVertex(r, b, f, norm, u1, v2);
                AddTriangle(0, 1, 2); TriangleCount++; AddTriangle(2, 3, 0); TriangleCount++;  // clockwise order
                AddTriangle(2, 1, 4); TriangleCount++; AddTriangle(4, 5, 2); TriangleCount++;
                AddTriangle(5, 4, 6); TriangleCount++; AddTriangle(6, 7, 5); TriangleCount++;
                AddTriangle(7, 6, 0); TriangleCount++; AddTriangle(0, 3, 7); TriangleCount++;
                AddTriangle(8, 9, 10); TriangleCount++; AddTriangle(10, 11, 8); TriangleCount++;
                AddTriangle(12, 13, 14); TriangleCount++; AddTriangle(14, 15, 12); TriangleCount++;
                _vertexBuffer.SetData<VertexPositionNormalTexture>(_vBufStart * _vBytes, _verts, 0, _vBytes, _vBytes); _vBufStart = _vCount; _vCount = 0;
                _indexBuffer.SetData<ushort>(_iBufStart * _iBytes, _indices, 0, _iCount); _iBufStart = _iCount; _iCount = 0;
            }
        }

        // COMMON
        private GraphicsDevice _graphicsDevice;
        //private Light light; 
        private BasicEffect _basicEffect;
        private Vector3 _upDirection;
        private const int _vBytes = sizeof(float) * 8, _iBytes = sizeof(ushort);
        
        // GEO
        private Matrix _world; // for fixed geometry
        public List<Object3D> Objects; // list of 3D objects to render
        // static private allows nested classes to access
        private static ContentManager _content; // holds and controls transfer of indices to hardware
        private static IndexBuffer _indexBuffer; // holds and controls transfer of vertices to hardware
        private static VertexBuffer _vertexBuffer; // index list for index buffer
        private static ushort[] _indices; // index list for index buffer  
        private static VertexPositionNormalTexture[] _verts; // vertex list for assembling vertex buffer
        private static int _iCount, _iBufStart = 0; // for making texture-sort id's, index-buffer's current starting position (for adding new objects)  
        private static int _vCount, _vBufStart = 0; // v_cnt is for making current object, total_verts keeps track of how many were already added (and can be used as starting index when adding new ones)
        private static Dictionary<string, Texture2D> _textures; // helps to store textures only once and get desired texture based on associated object's texture-file-name
        
        // CONSTRUCTOR
        public Basic3DObjects(GraphicsDevice graphicsDevice, Vector3 upDirection, ContentManager content)
        {
            _graphicsDevice = graphicsDevice;
            _world = Matrix.Identity;
            _basicEffect = new BasicEffect(_graphicsDevice);
            _verts = new VertexPositionNormalTexture[65535];
            _indices = new ushort[65535];
            _content = content;
            _upDirection = upDirection;
            _textures = new Dictionary<string, Texture2D>();
            _indexBuffer = new IndexBuffer(_graphicsDevice, typeof(ushort), 65535, BufferUsage.WriteOnly);
            _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionNormalTexture), 65535, BufferUsage.WriteOnly);
            Objects = new List<Object3D>();

            // INIT
            _basicEffect.Alpha = 1f;                // I think this is default anyway (transparency)
            _basicEffect.LightingEnabled = true;  // vertex type needs normals for this to work (which we have)
            _basicEffect.AmbientLightColor = new Vector3(0.1f, 0.2f, 0.3f);    // medium-dark for dark parts of object
            _basicEffect.DiffuseColor = new Vector3(0.94f, 0.94f, 0.94f); // fairly bright for lit parts of object
            _basicEffect.EnableDefaultLighting();   // make sure lighting is on
            _basicEffect.TextureEnabled = true;  // make sure this is enabled


        }

        // ADD VERTEX
        static void AddVertex(float x, float y, float z, Vector3 norm, float u, float v)
        {
            if ((_vBufStart + _vCount) > 65530) { Debug.WriteLine("exceeded vertex buffer size"); return; }
            _verts[_vCount] = new VertexPositionNormalTexture(new Vector3(x, y, z), norm, new Vector2(u, v));
            _vCount++;
        }

        // ADD TRIANGLE
        static void AddTriangle(ushort a, ushort b, ushort c)
        {
            if ((_iBufStart + 3) > 65530) { Debug.WriteLine("exceeded index buffer size [may need UIint32 type]"); return; }
            ushort offset = (ushort)_vBufStart;
            a += offset; b += offset; c += offset;
            _indices[_iCount] = a; _iCount++; _indices[_iCount] = b; _iCount++; _indices[_iCount] = c; _iCount++;  // MUST CAST AGAIN due to math always converts to int32
        }

        // LOAD TEXTURE (or point to existing one)
        static Texture2D LoadTexture(string name)
        {
            Texture2D texture;
            if (_textures.TryGetValue(name, out texture) == true)
            {
                return texture; // if already in list, just return reference to the one found
            }
            else
            {
                texture = _content.Load<Texture2D>(name);
                _textures.Add(name, texture);
                return texture;
            }
        }

        // BASIC OBJECTS THAT YOU CAN ADD ------------------------------ 
        // BASIC FLOOR 
        public void AddFloor(float width, float length, Vector3 mid_position, Vector3 rotation, string textureFile, Rectangle? sourceRect)
        {
            Object3D obj = new Object3D();
            obj.AddQuad(mid_position, width, length, rotation, textureFile, sourceRect);
            Objects.Add(obj);
        }
        // BASIC CUBE
        public void AddCube(float width, float length, float height, Vector3 mid_position, Vector3 rotation, string textureFile, Rectangle? sourceRect)
        {
            Object3D obj = new Object3D();
            obj.AddCube(mid_position, new Vector3(width, height, length), rotation, textureFile, sourceRect);
            Objects.Add(obj);
        }
        //--------------------------------------------------------------


        // DRAW  
        public void Draw(Camera cam)
        {
            _graphicsDevice.SetVertexBuffer(_vertexBuffer);
            _graphicsDevice.Indices = _indexBuffer;
            int obj_cnt = Objects.Count, o;
            o = 0; while (o < obj_cnt)
            {
                Object3D ob = Objects[o];
                #region //[Reminder_To_Set_Lighting_Draw_Params_Later for custom lighting class and effect]
                // TO DO (later): 
                // if (DrawDepth)        light.SetDepthParams(ob.transform);           // for drawing to a depth shader
                // else if (DrawShadows) light.SetShadowParams(ob.transform, cam);     // for drawing shadows (using depth shader results) 
                // else                  light.SetDrawParams(ob.transform,cam,ob.tex); // regular drawing
                #endregion
                // SET SHADER PARAMETERS: 
                _basicEffect.Texture = ob.Texture;
                _basicEffect.World = ob.Transform;
                _basicEffect.View = cam.View;
                _basicEffect.Projection = cam.Projection;
                foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, ob.StartIndex, ob.TriangleCount); o++;
                }
            }
        }
    }
}
