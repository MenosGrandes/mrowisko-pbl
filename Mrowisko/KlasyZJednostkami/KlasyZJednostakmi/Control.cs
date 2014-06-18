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
using Logic.EnviroModel;
using Logic.Meterials;
using Logic.Units.Ants;


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
        public int heightsa;
        public int width;
        public int length;
        public VertexMultitextured[] vertices;
        private List<float> history=new List<float>();

        public int indeks;

        //private Texture2D texture;
        Vector2 position;
        public Vector3 position3d, position3DMouseOnlyMove;
        MouseState currentMouseState;
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
            modelos = new InteractiveModel(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/shoot"), Vector3.Zero, new Vector3(0.3f), Vector3.One, StaticHelpers.StaticHelper.Device, null)); 

        }
        public void Update(GameTime gameTime)
        {




            Avoid(gameTime);
           
            currentMouseState = Mouse.GetState();
            mouseRay = GetMouseRay(new Vector2(currentMouseState.X, currentMouseState.Y));

            
            Vector3 mouse3d2 = QuadNodeController.getIntersectedQuadNode(mouseRay);
            modelos.Model.Position = new Vector3(mouse3d2.X, StaticHelpers.StaticHelper.GetHeightAt(mouse3d2.X,mouse3d2.Z), mouse3d2.Z);
            Console.WriteLine(selectedObject);
            selectedObjectMouseOnlyMove = null;
            for (int i = 0; i < models.Count; i++)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                if (models[i].CheckRayIntersection(mouseRay))
                {
                    //Console.WriteLine(models[i].GetType() + " adad");
                   
                        selectedObject = models[i];
                        //Console.WriteLine(selectedObject);

                }
               
                    
                }
               
               else
                        selectedObjectMouseOnlyMove = models[i];

            }
            for (int i = 0; i < Models_Colision.Count; i++)
            {
                Models_Colision[i].Model.B_Box = Models_Colision[i].Model.updateBoundingBox();
              

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

                starSelectMouseRay = GetMouseRay(new Vector2(currentMouseState.X, currentMouseState.Y));
                selectRectangleStart = QuadNodeController.getIntersectedQuadNode(starSelectMouseRay);
                position = new Vector2(currentMouseState.X, currentMouseState.Y);
                selectCorner = position;
                selectRectangle = new Rectangle((int)position.X, (int)position.Y, 0, 0);
                mouseDown = true;

            }
            else if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                stopMouseRay=GetMouseRay(new Vector2(currentMouseState.X,currentMouseState.Y));
                selectCorner = new Vector2(currentMouseState.X, currentMouseState.Y);
                selectRectangleStop = QuadNodeController.getIntersectedQuadNode(stopMouseRay);
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


            //updateAnt(gameTime);
           
                           
        }

        public void Draw(SpriteBatch spriteBatch,FreeCamera camera)
        {
            modelos.Draw(camera);
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
            float Speed = (float)20f * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

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
                {  if(modelsy.GetType().BaseType==typeof(EnviroModels))
                {
                    continue;
                }
                    Vector3 center = modelsy.Model.B_Box.Min + (modelsy.Model.B_Box.Max - modelsy.Model.B_Box.Min) / 2;
                    //float bBoxHeight = modelsy.Model.B_Box.Max.Y - modelsy.Model.B_Box.Min.Y;
                    //float bBoxxWidth = modelsy.Model.B_Box.Max.X - modelsy.Model.B_Box.Min.X;
                    //float odlegloscMiedzyWektorami=Vector3.Distance(modelsy.Model.B_Box.Max, modelsy.Model.B_Box.Min);
                    //float przekatna = (float)Math.Sqrt(Math.Pow(odlegloscMiedzyWektorami, 2) - Math.Pow(bBoxHeight, 2));
                    if ((modelsy.Model.B_Box.Contains(ant.Model.playerTarget) == ContainmentType.Contains))
                    {

                        //if (ant.Model.Position.X > ant.Model.playerTarget.X && ant.Model.Position.Z < ant.Model.playerTarget.Z)
                        {
//ant.Model.playerTarget.X = modelsy.Model.B_Box.Max.X + modelsy.Model.B_Box.Max.X - modelsy.Model.B_Box.Min.X;//modelsy.Model.BoundingSphere.Center.X + modelsy.Model.BoundingSphere.Radius;
  //                          ant.Model.playerTarget.Z = modelsy.Model.B_Box.Max.Y + modelsy.Model.B_Box.Max.Y - modelsy.Model.B_Box.Min.Y;//modelsy.Model.BoundingSphere.Center.Z + modelsy.Model.BoundingSphere.Radius;
                        }
                                            
                    }

                }


                Vector3 lewo, prawo, gora, dol;
                Vector3 lewy_gorny, prawy_gorny, lewy_dolny, prawy_dolny;
                bool czylewo = false, czyprawo = false, czygora = false, czydol = false, czylewy_gorny = false, czyprawy_gorny = false, czylewy_dolny = false, czyprawy_dolny = false;
                lewo = ant.Model.Position + (Vector3.Left  * Speed);
                prawo = ant.Model.Position + (Vector3.Right  * Speed  );
                gora = ant.Model.Position + (Vector3.Forward  * Speed );
                dol = ant.Model.Position + (Vector3.Backward  * Speed );
                lewy_gorny = ant.Model.Position + (new Vector3(-1, 0, -1)  * Speed  );
                lewy_dolny = ant.Model.Position + (new Vector3(-1, 0, 1)  * Speed  );
                prawy_dolny = ant.Model.Position + (new Vector3(1, 0, 1)  * Speed  );
                prawy_gorny = ant.Model.Position + (new Vector3(1, 0, -1)  * Speed  );

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
                   
                    else
                    {
                        ant.Model.Position += Vector3.Left * Speed;
                        poprzedni = Vector3.Left;
                        if (ant.GetType().Name == "Queen")
                            ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-90), 0);
                        else
                        {
                            ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(90), 0);
                        }
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
                    if(ant.GetType().Name=="Queen")
                    ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(90), 0);
                    else
                    {
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-90), 0);
                    }
                }
                if (indeks == 2)
                {
                    if (poprzedni == Vector3.Backward)
                        // ant.Model.Position += new Vector3(1, 0, -1) * 4.0f;
                        ant.Model.playerTarget.Z = ant.Model.playerTarget.Z + Speed;
                    else
                        ant.Model.Position += Vector3.Forward * Speed;
                    poprzedni = Vector3.Forward;
                    if (ant.GetType().Name == "Queen")
                    ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(180), 0);
                    else
                    {
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(0), 0);
                    }
                }
                if (indeks == 3)
                {
                    if (poprzedni == Vector3.Forward)
                        ant.Model.playerTarget.Z = ant.Model.playerTarget.Z + Speed;
                    else
                        ant.Model.Position += Vector3.Backward * Speed;
                    poprzedni = Vector3.Backward;
                    if (ant.GetType().Name == "Queen")
                    ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(0), 0);
                    else
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(180), 0);
                }

                if (indeks == 4)
                {
                    if (poprzedni == prawy_dolny)
                        ant.Model.playerTarget = ant.Model.playerTarget + (new Vector3(-1, 0, -1) * Speed);
                    else
                        ant.Model.Position += (new Vector3(-1, 0, -1) * Speed);
                    if (ant.GetType().Name == "Queen") 
                    ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-45 - 90), 0);
                    else
                        //ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(45 + 90), 0);
                    ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(45 ), 0);

                    poprzedni = lewy_gorny;
                }

                if (indeks == 5)
                {
                    if (poprzedni == prawy_gorny)
                        ant.Model.Position += Vector3.Backward * Speed;
                    else
                        ant.Model.Position += (new Vector3(-1, 0, 1) * Speed);
                    if (ant.GetType().Name == "Queen")  
                    ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-45 ), 0);
                    else
                    {
                       // ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(45), 0);
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(45 + 90), 0);
                    }
                    poprzedni = lewy_dolny;

                }

                if (indeks == 6)
                {
                    if (poprzedni == lewy_gorny)
                        ant.Model.playerTarget = ant.Model.playerTarget + (new Vector3(-1, 0, -1) * Speed);
                    else
                        ant.Model.Position += (new Vector3(1, 0, 1) * Speed);
                    if (ant.GetType().Name == "Queen")
                    ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(45 ), 0);
                    else
                    {
                        //ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-45 ), 0); 
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-45 - 90), 0);

                    }
                    poprzedni = prawy_dolny;

                }
                if (indeks == 7)
                {
                    if (poprzedni == lewy_dolny)
                        ant.Model.playerTarget = ant.Model.playerTarget + (new Vector3(-1, 0, -1) * Speed);
                    else
                        ant.Model.Position += (new Vector3(1, 0, -1) * Speed);
                    if (ant.GetType().Name == "Queen")
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(45 + 90), 0);
                    else
                    {//ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-45 - 90), 0);
                        ant.Model.Rotation = new Vector3(0, MathHelper.ToRadians(-45), 0);
                    }
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
                    switch(ant.GetType().Name)
                    {
                        case "Beetle": height += 11; break;
                        case "AntPeasant": height +=7; break;
                        case "Spider": height += 7; break;
                    }
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



