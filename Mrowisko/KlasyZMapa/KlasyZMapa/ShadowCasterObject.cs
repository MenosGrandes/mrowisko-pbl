using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Map
{
    public class ShadowCasterObject
    {
        //public VertexDeclaration VertexDecl;
        public VertexBuffer VertexBuffer;
        public int StreamOffset;
       // public int VertexStride;
        public IndexBuffer IndexBuffer;
       // public int BaseVertex;
        public int VerticesCount;
        public int StartIndex;
        public int PrimitiveCount;
        public Matrix World;
      

        public ShadowCasterObject(
                       // VertexDeclaration VertexDecl,
                        VertexBuffer VertexBuffer,
                        int StreamOffset,
                       // int VertexStride,
                        IndexBuffer IndexBuffer,
                      //  int BaseVertex,
                        int VerticesCount,
                        int StartIndex,
                        int PrimitiveCount,
                        Matrix World
            )
        {
           // this.VertexDecl = VertexDecl;
            this.VertexBuffer = VertexBuffer;
            this.StreamOffset = StreamOffset;
          //  this.VertexStride = VertexStride;
            this.IndexBuffer = IndexBuffer;
          //  this.BaseVertex = BaseVertex;
            this.VerticesCount = VerticesCount;
            this.StartIndex = StartIndex;
            this.PrimitiveCount = PrimitiveCount;
            this.World = World;
        }

        public ShadowCasterObject()
        {

        }
    }
}
