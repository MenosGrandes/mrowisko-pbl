using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    internal class BufferManager
    {
        int _active = 0;
        internal VertexBuffer VertexBuffer;
        IndexBuffer[] _IndexBuffers;
        GraphicsDevice _device;

        internal BufferManager(VertexMultitextured[] vertices, GraphicsDevice device)
        {
            _device = device;

            VertexBuffer = new VertexBuffer(device, VertexMultitextured.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(vertices);

            _IndexBuffers = new IndexBuffer[]
                    {
                            new IndexBuffer(_device, IndexElementSize.ThirtyTwoBits, 100000, BufferUsage.WriteOnly),
                            new IndexBuffer(_device, IndexElementSize.ThirtyTwoBits, 100000, BufferUsage.WriteOnly)
                    };

        }


        internal IndexBuffer IndexBuffer
        {
            get { return _IndexBuffers[_active]; }
        }

        internal void UpdateIndexBuffer(int[] indices, int indexCount)
        {
            if (indices.Length > 0)
            {
                int inactive = _active == 0 ? 1 : 0;

                _IndexBuffers[inactive].SetData(indices, 0, indexCount);
            }

        }

        internal void SwapBuffer()
        {
            _active = _active == 0 ? 1 : 0; ;
        }
    }
}