public void Avoid(GameTime gameTime)
{
  for(int j=0;j<models.Count;j++)
  {

      if (models[j].ImMoving == false)
      {
          continue;
      }
      if (Vector2.Distance(new Vector2(models[j].Model.Position.X, models[j].Model.Position.Z), new Vector2(models[j].Model.playerTarget.X, models[j].Model.playerTarget.Z)) < 1)
      {
          models[j].ImMoving = false;
          continue;
      };
    float pullDistance = Vector3.Distance(models[j].Model.playerTarget, models[j].Model.Position);
 
    //Only do something if we are not already there
    if (pullDistance > 1)
    {
        Vector3 pull = (models[j].Model.playerTarget - models[j].Model.Position) * (1 / pullDistance); //the target tries to 'pull us in'
        Vector3 totalPush = Vector3.Zero;
 
        int contenders = 0;
        for (int i = 0; i < Models_Colision.Count; ++i)		
        {  
        //if(ship.GetType()==typeof(AntPeasant))
        //{
        //    if (((AntPeasant)ship).gaterMaterialObject == obstacles[i])
        //           {
        //               continue;
        //           }
        //}

            //draw a vector from the obstacle to the ship, that 'pushes the ship away'
            Vector3 push = models[j].Model.Position - Models_Colision[i].Model.Position;
 
            //calculate how much we are pushed away from this obstacle, the closer, the more push
            float distance = (Vector3.Distance(models[j].Model.Position, Models_Colision[i].Model.Position) - Models_Colision[i].Model.BoundingSphere.Radius) - models[j].Model.BoundingSphere.Radius;
            //only use push force if this object is close enough such that an effect is needed
            if (distance < models[j].Model.BoundingSphere.Radius * 2)
            {
                ++contenders; //note that this object is actively pushing
 
                if (distance < 0.0001f) //prevent division by zero errors and extreme pushes
                {
                    distance = 0.0001f;
                }
                float weight = 1 / distance;
 
                totalPush += push * weight;
            }
        }
 
        pull *= Math.Max(1, 4 * contenders); //4 * contenders gives the pull enough force to pull stuff trough (tweak this setting for your game!)
        pull += totalPush;
 
        //Normalize the vector so that we get a vector that points in a certain direction, which we van multiply by our desired speed
        pull.Normalize();
        //Set the ships new position;
        float height = 0;
        switch (models[j].GetType().Name)
        {
            case "Beetle": height += 11; break;
            case "AntPeasant": height += 7; break;
            case "Spider": height += 7; break;
        }
        models[j].Model.Position = new Vector3(models[j].Model.Position.X + (pull.X * 60) * (float)gameTime.ElapsedGameTime.TotalSeconds, StaticHelpers.StaticHelper.GetHeightAt(models[j].Model.Position.X, models[j].Model.Position.Z) + height, models[j].Model.Position.Z + (pull.Z * 60) * (float)gameTime.ElapsedGameTime.TotalSeconds);
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

            startRectangle = selectRectangleStart;
            endRectangle = selectRectangleStop;

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
        {
            foreach (InteractiveModel IM in IModel)
            {
                if (IM.CheckRayIntersection(mouseRay))
                {
                    return IM;
                }
            }
            foreach (InteractiveModel ant in models)
            {

                if (ant.CheckRayIntersection(mouseRay))
                {

                    SelectedModels.Add(ant);
                    return ant;
                }
            }
            return null;
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
                                                                                        
      

        public Ray starSelectMouseRay { get; set; }

        public Vector3 selectRectangleStart { get; set; }

        public Ray stopMouseRay { get; set; }

        public Vector3 selectRectangleStop { get; set; }
    }
}

