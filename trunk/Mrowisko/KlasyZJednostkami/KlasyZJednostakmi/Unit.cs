using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Map;
//using Skills;

namespace Logic
{
    [Serializable]
    public class Unit : InteractiveModel
    {

        public Vector2 destination { get; set; }
        public Vector2 direction { get; set; }
        public Vector2 middlePoint { get; set; }
        public float tempHeight { get; set; }

        private bool moving=false;

        public bool Moving
        {
            get { return moving; }
            set { moving = value; }
        }


        private bool jumping = false;
        public bool Jumping
        {
            get { return jumping; }
            set { jumping = value; }
        }

        private bool halfOfJump = false;

        
        public List<InteractiveModel> obstacles = new List<InteractiveModel>();

     
             
        private PathFinderNamespace.PathFinder pathFinder;

        public PathFinderNamespace.PathFinder PathFinder
        {
            get { return pathFinder; }
            set { pathFinder = value; }
        }
        private Queue<Node> movementPath;

        public Queue<Node> MovementPath
        {
            get { return movementPath; }
            set { movementPath = value; }
        }

        public uint modelHeight = 0;

        protected float armorAfterBuff;

        public float ArmorAfterBuff
        {
            get { return armorAfterBuff; }
            set { armorAfterBuff = value; }
        }

        protected float armor;

        public float Armor
        {
            get { return armor; }
            set { armor = value; }
        }
        protected float strength;

        public float Strength
        {
            get { return strength; }
            set { strength = value; }
        }

        protected float range;

        public float Range
        {
            get { return range; }
            set { range = value; }
        }

        

        protected float speed;

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }


        protected float buildingTime;

        public float BuildingTime
        {
            get { return buildingTime; }
            set { buildingTime = value; }
        }

