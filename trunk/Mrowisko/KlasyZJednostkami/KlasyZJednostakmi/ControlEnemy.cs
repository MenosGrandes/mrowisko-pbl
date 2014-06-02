
using GameCamera;
using Logic;
using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Logic.Building.AntBuildings.Granary;
using Logic.Building.AntBuildings.SeedFarms;
using System;
using Logic.Units.Ants;
using Logic.Meterials;
using Logic.Meterials.MaterialCluster;
using Logic.Building;
using Logic.Building.AntBuildings;
namespace Logic
{
    public class ControlEnemy
    {

        private int promien = 100;
        float czas = 0.0f;
        int hp = 100;
        public Vector3 S = new Vector3(250.0f, 12.0f, 250.0f);
        public Vector3 pozycja = new Vector3(250.0f, 12.0f, 250.0f);
        public InteractiveModel Enemy;
        public InteractiveModel Ant;
        public GameTime gameTime;
        public List<InteractiveModel> Models_Colision = new List<InteractiveModel>();
        public List<InteractiveModel> Ants = new List<InteractiveModel>();
        private Vector3 poprzedni = Vector3.Zero;
        private int count = 0;
        public List<InteractiveModel> Snared = new List<InteractiveModel>();
       
        public void Update()
        {
            float x;
            float z;
            foreach (InteractiveModel ants1 in Ants)
            {
                if (ants1.snared == true && !Snared.Contains(ants1))
                {
                    count++;
                    Snared.Add(ants1);
                }
                //float spr = (float)Math.Pow(Ant.Model.Position.X - S.X, 2.0) + (float)Math.Pow(Ant.Model.Position.Z - S.Z, 2.0);
                float spr = (float)Math.Pow(ants1.Model.Position.X - S.X, 2.0) + (float)Math.Pow(ants1.Model.Position.Z - S.Z, 2.0);
                //  if (Enemy.Model.Position.X == pozycja.X && Enemy.Model.Position.Z == pozycja.Z && spr > (promien * promien))
                //    {
                //        Random losuj = new System.Random(DateTime.Now.Millisecond);
                //        x = (S.X - promien) + ((float)losuj.NextDouble() * ((S.X + promien) - (S.X - promien)));
                //        z = (S.Z - promien) + ((float)losuj.NextDouble() * ((S.Z + promien) - (S.Z - promien)));

                //         pozycja = new Vector3(x, Enemy.Model.Position.Y, z);
                //    }
                Console.WriteLine(count);
                if (spr <= (promien * promien))
                {
                    pozycja = new Vector3(ants1.Model.Position.X, ants1.Model.Position.Y, ants1.Model.Position.Z);
                    if(count<2)
                    {
                        ants1.snared = true;
                      //  count++;
                   }
                    ants1.time_snared += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (ants1.time_snared > 8.0f)
                    {
                        ants1.snared = false;
                        ants1.time_snared = 9.0f;
                    }
                }
            }
            updateEnemy(pozycja, gameTime);
        }

