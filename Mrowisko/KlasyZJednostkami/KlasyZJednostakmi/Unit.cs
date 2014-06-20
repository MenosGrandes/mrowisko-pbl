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


        private bool moving=false;

        public bool Moving
        {
            get { return moving; }
            set { moving = value; }
        }
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
            speed = 6;
        }
        public Unit()
            : base()
        {
            base.elapsedTime = 0;
            this.armorAfterBuff = armor * 2;
            pathFinder = new PathFinderNamespace.PathFinder();
            speed = 6;

        }
        public Unit(LoadModel model)
            : base(model)
        {

            base.elapsedTime = 0;
            this.armorAfterBuff = armor * 2;
            pathFinder = new PathFinderNamespace.PathFinder();
            speed = 6;
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
            if (ArmorBuff)
            {
                armor = armorAfterBuff;
            }
            else
            {
                armor = armorAfterBuff / 2;
            }

            if (moving)
            {
                float elapsedTime3 = (float)time.ElapsedGameTime.TotalSeconds ;

                // If we have any waypoints, the first one on the list is where 
                // we want to go
                if (movementPath.Count >= 1)
                {
                    destination = movementPath.Peek().centerPosition;
                }

                // If we’re at the destination and there is at least one waypoint in 
                // the list, get rid of the first one since we’re there now
                if (AtDestination && movementPath.Count >= 1)
                {
                    MyNode= movementPath.Dequeue();

                }

                if (!AtDestination)
                {
                    direction = -(new Vector2(Model.Position.X, Model.Position.Z) - destination);
                    //This scales the vector to 1, we'll use move Speed and elapsed Time 
                    //to find the how far the tank moves
                    direction.Normalize();
                    model.Position = new Vector3(Model.Position.X + (direction.X *
                        speed * elapsedTime3),
                        StaticHelpers.StaticHelper.GetHeightAt(Model.Position.X + (direction.X *
                        speed * elapsedTime3), Model.Position.Z + (direction.Y *
                        speed * elapsedTime3))
                        ,
                        Model.Position.Z + (direction.Y *
                        speed * elapsedTime3));
                }
            }

        }
        public override void Intersect(InteractiveModel interactive)
        {
            base.Intersect(interactive);
            //if (interactive.GetType().BaseType.BaseType == typeof(Unit))
            //{
            //    if (model.BoundingSphere.Intersects(interactive.Model.BoundingSphere))
            //    {
            //        //interactive.Model.Position -= Vector3.Left;
            //        float distance2 = Vector3.Distance(model.BoundingSphere.Center, interactive.Model.BoundingSphere.Center);
            //        Vector3 aa = (model.Position - interactive.Model.Position);
            //        if (aa.X > 0)
            //        {
            //            model.Position += Vector3.Right;
            //        }
            //        if (aa.X < 0)
            //        {
            //            model.Position += Vector3.Left;
            //        }
            //    }
            //}
        }
        public bool AtDestination
        {
            get { return DistanceToDestination < 6; }
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