        protected int cost = 0;

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }


        protected float atackInterval;

        public float AtackInterval
        {
            get { return atackInterval; }
            set { atackInterval = value; }
        }

        protected bool ifLeader = false;

        public bool IfLeader
        {
            get { return ifLeader; }
            set { ifLeader = value; }
        }


        /*
         protected List<Skill> skillsList;

         public List<Skill> SkillsList
         {
             get { return skillsList; }
             set { skillsList = value; }
         }
        */



        public Unit(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, float atackInterval)
            : base(model)
        {


            this.hp = hp;
            this.armor = armor;
            this.strength = strength;
            this.range = range;
            this.cost = cost;
            this.buildingTime = buildingTime;
            this.atackInterval = atackInterval;
            base.elapsedTime = 0;
            this.armorAfterBuff = armor * 2;
            pathFinder = new PathFinderNamespace.PathFinder();
            speed = 42;
        }
        public Unit()
            : base()
        {
            base.elapsedTime = 0;
            this.armorAfterBuff = armor * 2;
            pathFinder = new PathFinderNamespace.PathFinder();
            speed = 42;

        }
        public Unit(LoadModel model)
            : base(model)
        {

            base.elapsedTime = 0;
            this.armorAfterBuff = armor * 2;
            pathFinder = new PathFinderNamespace.PathFinder();
            speed = 42;
        }
        public Vector3 getPosition()
        {
            return this.model.Position;
        }
        public void setPosition(Vector3 position)
        {
            this.model.Position = position;
        }

        public Vector3 getRotation()
        {
            return this.model.Rotation;
        }

        public void setRotation(Vector3 rotation)
        {
            this.model.Rotation = rotation;
        }
        public override void Update(GameTime time)
        {
            base.Update(time);

            if (this.GetType() == typeof(Transporter))
            {
                if (Vector2.Distance(new Vector2(this.Model.BoundingSphere.Center.X, this.Model.BoundingSphere.Center.Z), new Vector2(this.transportTarget.X, this.transportTarget.Z)) >= this.Model.BoundingSphere.Radius)
                this.reachTargetTransporter(time, this.transportTarget);

            }
            else
            {

                if (this.Model.snr == true)
                {

                    temp_time += (float)time.ElapsedGameTime.Milliseconds / 1000;
                    if (temp_time > 4)
                    {
                        this.Model.snr = false;

                        temp_time = 0.0f;

                    }

                }

                MyNode = this.getMyNode();

                if (this.foe != null)
                {
                    if (this.foe.Hp > 0 && this.hasBeenHit)
                    {
                        this.target = foe;
                        //attacking = true;
                        model.Rotation = new Vector3(model.Rotation.X, StaticHelpers.StaticHelper.TurnToFace(new Vector2(model.Position.X, model.Position.Z), new Vector2(this.foe.Model.Position.X, this.foe.Model.Position.Z), model.Rotation.Y, 1.05f), model.Rotation.Z);
                    }
                    else
                    {
                        this.foe = null;
                    }
                }

                if (this.Model.snr == false && this.attacking == false)
                {
                    if (ArmorBuff)
                    {
                        armor = armorAfterBuff;
                    }
                    else
                    {
                        armor = armorAfterBuff / 2;
                    }

                    if (ifLeader)
                    {
                        this.goToTarget(time);
                    }
                    else
                    {
                        this.follow(time);
                    }
                }
                MyNode = this.getMyNode();
            }
        }

        public void follow(GameTime time)
        {
           
            if (moving)
            {
                float elapsedTime3 = (float)time.ElapsedGameTime.TotalSeconds;
                this.model.switchAnimation("Walk");
              
                if (!AtDestination && (target==null || Vector3.Distance(target.Model.Position, this.model.Position)>this.rangeOfSight))
                {
                    direction = -(new Vector2(Model.Position.X, Model.Position.Z) - destination);
                    //This scales the vector to 1, we'll use move Speed and elapsed Time 
                    //to find the how far the tank moves
                    direction =Vector2.Normalize(direction);
                    
                    model.Position = new Vector3(Model.Position.X + (direction.X *
                        speed * elapsedTime3),
                        StaticHelpers.StaticHelper.GetHeightAt(Model.Position.X + (direction.X *
                        speed * elapsedTime3), Model.Position.Z + (direction.Y *
                        speed * elapsedTime3)) + modelHeight
                        ,
                        Model.Position.Z + (direction.Y *
                        speed * elapsedTime3));
                    model.Rotation = new Vector3(model.Rotation.X, StaticHelpers.StaticHelper.TurnToFace(new Vector2(model.Position.X, model.Position.Z), destination, model.Rotation.Y, 1.05f), model.Rotation.Z);
                }
                else if (this.target != null && Vector3.Distance(target.Model.Position, this.model.Position) < this.rangeOfSight)
                {
                   // movementPath.Clear();
                    if (Vector3.Distance(this.target.Model.Position, this.model.Position) >= this.model.BoundingSphere.Radius/2)
                        this.reachTarget(time, this, target.Model.Position);
                }
                /*if(AtDestination || ( target != null && Vector3.Distance(target.Model.Position, this.model.Position) > this.rangeOfSight))
                {
                    this.model.switchAnimation("Idle");
                }*/
               
                
            }

        }

        public void goToTarget(GameTime time)
        {
            

            if (moving)
            {
                float elapsedTime3 = (float)time.ElapsedGameTime.TotalSeconds;
                this.model.switchAnimation("Walk");
                // If we have any waypoints, the first one on the list is where 
                // we want to go
                if (movementPath.Count >= 1)
                {
                    destination = movementPath.Peek().centerPosition;
                }
              /*  if (target != null && Vector3.Distance(target.Model.Position, this.model.Position) < this.rangeOfSight)
                {
                    if (movementPath.Count >= 1)
                    movementPath.Clear();
                    if (Vector3.Distance(target.Model.Position, this.model.Position) >= this.model.BoundingSphere.Radius / 2)
                        this.reachTarget(time, this, target.Model.Position);
                }*/

                // If we’re at the destination and there is at least one waypoint in 
                // the list, get rid of the first one since we’re there now
                if (AtDestination && movementPath.Count >= 1)
                {
                    MyNode = movementPath.Dequeue();

                }
                if (movementPath.Count < 1 && target == null)
                {
                    this.moving = false;
                }

                if (!AtDestination)
                {
                    direction = -(new Vector2(Model.Position.X, Model.Position.Z) - destination);
                    //This scales the vector to 1, we'll use move Speed and elapsed Time 
                    //to find the how far the tank moves
                    direction = Vector2.Normalize(direction);
                    this.reachTarget(time, this, new Vector3(destination.X, StaticHelpers.StaticHelper.GetHeightAt(destination.X, destination.Y) + modelHeight, destination.Y));
                    /*model.Position = new Vector3(Model.Position.X + (direction.X *
                        speed * elapsedTime3),
                        StaticHelpers.StaticHelper.GetHeightAt(Model.Position.X + (direction.X *
                        speed * elapsedTime3), Model.Position.Z + (direction.Y *
                        speed * elapsedTime3))
                        ,
                        Model.Position.Z + (direction.Y *
                        speed * elapsedTime3));*/
                    model.Rotation = new Vector3(model.Rotation.X, StaticHelpers.StaticHelper.TurnToFace(new Vector2(model.Position.X, model.Position.Z), destination, model.Rotation.Y, 1.0f), model.Rotation.Z);
                    //Console.WriteLine(model.Rotation.Y);
                }
            }
          
              
            
            //if (jumping)
            //{
            //    this.model.switchAnimation("Jump");
            //    if(!AtDestination)
            //    {
            //        //direction = -(new Vector2(Model.Position.X, Model.Position.Z) - destination);
            //        float maxHeight = (direction.Length() / 2) + StaticHelpers.StaticHelper.GetHeightAt(model.Position.X, model.Position.Z);
            //        //direction = Vector2.Normalize(direction);

            //        float tempHeight = StaticHelpers.StaticHelper.GetHeightAt(model.Position.X, model.Position.Z);
            //        if (!halfOfJump)
            //        {
            //            if (Math.Abs(middlePoint.X - model.Position.X) <= 1f || Math.Abs(middlePoint.Y - model.Position.Z) <= 1f)
            //             {
            //                 halfOfJump = true;
            //             }
            //            model.Position += new Vector3(direction.X, 100, direction.Y); 
            //            tempHeight += 20;
                        
            //        }
            //        else
            //        {
            //            tempHeight -= 20;
            //            model.Position += new Vector3(direction.X, -100, direction.Y);
                       
            //        }
                    
            //    }
            //    else if (AtDestination)
            //    {
            //        model.Position = new Vector3(model.Position.X, StaticHelpers.StaticHelper.GetHeightAt(model.Position.X, model.Position.Z) + modelHeight, model.Position.Z);
            //        jumping = false;
            //        halfOfJump = false;
            //        model.switchAnimation("Idle");

            //    }
            //}

            if(!this.moving && !this.jumping)
            this.model.switchAnimation("Idle");
        }

        public void reachTargetAutonomus(GameTime gameTime,Vector3 target)
        {
          

               
                if (Vector2.Distance(new Vector2(Model.Position.X, Model.Position.Z), new Vector2(target.X, target.Z)) < 1)
                {
                    ImMoving = false;
                    
                }
                float pullDistance = Vector2.Distance(new Vector2(target.X, target.Z), new Vector2(this.Model.Position.X, this.Model.Position.Y));

                //Only do something if we are not already there
                if (pullDistance > 1)
                {
                    Vector3 pull = (target - Model.Position) * (1 / pullDistance); //the target tries to 'pull us in'
                    Vector3 totalPush = Vector3.Zero;

                    int contenders = 0;
                    for (int i = 0; i < obstacles.Count; ++i)
                    {

                        Vector3 push = Model.Position - obstacles[i].Model.Position;

                        //calculate how much we are pushed away from this obstacle, the closer, the more push
                        float distance = (Vector3.Distance(Model.Position, obstacles[i].Model.Position) - obstacles[i].Model.BoundingSphere.Radius) - Model.BoundingSphere.Radius;
                        //only use push force if this object is close enough such that an effect is needed
                        if (distance < Model.BoundingSphere.Radius * 2)
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

                    Model.Position = new Vector3(Model.Position.X + (pull.X * 60) * (float)gameTime.ElapsedGameTime.TotalSeconds, StaticHelpers.StaticHelper.GetHeightAt(Model.Position.X, Model.Position.Z) + height, Model.Position.Z + (pull.Z * 60) * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }

        public void reachTargetTransporter(GameTime gameTime, Vector3 target)
        {



            if (Vector2.Distance(new Vector2(Model.Position.X, Model.Position.Z), new Vector2(Model.playerTarget.X, Model.playerTarget.Z)) < 1 || Model.playerTarget.Y > 10)
            {
                ImMoving = false;

            }
            float pullDistance = Vector2.Distance(new Vector2(target.X, target.Z), new Vector2(this.Model.Position.X, this.Model.Position.Y));

            //Only do something if we are not already there
            if (this.model.BoundingSphere.Center.Y < 49 || this.transportTarget.Y < 49)
            {
                Vector3 pull = (target - Model.Position) * (1 / pullDistance); //the target tries to 'pull us in'
                Vector3 totalPush = Vector3.Zero;

                int contenders = 0;
                for (int i = 0; i < obstacles.Count; ++i)
                {

                    Vector3 push = Model.Position - obstacles[i].Model.Position;

                    //calculate how much we are pushed away from this obstacle, the closer, the more push
                    float distance = (Vector3.Distance(Model.Position, obstacles[i].Model.Position) - obstacles[i].Model.BoundingSphere.Radius) - Model.BoundingSphere.Radius;
                    //only use push force if this object is close enough such that an effect is needed
                    if (distance < Model.BoundingSphere.Radius * 2)
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

                Model.Position = new Vector3(Model.Position.X + (pull.X * 60) * (float)gameTime.ElapsedGameTime.TotalSeconds, StaticHelpers.StaticHelper.GetHeightAt(Model.Position.X, Model.Position.Z) + height, Model.Position.Z + (pull.Z * 60) * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public void reachTarget(GameTime gameTime, InteractiveModel ship, Vector3 target)
        {
            float pullDistance = Vector2.Distance(new Vector2(target.X,target.Z), new Vector2(ship.Model.Position.X, ship.Model.Position.Y));



            //Only do something if we are not already there
          //  if (pullDistance > 1)
            
                Vector3 pull = (target - ship.Model.Position) * (1 / pullDistance); //the target tries to 'pull us in'
                Vector3 totalPush = Vector3.Zero;
                this.moving = true;

                int contenders = 0;
                for (int i = 0; i < obstacles.Count; ++i)
                {
                    //draw a vector from the obstacle to the ship, that 'pushes the ship away'
                    Vector3 push = ship.Model.Position - obstacles[i].Model.Position;

                    //calculate how much we are pushed away from this obstacle, the closer, the more push
                    float distance = (Vector3.Distance(ship.Model.Position, obstacles[i].Model.Position) - obstacles[i].Model.BoundingSphere.Radius) - ship.Model.BoundingSphere.Radius;
                    //only use push force if this object is close enough such that an effect is needed
                    if (distance < ship.Model.BoundingSphere.Radius)
                    {
                        ++contenders; //note that this object is actively pushing

                        if (distance < 0.0001f) //prevent division by zero errors and extreme pushes
                        {
                            distance = 0.0001f;
                        }
                        float weight = 1 / distance;

                //        totalPush += push * weight;
                    }
                }

                pull *= Math.Max(1, 4 * contenders); //4 * contenders gives the pull enough force to pull stuff trough (tweak this setting for your game!)
                pull += totalPush;

                //Normalize the vector so that we get a vector that points in a certain direction, which we van multiply by our desired speed
                pull.Normalize();
                //Set the ships new position;
           //     if (this.target != null && Vector3.Distance(this.target.Model.Position, this.model.Position) < this.rangeOfSight)
             //   {
               //     model.Rotation = new Vector3(model.Rotation.X, StaticHelpers.StaticHelper.TurnToFace(new Vector2(model.Position.X, model.Position.Z), new Vector2(this.target.Model.Position.X, this.target.Model.Position.Y), model.Rotation.Y, 1.0f), model.Rotation.Z);
                //}
                ship.Model.Position = new Vector3(ship.Model.Position.X + (pull.X * 60) * (float)gameTime.ElapsedGameTime.TotalSeconds, StaticHelpers.StaticHelper.GetHeightAt(ship.Model.Position.X, ship.Model.Position.Z) + modelHeight, ship.Model.Position.Z + (pull.Z * 60) * (float)gameTime.ElapsedGameTime.TotalSeconds);


                if (this.target != null && pullDistance < this.target.Model.BoundingSphere.Radius/2)
                {

                    this.moving = false;
                }
          
        }

        public override void obstaclesOnRoad(List<InteractiveModel> obstacles)
        {
            foreach(InteractiveModel unit in obstacles)
            {
                if (!this.obstacles.Contains(unit))
                {
                    if (Vector2.Distance(new Vector2(this.Model.Position.X, this.Model.Position.Z), new Vector2(unit.Model.Position.X, unit.Model.Position.X)) < 150)
                        this.obstacles.Add(unit);
                }
                else if (this.obstacles.Contains(unit) && unit.Model.Selected)
                {
                    this.obstacles.Remove(unit);
                }
            }
            
        }


        public override void Intersect(InteractiveModel interactive)
        {
            base.Intersect(interactive);
        }
        public bool AtDestination
        {
            get
            {
                if (this.IfLeader) return DistanceToDestination < 6;
                else { return DistanceToDestination < this.model.BoundingSphere.Radius + 3; }
            }
        }
        public float DistanceToDestination
        {
            get { return Vector2.Distance(new Vector2(Model.Position.X, Model.Position.Z), destination); }
        }

        /// <summary>
        /// True when the tank is "close enough" to it's destination
        /// </summary>


    }
}