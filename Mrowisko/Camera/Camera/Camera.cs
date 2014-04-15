using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameCamera
{
    public abstract class Camera
    {

        protected GraphicsDevice GraphicsDevice { get; set; }
        public BoundingFrustum Frustrum { get; private set; }
        Matrix view;
        Matrix projection;
        public Matrix Projection
        {
            get { return projection; }
            protected set
            {
                projection = value;
                generateFrustum();
            }
        }
        public Matrix View
        {
            get { return view; }
            protected set
            {
                view = value;
                generateFrustum();
            }
        }
        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            generatePerspectiveProjectionMatrix(MathHelper.PiOver4);
        }
        private void generatePerspectiveProjectionMatrix(float FieldOfView)
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            float aspectRatio = (float)pp.BackBufferWidth /
            (float)pp.BackBufferHeight; this.Projection = Matrix.CreatePerspectiveFieldOfView(
             MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000000.0f);
        }
        public virtual void Update(GameTime gameTime)
        {
        }
        
        private void generateFrustum()
        {
            Matrix viewProjection = View * Projection;
            Frustrum = new BoundingFrustum(viewProjection);
        }
        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            return (Frustrum.Contains(sphere) != ContainmentType.Disjoint);
        }
        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            return (Frustrum.Contains(box) != ContainmentType.Disjoint);
        }
    }
}
