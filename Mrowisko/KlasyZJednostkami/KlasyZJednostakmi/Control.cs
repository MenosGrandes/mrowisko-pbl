using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Map;
using GameCamera;


namespace Logic
{
    public class Control
    {
        public Matrix View;
        public Matrix Projection;
        public GraphicsDevice device;
        public List<Map.LoadModel> models;
        public Texture2D texture;
        public List<Map.LoadModel> SelectedModels= new List<LoadModel>();
        //private Vector3 playerTarget;
        private Vector2 selectCorner;
        private Rectangle selectRectangle;
        public LoadModel moving;
        //private Texture2D texture;
        Vector2 position;
        Vector3 position3d;
        MouseState currentMouseState;
        int f = 0;
        private bool mouseDown;
        private Vector3 startRectangle;
        private Vector3 endRectangle;
        
         public Control(Texture2D _texture)
        {
            this.texture = _texture;

        }
        public void Update(GameTime gameTime)
        {
            //pozycja mrowki
            float pozycja_X_lewo = models[0].Position.X - 800;
            float pozycja_X_prawo = models[0].Position.X + 800;
            float pozycja_Z_gora = models[0].Position.Z + 400;
            float pozycja_Z_dol = models[0].Position.Z - 800;

           
            currentMouseState = Mouse.GetState();
            Vector3 mouse3d2 = CalculateMouse3DPosition();

           
            if (currentMouseState.RightButton == ButtonState.Pressed && !mouseDown)
            {
                //SelectedModels.Clear();

                SelectedModels.Clear();
                mouseDown = true;
                position = new Vector2(currentMouseState.X, currentMouseState.Y);
                position3d = CalculateMouse3DPosition(position);
                selectCorner = position;
                selectRectangle = new Rectangle((int)position.X, (int)position.Y, 0, 0);

           
                //if ((mouse3d2.X > pozycja_X_lewo && mouse3d2.X < pozycja_X_prawo) && (mouse3d2.Z > pozycja_Z_dol && mouse3d2.Z < pozycja_Z_gora))
               
                
            }
            else if (currentMouseState.RightButton == ButtonState.Pressed)
            {

                SelectedModels.Clear();
                Selected();

                for (int i = 0; i < models.Count; i++)
                    if (models[i].Selected)
                    {
                        SelectedModels.Add(models[i]);
                    }
   

                selectCorner = new Vector2(currentMouseState.X, currentMouseState.Y);
                if (selectCorner.X > position.X)
                {
                    selectRectangle.X = (int)position.X;
                }
                else
                {
                    selectRectangle.X = (int)selectCorner.X;
                }

                if(selectCorner.Y > position.Y)
                {
                    selectRectangle.Y = (int)position.Y;
                }
                else
                {
                    selectRectangle.Y = (int)selectCorner.Y;
                }

                selectRectangle.Width = (int)Math.Abs(position.X - selectCorner.X);
                selectRectangle.Height = (int)Math.Abs(position.Y - selectCorner.Y);
            }
            else
            {
                mouseDown = false;
            }
            
            //Console.WriteLine(f);
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                // This will give the player a target to go to.
                foreach (var ant in SelectedModels)
                {
                    ant.playerTarget.X = mouse3d2.X;
                    ant.playerTarget.Z = mouse3d2.Z;
                }
            }

