using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    /// <summary>
    /// Class responsible form IndexBuffer managament.
    /// </summary>
    public class BufferManager
    {
        int _active = 0;
        internal VertexBuffer VertexBuffer;
        public IndexBuffer[] _IndexBuffers;
        GraphicsDevice _device;
        /// <summary>
        /// Constructor of BufferManager class. Create two IndexBuffers,which are swaped when we need to do operations.
        /// Create also VertexBuffer ,which represent a list of 3D vertices to be streamed to the graphics device
        /// <param name="vertices">Array where vertices from terrain are store.<see cref="Map.MapRender.VertexMultitextured"/></param>
        /// <param name="device">GraphicsDevice</param>
        /// </summary>
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

        /// <summary>
        /// IndexBuffer represent rendering order of the vertices in VertexBuffer
        /// <returns>Active IndexBuffer</returns>
        /// </summary>
        public IndexBuffer IndexBuffer
        {
            get { return _IndexBuffers[_active]; }
        }
         /// <summary>
         /// Updating IndexBuffer by swapping IndexBuffer with <paramref name="indices"/> and <paramref name="indexCount"/>
         /// </summary>
        /// <param name="indices"></param>
        /// <param name="indexCount"></param>
        internal void UpdateIndexBuffer(int[] indices, int indexCount)
        {
            if (indices.Length > 0 && indexCount>0)
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
