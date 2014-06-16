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
using Logic.Building;
using Logic.Units.Allies;


namespace Logic
{
    public class Control
    {
        public InteractiveModel modelos;
        public float cameraYaw, cameraPitch;
        public Matrix View;
        public Matrix Projection;
        public GraphicsDevice device;
        public List<InteractiveModel> models;
        public List<float> angles = new List<float>();
        public Texture2D texture;
        public List<InteractiveModel> SelectedModels = new List<InteractiveModel>();
        public List<InteractiveModel> IModel = new List<InteractiveModel>();
        public List<InteractiveModel> Models_Colision = new List<InteractiveModel>();
        public InteractiveModel selectedObject, selectedObjectMouseOnlyMove;
        private Vector3 poprzedni = Vector3.Zero;
        public Ray ray, mouseRay;
        public Vector3 angle = new Vector3(0, 0, 0);
        //private Vector3 playerTarget;
        private Vector2 selectCorner;
        private Rectangle selectRectangle;
        public LoadModel moving;

        //tabele wysokosci
        private float[,] heights;
        public int heightsa;
        public int width;
        public int length;
        public VertexMultitextured[] vertices;


        public int indeks;

        //private Texture2D texture;
        Vector2 position, positionMouseOnlyMove;
        public Vector3 position3d, position3DMouseOnlyMove;
        MouseState currentMouseState;
        int f = 0;
        private bool mouseDown;
        private Vector3 startRectangle;
        private Vector3 endRectangle;
        public Vector3 mouse3d2;

