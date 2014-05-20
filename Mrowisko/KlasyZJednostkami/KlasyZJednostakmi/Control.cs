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
        public List<InteractiveModel> models;
        public List<float> angles = new List<float>();
        public Texture2D texture;
        public List<InteractiveModel> SelectedModels = new List<InteractiveModel>();
        public List<InteractiveModel> IModel = new List<InteractiveModel>();
        public Ray ray;
        public Vector3 angle = new Vector3(0, 0, 0);
        //private Vector3 playerTarget;
        private Vector2 selectCorner;
        private Rectangle selectRectangle;
        public LoadModel moving;

        //tabele wysokosci
        private float[,] heights;
        public int width;
        public int length;


        //private Texture2D texture;
        Vector2 position;
        Vector3 position3d;
        MouseState currentMouseState;
        int f = 0;
        private bool mouseDown;
        private Vector3 startRectangle;
        private Vector3 endRectangle;
        
         public Control(Texture2D _texture, QuadTree quad)
        {
            this.texture = _texture;
            this.heights = quad.Vertices.heightData;
            double a = Math.Sqrt(heights.Length);
            this.width = (int)a;
            this.length = (int)a;

        }
        public void Update(GameTime gameTime)
        {


           
            currentMouseState = Mouse.GetState();
            Vector3 mouse3d2 = CalculateMouse3DPosition();

           
            if (currentMouseState.RightButton == ButtonState.Pressed && !mouseDown)
            {
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
                Selected(startRectangle, endRectangle);
                foreach (InteractiveModel ant in models)
                    if (ant.Model.Selected)
                    {

                        SelectedModels.Add(ant);
                    }
                    else
                    {
                        //f = 0;
                        SelectedModels.Clear();
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
                    ant.Model.playerTarget.X = mouse3d2.X;
                    ant.Model.playerTarget.Z = mouse3d2.Z;
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
            if(models!=null)
            foreach (var ant in models)
            {


                {

                    Vector3 oldPosition = ant.Model.Position;
                    if (FloatEquals(ant.Model.Position.X, ant.Model.playerTarget.X) && FloatEquals(ant.Model.Position.Z, ant.Model.playerTarget.Z))
                    //if (ant.Selected)
                    {

                        //ant.Selected = false;
                        return;
                    }

                    ////////////////////////////////////////////////////////////////////////////////////////////
                    //poruszanie się pod kątem 90 lub 45 stopni

                        if (ant.Model.Position.X == ant.Model.playerTarget.X && ant.Model.Position.Z == ant.Model.playerTarget.Z )//|| ant.Model.Collide==true)

                        {
                            //ant.Selected = false;
                            return;
                        }

                        float Speed = (float)2.0;
                        if (ant.Model.Position.X > ant.Model.playerTarget.X)
                        {
                            //ant.Model.Position += new Vector3(-0.01f,0,0) * Speed;
                            ant.Model.Position += Vector3.Left * Speed;
                        }
                        if (ant.Model.Position.X < ant.Model.playerTarget.X)
                        {
                            ant.Model.Position += Vector3.Right * Speed;
                            //ant.Model.Position += new Vector3(0.01f, 0, 0) * Speed;
                        }

                        if (ant.Model.Position.Z > ant.Model.playerTarget.Z)
                        {
                           // ant.Model.Position += new Vector3(0, 0, -0.01f) * Speed;
                            ant.Model.Position += Vector3.Forward * Speed;

                        }
                        if (ant.Model.Position.Z < ant.Model.playerTarget.Z)
                        {
                           // ant.Model.Position += new Vector3(0, 0, 0.01f) * Speed;
                            ant.Model.Position += Vector3.Backward * Speed;

                        }

                        //wysokosc
                        float height = GetHeightAt(ant.Model.Position.X, ant.Model.Position.Z);
                        if (ant.Model.Position.Y < height)
                        {
                            ant.Model.Position += Vector3.Up;
                        }
                        if (ant.Model.Position.Y > height)
                        {
                            ant.Model.Position += Vector3.Down;
                        }


                   // Vector3 direction = ant.Model.Position - oldPosition;
                    //ant.Model.Rotation = new Vector3(Vector3ToRadian(direction)+3);
                    //ant.Model.Rotation=new Vector3((float)Rotation(ant, oldPosition));
                    


                    /*
                    Vector3 direction = ant.Model.playerTarget - ant.Model.Position;
                    float rotation = (float)Math.Atan2(direction.Y, direction.X) + ((1f * (float)Math.PI) / 2);
                    //rotation += (float)(Math.PI * 0.5f);

                    ant.Model.Rotation = new Vector3(rotation);
                    /////////////////////////////////////////////////////////////////////////////////////////////

                    /////////////////////////////////////////////////////////////////////////////////////////////
                    /*
                    //poruszanie się płynne 
                    float height = GetHeightAt(ant.Model.Position.X, ant.Model.Position.Z);
                    ant.Model.playerTarget.Y = height;
                    Vector3 distance = ant.Model.playerTarget - ant.Model.Position;
                    distance.Normalize();
                    ant.Model.Position += distance * Speed;
                    if (ant.Model.Position.Y < height)
                    {
                        ant.Model.Position += Vector3.Up;
                    }
                    */
                    }
                }
            }

        public static bool FloatEquals(float f1, float f2)
        {
            return Math.Abs(f1 - f2) < 1;
        }

        
           



        /*
    private Vector3 Rotation(InteractiveModel ant, Vector3 oldPosition){

            if (ant.Model.Position.X > ant.Model.playerTarget.X && ant.Model.Position.Z > ant.Model.playerTarget.Z)
            {
                return angle = new Vector3(0, 7, 0);//góra-lewo
            } //ant.Model.playerTarget.X/100
            else if (ant.Model.Position.X > ant.Model.playerTarget.X && ant.Model.Position.Z < ant.Model.playerTarget.Z)
            {
                return angle = new Vector3(0, (float)-1.2, 0);/////góra-prawo
            }
            else if (ant.Model.Position.X == ant.Model.playerTarget.X && ant.Model.Position.Z < ant.Model.playerTarget.Z)
            {
                return angle = new Vector3(0, (float)2.5, 0);//prawo
            }
            else if (ant.Model.Position.X == ant.Model.playerTarget.X && ant.Model.Position.Z > ant.Model.playerTarget.Z)
            {
                return angle = new Vector3(0, 4, 0);///////lewo
            }
            else if (ant.Model.Position.X < ant.Model.playerTarget.X && ant.Model.Position.Z < ant.Model.playerTarget.Z)
            {
                return angle = new Vector3(0, 5, 0);//////////dół-prawo
            }
            else if (ant.Model.Position.X < ant.Model.playerTarget.X && ant.Model.Position.Z > ant.Model.playerTarget.Z)
            {
                return angle = new Vector3(0, 2, 0);//dół-lewo
            }
            else if (ant.Model.Position.X < ant.Model.playerTarget.X && ant.Model.Position.Z == ant.Model.playerTarget.Z)
            {
                return angle = new Vector3(0, 7, 0);/////////dół
            }
            else if (ant.Model.Position.X > ant.Model.playerTarget.X && ant.Model.Position.Z == ant.Model.playerTarget.Z)
            {
                return angle = new Vector3(0, (float)-0.7, 0);//góra
            }
            else return angle;      
            
        }
        */
        private float Rotation(InteractiveModel ant, Vector3 oldPosition)
        {
            double angle = Math.Atan2(ant.Model.Position.Z - oldPosition.Z, ant.Model.Position.X - oldPosition.X);
            //double angle = Math.Atan2(ant.Model.Position.Y - ant.Model.playerTarget.Y, ant.Model.Position.X - ant.Model.playerTarget.X);
            
            return (float)angle;

            //Vector3 first = 
            //Vector3 second = oldPosition.Normalize();
            //return 1.0f;
        }

        private float Vector3ToRadian(Vector3 direction)
        {
            return (float)Math.Atan2(direction.X, -direction.Z);
        }

        public float GetHeightAt(float worldX, float worldZ)
        {
            int x, z; // Cell coordinates in the height array 
            float fractionX = 0.0f, fractionZ = 0.0f; // Fractional coordinates within the quad 

            // If the position is off the height field to the left or right side, 
            // we interpolate along the respective border of the height field. 
            if (worldX <= 0.0f)
            {
                x = 0;
                fractionX = 0.0f;
            }
            else    if (worldX >= (float)(this.width - 1))
            {
                x = this.width - 2;
                fractionX = 1.0f;
            }
            else
            {
                x = (int)worldX;
                fractionX = worldX - x;
            }

            // If the position is off the height field to the top or bottom side, 
            // we interpolate along the respective border of the height field. 
            if (worldZ <= 0.0f)
            {
                z = 0;
                fractionZ = 0.0f;
            }
            else if (worldZ >= (float)(this.length - 1))
            {
                z = this.length - 2;
                fractionZ = 1.0f;
            }
            else
            {
                z = (int)worldZ;
                fractionZ = worldZ - z;
            }

            if ((fractionX + fractionZ) < 1.0f)
            { // We're in the upper left triangle 

                return
                  MathHelper.Lerp(this.heights[x, z], this.heights[x + 1, z], fractionX) +
                  (this.heights[x, z + 1] - this.heights[x, z]) * fractionZ;

            }
            else
            { // We're in the lower right triangle 

                return
                  MathHelper.Lerp(this.heights[x, z + 1], this.heights[x + 1, z + 1], fractionX) +
                  (this.heights[x + 1, z] - this.heights[x + 1, z + 1]) * (1.0f - fractionZ);

            }
        }

        private void Selected(Vector3 startRectangle, Vector3 endRectangle)
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
                if ((minRectangle.X < ant.Model.Position.X && maxRectangle.X > ant.Model.Position.X) &&
                (minRectangle.Z < ant.Model.Position.Z && maxRectangle.Z > ant.Model.Position.Z))
                {
                    ant.Model.Selected = true;
                }
                else
                {
                    ant.Model.Selected = false;
                }
            }
        }


        public InteractiveModel SelectedObject(Vector3 vector, Vector3 position)
        {

            ray.Position = position;
            Vector3 a = new Vector3(0, 0, 0);
            foreach (InteractiveModel IM in IModel)
            {
                if (IM.Model.BoundingSphere.Intersects(ray) != null)
                {
                    return IM;
                }
            }
            //to nie działa - nie wiem jeszcze czemu
            foreach (InteractiveModel ant in models)
            {
                if (ant.Model.BoundingSphere.Intersects(ray) != null)
                {
                    return ant;
                }
            }
            //////////////////////////////////
            return null;
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

