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


        private Node endNode;
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
        MouseState currentMouseState,lastMouseState;
        private bool mouseDown;
        private Vector3 startRectangle;
        private Vector3 endRectangle;
        public Vector3 mouse3d2;
        
        public Control(Texture2D _texture, QuadTree quad)
        {
            this.texture = _texture;
         modelos = new InteractiveModel(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/shoot"), Vector3.Zero, new Vector3(0.3f), new Vector3(0.2f), StaticHelpers.StaticHelper.Device, null));
           
        }
        public void Update(GameTime gameTime)
        {




          //  Avoid(gameTime);
           
            currentMouseState = Mouse.GetState();
            mouseRay = GetMouseRay(new Vector2(currentMouseState.X,currentMouseState.Y));
            
            Vector3 mouse3d2 = QuadNodeController.getIntersectedQuadNode(mouseRay);
                        Node n= PathFinderManagerNamespace.PathFinderManager.getNodeIntersected(mouseRay);
                        Vector3 mouseNodeSelectedVector=new Vector3(n.centerPosition.X,n.Height,n.centerPosition.Y);
           // int intersectedTileNumber = QuadNodeController.getIntersectedQuadNodeForMove(mouseRay);
           // modelos.Model.Position = mouse3d2;
                        modelos.Model.Position = mouseNodeSelectedVector;
            //modelos.Model.Position = new Vector3(mouse3d2.X, StaticHelpers.StaticHelper.GetHeightAt(mouse3d2.X,mouse3d2.Z), mouse3d2.Z);
            //Console.WriteLine(selectedObject);
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
#region modele z którymi mamy interakacje
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

            if (lastMouseState.LeftButton == ButtonState.Pressed &&currentMouseState.LeftButton== ButtonState.Released)
            {
                foreach (InteractiveModel ant in SelectedModels)
                {

                    endNode = PathFinderManagerNamespace.PathFinderManager.getNodeIntersected(mouseRay);
                    if (((Unit)ant).PathFinder.Search(ant.MyNode, endNode) == true)
                    {

                        ((Unit)ant).MovementPath=new Queue<Node>(((Unit)ant).PathFinder.finalPath);
                        ((Unit)ant).Moving = true;
                        ((Unit)ant).PathFinder.finalPath.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Nie ma mnie jeszcze");
                    }
                }
            }


            lastMouseState = currentMouseState; 
                           
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


        void updatePosition(InteractiveModel unit, Vector2 targetPos, GameTime gameTime)
        {
            float speed = (float)2.5f * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            Vector2 Direction = targetPos - new Vector2(unit.Model.Position.X, unit.Model.Position.Z);
            Direction.Normalize();

            Vector3 UnitSpeed = new Vector3(Direction.X, 0, Direction.Y) * speed; // speed is your unit's speed

            // Now you update position
            unit.Model.Position = new Vector3(unit.Model.Position.X + UnitSpeed.X, StaticHelpers.StaticHelper.GetHeightAt(unit.Model.Position.X + UnitSpeed.X, unit.Model.Position.Z + UnitSpeed.Z), unit.Model.Position.Z + UnitSpeed.Z);
            unit.MyNode = unit.getMyNode();
        }
        public Ray starSelectMouseRay { get; set; }

        public Vector3 selectRectangleStart { get; set; }

        public Ray stopMouseRay { get; set; }

        public Vector3 selectRectangleStop { get; set; }
    }

}