        public Control(Texture2D _texture, QuadTree quad)
        {
            this.texture = _texture;
            StaticHelpers.StaticHelper.heights=quad.Vertices.heightDataToControl;
            StaticHelpers.StaticHelper.width = (int)Math.Sqrt(quad.Vertices.heightDataToControl.Length);
            StaticHelpers.StaticHelper.length = (int)Math.Sqrt(quad.Vertices.heightDataToControl.Length);
            modelos = new InteractiveModel(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/shoot"), Vector3.Zero, Vector3.One, Vector3.One, StaticHelpers.StaticHelper.Device, null)); 

        }
        public void Update(GameTime gameTime)
        {

            //if(selectedObject!=null)
            // Console.WriteLine(selectedObject);






            currentMouseState = Mouse.GetState();
            mouseRay = GetMouseRay(new Vector2(currentMouseState.X, currentMouseState.Y));

            // Vector3 mouse3d2 = CalculateMouse3DPosition(currentMouseState.X,currentMouseState.Y);
            
            Vector3 mouse3d2 = QuadNodeController.getIntersectedQuadNode(mouseRay);
            modelos.Model.Position = new Vector3(mouse3d2.X, 30, mouse3d2.Z);

            selectedObjectMouseOnlyMove = null;
            for (int i = 0; i < models.Count; i++)
                if (models[i].CheckRayIntersection(mouseRay))
                {
                    //Console.WriteLine(models[i].GetType() + " adad");
                    if (currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        selectedObject = models[i];
                        //Console.WriteLine(selectedObject);

                    }
                    else
                        selectedObjectMouseOnlyMove = models[i];
                    break;
                }

            for (int i = 0; i < Models_Colision.Count; i++)
                if (Models_Colision[i].CheckRayIntersection(mouseRay))
                {
                    if (currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (Models_Colision[i].GetType() == typeof(BuildingPlace))
                        {
                            if (((BuildingPlace)Models_Colision[i]).House != null)
                            {
                                selectedObject = ((BuildingPlace)Models_Colision[i]).House;
                            }
                            else
                            {
                                selectedObject = Models_Colision[i];

                            }

                        }
                        else
                        {
                            selectedObject = Models_Colision[i];

                        }


                    }
                    else

                        if (Models_Colision[i].GetType() == typeof(BuildingPlace))
                        {
                            if (((BuildingPlace)Models_Colision[i]).House != null)
                            {
                                selectedObjectMouseOnlyMove = ((BuildingPlace)Models_Colision[i]).House;
                            }
                            else
                            {
                                selectedObjectMouseOnlyMove = Models_Colision[i];

                            }
                        }
                        else
                        {
                            selectedObjectMouseOnlyMove = Models_Colision[i];

                        }

                    break;
                }
            #region kolizja między jednostkami
            /*
            for (int i = 0; i < models.Count; i++)
            {
                for (int j = 0; j < models.Count; j++)
                {
                    if (models[i] == models[j]) continue;



                    if (models[j].Model.BoundingSphere.Intersects(models[i].Model.BoundingSphere))
                    {
                        models[i].Model.Position = models[i].Model.tempPosition;
                    }
                }
            }
                */

            #endregion
            if (currentMouseState.RightButton == ButtonState.Pressed && !mouseDown)
            {
                SelectedModels.Clear();
                SelectedObject();


                position = new Vector2(currentMouseState.X, currentMouseState.Y);
                position3d = CalculateMouse3DPosition(position);
                selectCorner = position;
                selectRectangle = new Rectangle((int)position.X, (int)position.Y, 0, 0);
                //selectedObject = SelectedObject(mouse3d2);
                //if ((mouse3d2.X > pozycja_X_lewo && mouse3d2.X < pozycja_X_prawo) && (mouse3d2.Z > pozycja_Z_dol && mouse3d2.Z < pozycja_Z_gora))
                mouseDown = true;

            }
            else if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                //SelectedModels.Clear();

                selectCorner = new Vector2(currentMouseState.X, currentMouseState.Y);
                if (selectCorner.X > position.X)
                {
                    selectRectangle.X = (int)position.X;
                }
                else
                {
                    selectRectangle.X = (int)selectCorner.X;
                }

                if (selectCorner.Y > position.Y)
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
                if (mouseDown == true)
                {
                    Selected();
                    foreach (InteractiveModel ant in models)
                        if (ant.Model.Selected)
                        {
                            
                                SelectedModels.Add(ant);
                            
                        }
                    /*
                else
                {
                    //f = 0;
                    //SelectedModels.Clear();
                }
                      */
                }
                mouseDown = false;
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                foreach (InteractiveModel ant in SelectedModels)
                {
                    ant.Model.playerTarget.X = mouse3d2.X;
                    ant.Model.playerTarget.Z = mouse3d2.Z;
                    ant.Model.switchAnimation("Walk");
                    ant.ImMoving = true;
                }
            }

            updateAnt(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch,FreeCamera camera)
        {
            //modelos.Draw(camera);
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
            float Speed = (float)2.5f * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 100;

            foreach (InteractiveModel ant in models)
            {
                ant.Model.tempPosition = ant.Model.Position;


                if (ant.ImMoving == false)
                {
                    continue;
                }
                if (Math.Abs(ant.Model.Position.X - ant.Model.playerTarget.X) <= Speed && Math.Abs(ant.Model.Position.Z - ant.Model.playerTarget.Z) <= Speed)
                {

                    ant.Model.switchAnimation("Idle");
                    ant.ImMoving = false;
                    continue;
                }

                if (ant.snared == true)
                {
                    continue;
                }


                foreach (InteractiveModel modelsy in Models_Colision)
                {
                    modelsy.Model.B_Box = modelsy.Model.updateBoundingBox();
                    if ((modelsy.Model.B_Box.Contains(ant.Model.playerTarget) == ContainmentType.Contains))
                    {
                        // ant.Model.playerTarget.X = modelsy.Model.B_Box.Max.X + modelsy.Model.B_Box.Max.X - modelsy.Model.B_Box.Min.X;//modelsy.Model.BoundingSphere.Center.X + modelsy.Model.BoundingSphere.Radius;
                        // ant.Model.playerTarget.Z = modelsy.Model.B_Box.Max.Y+modelsy.Model.B_Box.Max.Y-modelsy.Model.B_Box.Min.Y;//modelsy.Model.BoundingSphere.Center.Z + modelsy.Model.BoundingSphere.Radius;
                        break;
                    }

                }


                Vector3 lewo, prawo, gora, dol;
                Vector3 lewy_gorny, prawy_gorny, lewy_dolny, prawy_dolny;
                bool czylewo = false, czyprawo = false, czygora = false, czydol = false, czylewy_gorny = false, czyprawy_gorny = false, czylewy_dolny = false, czyprawy_dolny = false;
                lewo = ant.Model.Position + (Vector3.Left * Speed);
                prawo = ant.Model.Position + (Vector3.Right * Speed);
                gora = ant.Model.Position + (Vector3.Forward * Speed);
                dol = ant.Model.Position + (Vector3.Backward * Speed);
                lewy_gorny = ant.Model.Position + (new Vector3(-1, 0, -1) * Speed);
                lewy_dolny = ant.Model.Position + (new Vector3(-1, 0, 1) * Speed);
                prawy_dolny = ant.Model.Position + (new Vector3(1, 0, 1) * Speed);
                prawy_gorny = ant.Model.Position + (new Vector3(1, 0, -1) * Speed);

                float min;
                float[] tab = new float[8];

                foreach (InteractiveModel model in Models_Colision)
                {

                    if ((model.Model.B_Box.Contains(lewo) == ContainmentType.Contains))
                        czylewo = true;
                    if ((model.Model.B_Box.Contains(prawo) == ContainmentType.Contains))
                        czyprawo = true;
                    if ((model.Model.B_Box.Contains(gora) == ContainmentType.Contains))
                        czygora = true;
                    if ((model.Model.B_Box.Contains(dol) == ContainmentType.Contains))
                        czydol = true;
                    if ((model.Model.B_Box.Contains(lewy_gorny) == ContainmentType.Contains))
                        czylewy_gorny = true;
                    if ((model.Model.B_Box.Contains(lewy_dolny) == ContainmentType.Contains))
                        czylewy_dolny = true;
                    if ((model.Model.B_Box.Contains(prawy_dolny) == ContainmentType.Contains))
                        czyprawy_dolny = true;
                    if ((model.Model.B_Box.Contains(prawy_gorny) == ContainmentType.Contains))
                        czyprawy_gorny = true;
                }





                if (czylewo == false)
                    tab[0] = (float)Math.Sqrt((float)Math.Pow(lewo.X - ant.Model.playerTarget.X, 2.0f) + (float)Math.Pow(lewo.Z - ant.Model.playerTarget.Z, 2.0f));

                if (czyprawo == false)
                    tab[1] = (float)Math.Sqrt((float)Math.Pow(prawo.X - ant.Model.playerTarget.X, 2.0f) + (float)Math.Pow(prawo.Z - ant.Model.playerTarget.Z, 2.0f));

                if (czygora == false)
                    tab[2] = (float)Math.Sqrt((float)Math.Pow(gora.X - ant.Model.playerTarget.X, 2.0f) + (float)Math.Pow(gora.Z - ant.Model.playerTarget.Z, 2.0f));

                if (czydol == false)
                    tab[3] = (float)Math.Sqrt((float)Math.Pow(dol.X - ant.Model.playerTarget.X, 2.0f) + (float)Math.Pow(dol.Z - ant.Model.playerTarget.Z, 2.0f));

                if (czylewy_gorny == false)
                    tab[4] = (float)Math.Sqrt((float)Math.Pow(lewy_gorny.X - ant.Model.playerTarget.X, 2.0f) + (float)Math.Pow(lewy_gorny.Z - ant.Model.playerTarget.Z, 2.0f));

                if (czylewy_dolny == false)
                    tab[5] = (float)Math.Sqrt((float)Math.Pow(lewy_dolny.X - ant.Model.playerTarget.X, 2.0f) + (float)Math.Pow(lewy_dolny.Z - ant.Model.playerTarget.Z, 2.0f));

                if (czyprawy_dolny == false)
                    tab[6] = (float)Math.Sqrt((float)Math.Pow(prawy_dolny.X - ant.Model.playerTarget.X, 2.0f) + (float)Math.Pow(prawy_dolny.Z - ant.Model.playerTarget.Z, 2.0f));

                if (czyprawy_gorny == false)
                    tab[7] = (float)Math.Sqrt((float)Math.Pow(prawy_gorny.X - ant.Model.playerTarget.X, 2.0f) + (float)Math.Pow(prawy_gorny.Z - ant.Model.playerTarget.Z, 2.0f));

                min = 10000.0f;
                indeks = 0;
                for (int i = 0; i < tab.Length; i++)
                {
                    if (tab[i] < min && tab[i] != 0)
                    {
                        min = tab[i];
                        indeks = i;
                    }
                }
                
                if (indeks == 0)
                {
                    if (poprzedni == Vector3.Right)
                        // ant.Model.Position += new Vector3(-1, 0, -1) * 4.0f;
                        ant.Model.playerTarget.X = ant.Model.playerTarget.X + Speed;
                    else if (poprzedni == Vector3.Up)
                    {
                        ant.Model.Position += Vector3.Left * Speed;
                        poprzedni = Vector3.Left;
                            ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(90), 0);
                    }
                    else if (poprzedni == Vector3.Forward)
                    {
                        ant.Model.Position += Vector3.Left * Speed;
                        poprzedni = Vector3.Left;
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-90), 0);
                    }
                    else if (poprzedni == Vector3.Backward)
                    {
                        ant.Model.Position += Vector3.Left * Speed;
                        poprzedni = Vector3.Left;
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-90), 0);
                    }
                    else if (poprzedni == lewy_dolny)
                    {
                        ant.Model.Position += Vector3.Left * Speed;
                        poprzedni = Vector3.Left;
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-45), 0);
                    }
                    else if (poprzedni == lewy_gorny)
                    {
                        ant.Model.Position += Vector3.Left * Speed;
                        poprzedni = Vector3.Left;
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(45), 0);
                    }
                    else if (poprzedni == prawy_dolny)
                    {
                        ant.Model.Position += Vector3.Left * Speed;
                        poprzedni = Vector3.Left;
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-45-90), 0);
                    }
                    else if (poprzedni == prawy_gorny)
                    {
                        ant.Model.Position += Vector3.Left * Speed;
                        poprzedni = Vector3.Left;
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(45+90), 0);
                    }
                    else
                    {
                        ant.Model.Position += Vector3.Left * Speed;
                        poprzedni = Vector3.Left;
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(45 + 90), 0);

                    }
                }
                if (indeks == 1)
                {
                    if (poprzedni == Vector3.Left)
                        // ant.Model.Position += new Vector3(-1, 0, 1) * 4.0f;
                        ant.Model.playerTarget.X = ant.Model.playerTarget.X + Speed;
                    else
                        ant.Model.Position += Vector3.Right * Speed;
                    poprzedni = Vector3.Right;
                   // ant.Model.Rotation += new Vector3(0, MathHelper.ToRadians(1), 0);
                }
                if (indeks == 2)
                {
                    if (poprzedni == Vector3.Backward)
                        // ant.Model.Position += new Vector3(1, 0, -1) * 4.0f;
                        ant.Model.playerTarget.Z = ant.Model.playerTarget.Z + Speed;
                    else
                        ant.Model.Position += Vector3.Forward * Speed;
                    poprzedni = Vector3.Forward;
                   // ant.Model.Rotation += new Vector3(0, MathHelper.ToRadians(1), 0);
                }
                if (indeks == 3)
                {
                    if (poprzedni == Vector3.Forward)
                        ant.Model.playerTarget.Z = ant.Model.playerTarget.Z + Speed;
                    else
                        ant.Model.Position += Vector3.Backward * Speed;
                    poprzedni = Vector3.Backward;
                   // ant.Model.Rotation += new Vector3(0, MathHelper.ToRadians(-1), 0);
                }

                if (indeks == 4)
                {
                    if (poprzedni == prawy_dolny)
                        ant.Model.playerTarget = ant.Model.playerTarget + (new Vector3(-1, 0, -1) * Speed);
                    else
                        ant.Model.Position += (new Vector3(-1, 0, -1) * Speed);
                  //  ant.Model.Rotation += new Vector3(0, MathHelper.ToRadians(-1), 0);
                    poprzedni = lewy_gorny;
                }

                if (indeks == 5)
                {
                    if (poprzedni == prawy_gorny)
                        ant.Model.Position += Vector3.Backward * Speed;
                    else
                        ant.Model.Position += (new Vector3(-1, 0, 1) * Speed);
                   // ant.Model.Rotation += new Vector3(0, MathHelper.ToRadians(-1), 0);
                    poprzedni = lewy_dolny;

                }

                if (indeks == 6)
                {
                    if (poprzedni == lewy_gorny)
                        ant.Model.playerTarget = ant.Model.playerTarget + (new Vector3(-1, 0, -1) * Speed);
                    else
                        ant.Model.Position += (new Vector3(1, 0, 1) * Speed);
                   // ant.Model.Rotation += new Vector3(0, MathHelper.ToRadians(-1), 0);
                    poprzedni = prawy_dolny;

                }
                if (indeks == 7)
                {
                    if (poprzedni == lewy_dolny)
                        ant.Model.playerTarget = ant.Model.playerTarget + (new Vector3(-1, 0, -1) * Speed);
                    else
                        ant.Model.Position += (new Vector3(1, 0, -1) * Speed);
                   // ant.Model.Rotation += new Vector3(0, MathHelper.ToRadians(-1), 0);
                    poprzedni = prawy_gorny;
                }





                {

                    // ant.Model.tempPosition = ant.Model.Position;
                    /*                    if (FloatEquals(ant.Model.Position.X, ant.Model.playerTarget.X) && FloatEquals(ant.Model.Position.Z, ant.Model.playerTarget.Z))
                                        //if (ant.Selected)
                                        {

                                            //ant.Selected = false;
                                            return;
                                        }
                                        */
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    //poruszanie się pod kątem 90 lub 45 stopni

                    //  if (FloatEquals(ant.Model.Position.X, ant.Model.playerTarget.X) && FloatEquals(ant.Model.Position.Z, ant.Model.playerTarget.Z ))//|| ant.Model.Collide==true)

                    //   {
                    //ant.Selected = false;
                    //         continue;
                    //     }

                    //wysokosc

                    float height = StaticHelpers.StaticHelper.GetHeightAt(ant.Model.Position.X, ant.Model.Position.Z);//GetHeightAt(ant.Model.Position.X, ant.Model.Position.Z);
                    //float height = vertices[((int)ant.Model.Position.X/3) + ((int)ant.Model.Position.Z/3) * (int)width].Position.Y;

                    if (ant.Model.Position.Y < height)
                    {
                        ant.Model.Position += Vector3.Up;
                    }
                    if (ant.Model.Position.Y > height)
                    {
                        ant.Model.Position += Vector3.Down;
                    }




                    //  Vector3 direction = ant.Model.tempPosition - ant.Model.Position;

                    //if (!ant.Model.playerTarget.Equals(ant.Model.Position))
                    //{

                    //   ant.Model.Rotation = new Vector3(0, Vector3ToRadian(direction), 0);
                    //ant.Model.Rotation=new Vector3(0,(float)Rotation(ant, oldPosition),0);
                    // }



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
            return Math.Abs(f1 - f2) < 2;
        }



        private float Vector3ToRadian(Vector3 direction)
        {
            return (float)Math.Atan2(direction.X, direction.Z);
        }





        private void Selected()
        {

            startRectangle = CalculateMouse3DPosition(position);
            endRectangle = CalculateMouse3DPosition(selectCorner);

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

        public InteractiveModel SelectedObjectOnMouseMove(Vector3 position)
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
            foreach (InteractiveModel ant in models)
            {
                //   Console.WriteLine(ant.Model.Position);

                if (ant.Model.BoundingSphere.Intersects(ray) != null)
                {

                    return ant;
                }
            }
            return null;
        }
        public InteractiveModel SelectedObject()
        {//Vector3 position

            //ray.Position = position;
            Vector3 a = new Vector3(0, 0, 0);
            foreach (InteractiveModel IM in IModel)
            {
                if (IM.CheckRayIntersection(mouseRay))
                {
                    return IM;
                }
            }
            foreach (InteractiveModel ant in models)
            {
                //   Console.WriteLine(ant.Model.Position);

                if (ant.CheckRayIntersection(mouseRay))
                {

                    SelectedModels.Add(ant);
                    return ant;
                }
            }
            return null;
        }


        private Vector3 CalculateMouse3DPosition()
        {
            Plane GroundPlane = new Plane(0, 30, 0, 30); // x - lewo prawo Z- gora dol
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            Vector3 nearSource = new Vector3((float)mouseX, (float)mouseY, 0.0f);
            Vector3 farSource = new Vector3((float)mouseX, (float)mouseY, 1.0f);

            // Matrix world = Matrix.CreateTranslation(0, 0, 0);

            Vector3 nearPoint = device.Viewport.Unproject(nearSource, this.Projection, this.View, Matrix.Identity);
            Vector3 farPoint = device.Viewport.Unproject(farSource, this.Projection, this.View, Matrix.Identity);

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

            Vector3 nearSource = new Vector3((float)mouseX, (float)mouseY, 0.0f);
            Vector3 farSource = new Vector3((float)mouseX, (float)mouseY, 1.0f);

            // Matrix world = Matrix.CreateTranslation(0, 0, 0);

            Vector3 nearPoint = device.Viewport.Unproject(nearSource, this.Projection, this.View, Matrix.Identity);
            Vector3 farPoint = device.Viewport.Unproject(farSource, this.Projection, this.View, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            //return new Ray(nearPoint, direction);

            Ray pickRay = new Ray(nearPoint, direction);
            float? position = pickRay.Intersects(GroundPlane);

            if (position != null)
                return pickRay.Position + pickRay.Direction * position.Value;
            else
                return new Vector3(0, 0, 0);


        }
        private Vector3 CalculateMouse3DPosition(int X, int Y)
        {
            int mouseX = X;
            int mouseY = Y;
            Vector3 nearScreenPoint = new Vector3(mouseX, mouseY, 0);
            Vector3 farScreenPoint = new Vector3(mouseX, mouseY, 1);
            Vector3 nearWorldPoint = device.Viewport.Unproject(nearScreenPoint, this.Projection, this.View, Matrix.Identity);
            Vector3 farWorldPoint = device.Viewport.Unproject(farScreenPoint, this.Projection, this.View, Matrix.Identity);

            Vector3 direction = farWorldPoint - nearWorldPoint;
            direction.Normalize();
            float zFactor = -nearWorldPoint.Y / direction.Y;
            Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;
            return zeroWorldPoint;


        }
        public Ray GetMouseRay(Vector2 mousePosition)
        {
            Vector3 nearSource = new Vector3(mousePosition, 0.0f);
            Vector3 farSource = new Vector3(mousePosition, 1.0f);

            Vector3 nearPoint = device.Viewport.Unproject(nearSource, this.Projection, this.View, Matrix.Identity);
            Vector3 farPoint = device.Viewport.Unproject(farSource, this.Projection, this.View, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        /*

        public Ray CalculateCursorRay(Matrix projectionMatrix, Matrix viewMatrix)
        {
            Vector3 nearSource = new Vector3(Position, 0f);
            Vector3 farSource = new Vector3(Position, 1f);

             Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(nearSource, projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farSource, projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }


       */
    }
}