            updateAnt(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (mouseDown)
            {
                //spriteBatch.Draw(texture[11], selectRectangle, new Color(255, 255, 255, 180));
                spriteBatch.Draw(texture, new Rectangle(selectRectangle.X, selectRectangle.Y, selectRectangle.Width, 2), Color.White);
                spriteBatch.Draw(texture, new Rectangle(selectRectangle.X + selectRectangle.Width, selectRectangle.Y, 2, selectRectangle.Height), Color.White);
                spriteBatch.Draw(texture, new Rectangle(selectRectangle.X, selectRectangle.Y + selectRectangle.Height, selectRectangle.Width + 2, 2), Color.White);
                spriteBatch.Draw(texture, new Rectangle(selectRectangle.X, selectRectangle.Y, 2, selectRectangle.Height + 2), Color.White);
            }
        }
        void updateAnt(GameTime gameTime)
        {

            // Check if the player has reached the target, if not, move towards it. 
         
            foreach (var ant in models)
            {


                {
                    //if (ant.Selected)
                    {
                        if(Compare(ant.playerTarget, ant.Position))
                        {
                            //ant.Selected = false;
                            return;
                        }

                        float Speed = (float)10;
                        if (ant.Position.X > ant.playerTarget.X)
                        {
                            ant.Position += Vector3.Left * Speed;
                        }
                        if (ant.Position.X < ant.playerTarget.X)
                        {
                            ant.Position += Vector3.Right * Speed;
                        }

                        if (ant.Position.Z > ant.playerTarget.Z)
                        {
                            ant.Position += Vector3.Forward * Speed;
                        }
                        if (ant.Position.Z < ant.playerTarget.Z)
                        {
                            ant.Position += Vector3.Backward * Speed;
                        }


                    }
                }
            }
        }
        private void Selected()
        {
            startRectangle = CalculateMouse3DPosition(position);
            endRectangle = CalculateMouse3DPosition(selectCorner);
            //Console.WriteLine("START RECTANGLE"+startRectangle);
            //Console.WriteLine("END RECTANGLE" + endRectangle);
            //Console.WriteLine("ANT" + models[0].Position);
            Vector3 minRectangle;
            Vector3 maxRectangle;

            if (startRectangle.X < endRectangle.X)
            {
                minRectangle.X = startRectangle.X;
                maxRectangle.X = endRectangle.X;
            }
            else
            {
                minRectangle.X = endRectangle.X;
                maxRectangle.X = startRectangle.X;
            }
            if (startRectangle.Z < endRectangle.Z)
            {
                minRectangle.Z = startRectangle.Z;
                maxRectangle.Z = endRectangle.Z;
            }
            else
            {
                minRectangle.Z = endRectangle.Z;
                maxRectangle.Z = startRectangle.Z;
            }

            foreach (var ant in models)
            {
                if ((minRectangle.X < ant.Position.X && maxRectangle.X > ant.Position.X) &&
                (minRectangle.Z < ant.Position.Z && maxRectangle.Z > ant.Position.Z))
                {
                    ant.Selected = true;
                }
                else
                {
                    ant.Selected = false;
                }
            }
        }


        public static bool Compare(Vector3 v1, Vector3 v2)
        {
            bool equal = true;
            float precision = 0.1f;

            if (Math.Abs(v1.X - v2.X) > precision) equal = false;
            if (Math.Abs(v1.Y - v2.Y) > precision) equal = false;
            if (Math.Abs(v1.Z - v2.Z) > precision) equal = false;

            return equal;
        }
        private Vector3 CalculateMouse3DPosition()
        {
            Plane GroundPlane = new Plane(0, 1, 0, 0); // x - lewo prawo Z- gora dol
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            Vector3 nearsource = new Vector3((float)mouseX, (float)mouseY, 0f);
            Vector3 farsource = new Vector3((float)mouseX, (float)mouseY, 1f);

            Matrix world = Matrix.CreateTranslation(0, 0, 0);

            Vector3 nearPoint = device.Viewport.Unproject(nearsource,
                this.Projection, this.View, Matrix.Identity);

            Vector3 farPoint = device.Viewport.Unproject(farsource,
                this.Projection, this.View, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
            float? position = pickRay.Intersects(GroundPlane);

            if (position != null)
                return pickRay.Position + pickRay.Direction * position.Value;
            else
                return new Vector3(0, 0, 0);


        }


        private Vector3 CalculateMouse3DPosition(Vector2 pos)
        {
            Plane GroundPlane = new Plane(0, 1, 0, 0); // x - lewo prawo Z- gora dol
            int mouseX = (int)pos.X;
            int mouseY = (int)pos.Y;

            Vector3 nearsource = new Vector3((float)mouseX, (float)mouseY, 0f);
            Vector3 farsource = new Vector3((float)mouseX, (float)mouseY, 1f);

            Matrix world = Matrix.CreateTranslation(0, 0, 0);

            Vector3 nearPoint = device.Viewport.Unproject(nearsource,
                this.Projection, this.View, Matrix.Identity);

            Vector3 farPoint = device.Viewport.Unproject(farsource,
                this.Projection, this.View, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
            float? position = pickRay.Intersects(GroundPlane);

            if (position != null)
                return pickRay.Position + pickRay.Direction * position.Value;
            else
                return new Vector3(0, 0, 0);


        }
        private Vector3 CalculateMouse3DPosition(int X, int Y)
        {
            Plane GroundPlane = new Plane(0, 1, 0, 0); // x - lewo prawo Z- gora dol
            int mouseX = X;
            int mouseY = Y;

            Vector3 nearsource = new Vector3((float)mouseX, (float)mouseY, 0f);
            Vector3 farsource = new Vector3((float)mouseX, (float)mouseY, 1f);

            Matrix world = Matrix.CreateTranslation(0, 0, 0);

            Vector3 nearPoint = device.Viewport.Unproject(nearsource,
                this.Projection, this.View, Matrix.Identity);

            Vector3 farPoint = device.Viewport.Unproject(farsource,
                this.Projection, this.View, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
            float? position = pickRay.Intersects(GroundPlane);

            if (position != null)
                return pickRay.Position + pickRay.Direction * position.Value;
            else
                return new Vector3(0, 0, 0);


        }


    }
}