        public void updateEnemy(Vector3 cel, GameTime gameTime)
        {
            float Speed = 1.2f;

            if (Math.Abs(Enemy.Model.Position.X - cel.X) <= Speed && Math.Abs(Enemy.Model.Position.Z - cel.Z) <= Speed)
            {
                Enemy.Model.Position = cel;
            }

            if (Enemy.Model.BoundingSphere.Intersects(Ant.Model.BoundingSphere))
            {
                czas += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (czas > 2.0f)
                {
                    Console.WriteLine("atak");
                    if (hp > 0)
                        hp -= 10;
                    Console.WriteLine(hp);
                    czas = 0;
                }
            }
            else
            {
                Vector3 lewo, prawo, gora, dol;
                Vector3 lewy_gorny, prawy_gorny, lewy_dolny, prawy_dolny;
                bool czylewo = false, czyprawo = false, czygora = false, czydol = false, czylewy_gorny = false, czyprawy_gorny = false, czylewy_dolny = false, czyprawy_dolny = false;
                lewo = Enemy.Model.Position + (Vector3.Left * Speed);
                prawo = Enemy.Model.Position + (Vector3.Right * Speed);
                gora = Enemy.Model.Position + (Vector3.Forward * Speed);
                dol = Enemy.Model.Position + (Vector3.Backward * Speed);
                lewy_gorny = Enemy.Model.Position + (new Vector3(-1, 0, -1) * Speed);
                lewy_dolny = Enemy.Model.Position + (new Vector3(-1, 0, 1) * Speed);
                prawy_dolny = Enemy.Model.Position + (new Vector3(1, 0, 1) * Speed);
                prawy_gorny = Enemy.Model.Position + (new Vector3(1, 0, -1) * Speed);

                float min;
                int indeks;
                float[] tab = new float[8];

                foreach (InteractiveModel model in Models_Colision)
                {
                    if ((model.Model.BoundingSphere.Contains(lewo) == ContainmentType.Contains))
                        czylewo = true;
                    if ((model.Model.BoundingSphere.Contains(prawo) == ContainmentType.Contains))
                        czyprawo = true;
                    if ((model.Model.BoundingSphere.Contains(gora) == ContainmentType.Contains))
                        czygora = true;
                    if ((model.Model.BoundingSphere.Contains(dol) == ContainmentType.Contains))
                        czydol = true;
                    if ((model.Model.BoundingSphere.Contains(lewy_gorny) == ContainmentType.Contains))
                        czylewy_gorny = true;
                    if ((model.Model.BoundingSphere.Contains(lewy_dolny) == ContainmentType.Contains))
                        czylewy_dolny = true;
                    if ((model.Model.BoundingSphere.Contains(prawy_dolny) == ContainmentType.Contains))
                        czyprawy_dolny = true;
                    if ((model.Model.BoundingSphere.Contains(prawy_gorny) == ContainmentType.Contains))
                        czyprawy_gorny = true;
                }


                if (czylewo == false)
                    tab[0] = (float)Math.Sqrt((float)Math.Pow(lewo.X - cel.X, 2.0f) + (float)Math.Pow(lewo.Z - cel.Z, 2.0f));

                if (czyprawo == false)
                    tab[1] = (float)Math.Sqrt((float)Math.Pow(prawo.X - cel.X, 2.0f) + (float)Math.Pow(prawo.Z - cel.Z, 2.0f));

                if (czygora == false)
                    tab[2] = (float)Math.Sqrt((float)Math.Pow(gora.X - cel.X, 2.0f) + (float)Math.Pow(gora.Z - cel.Z, 2.0f));

                if (czydol == false)
                    tab[3] = (float)Math.Sqrt((float)Math.Pow(dol.X - cel.X, 2.0f) + (float)Math.Pow(dol.Z - cel.Z, 2.0f));

                if (czylewy_gorny == false)
                    tab[4] = (float)Math.Sqrt((float)Math.Pow(lewy_gorny.X - cel.X, 2.0f) + (float)Math.Pow(lewy_gorny.Z - cel.Z, 2.0f));

                if (czylewy_dolny == false)
                    tab[5] = (float)Math.Sqrt((float)Math.Pow(lewy_dolny.X - cel.X, 2.0f) + (float)Math.Pow(lewy_dolny.Z - cel.Z, 2.0f));

                if (czyprawy_dolny == false)
                    tab[6] = (float)Math.Sqrt((float)Math.Pow(prawy_dolny.X - cel.X, 2.0f) + (float)Math.Pow(prawy_dolny.Z - cel.Z, 2.0f));

                if (czyprawy_gorny == false)
                    tab[7] = (float)Math.Sqrt((float)Math.Pow(prawy_gorny.X - cel.X, 2.0f) + (float)Math.Pow(prawy_gorny.Z - cel.Z, 2.0f));

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
                        cel.X = cel.X + Speed;
                    else
                        Enemy.Model.Position += Vector3.Left * Speed;
                    poprzedni = Vector3.Left;
                    Enemy.Model.Rotation = Vector3.Up * (44.8f);

                }
                if (indeks == 1)
                {
                    if (poprzedni == Vector3.Left)
                        // ant.Model.Position += new Vector3(-1, 0, 1) * 4.0f;
                        cel.X = cel.X + Speed;
                    else
                        Enemy.Model.Position += Vector3.Right * Speed;
                    poprzedni = Vector3.Right;
                    Enemy.Model.Rotation = Vector3.Up * 179.9f;
                }
                if (indeks == 2)
                {
                    if (poprzedni == Vector3.Backward)
                        // ant.Model.Position += new Vector3(1, 0, -1) * 4.0f;
                        cel.Z = cel.Z + Speed;
                    else
                        Enemy.Model.Position += Vector3.Forward * Speed;
                    poprzedni = Vector3.Forward;
                    Enemy.Model.Rotation = Vector3.Up * 43.15f;
                }
                if (indeks == 3)
                {
                    if (poprzedni == Vector3.Forward)
                        cel.Z = cel.Z + Speed;
                    else
                        Enemy.Model.Position += Vector3.Backward * Speed;
                    poprzedni = Vector3.Backward;
                    Enemy.Model.Rotation = Vector3.Up * (-179.9f);
                }

                if (indeks == 4)
                {
                    if (poprzedni == prawy_dolny)
                        cel = cel + (new Vector3(-1, 0, -1) * Speed);
                    else
                        Enemy.Model.Position += (new Vector3(-1, 0, -1) * Speed);
                    Enemy.Model.Rotation = Vector3.Up * (-44);
                    poprzedni = lewy_gorny;
                }

                if (indeks == 5)
                {
                    if (poprzedni == prawy_gorny)
                        Enemy.Model.Position += Vector3.Backward * Speed;
                    else
                        Enemy.Model.Position += (new Vector3(-1, 0, 1) * Speed);
                    Enemy.Model.Rotation = Vector3.Up * (-42.4f);
                    poprzedni = lewy_dolny;

                }

                if (indeks == 6)
                {
                    if (poprzedni == lewy_gorny)
                        cel = cel + (new Vector3(-1, 0, -1) * Speed);
                    else
                        Enemy.Model.Position += (new Vector3(1, 0, 1) * Speed);
                    Enemy.Model.Rotation = Vector3.Up * (-47.4f);
                    poprzedni = prawy_dolny;

                }
                if (indeks == 7)
                {
                    if (poprzedni == lewy_dolny)
                        cel = cel + (new Vector3(-1, 0, -1) * Speed);
                    else
                        Enemy.Model.Position += (new Vector3(1, 0, -1) * Speed);
                    Enemy.Model.Rotation = Vector3.Up * (-45.5f);
                    poprzedni = prawy_gorny;
                }

                {


                }
            }




        }

    }



}

