using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Map;


namespace Logic
{
    public class Control
    {
        Vector2 position, velocity;
        int baseSpeed;
        Unit unit;


        public void Move()
        {
            MouseState MS = Mouse.GetState();

            Vector2 mouse_pos = new Vector2(MS.X, MS.Y);
            Vector2 direction = mouse_pos - position;

            velocity = Vector2.Zero;
            float distance = calculateDistance(mouse_pos, position);

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            if (MS.LeftButton == ButtonState.Pressed)
            {
                if (distance < baseSpeed)
                    velocity += direction * distance;
                else
                    velocity += direction * baseSpeed;
            }

            position += velocity;
        }

        private float calculateDistance(Vector2 A, Vector2 B)
        {
            //A pozycja myszy B pozycja gracza
            A = new Vector2(Math.Abs(A.X), Math.Abs(A.Y));
            B = new Vector2(Math.Abs(B.X), Math.Abs(B.Y));
            float X_diff, Y_diff, distance;
            Y_diff = A.Y - B.Y;
            X_diff = A.X - B.X;

            distance = (float)Math.Sqrt(Math.Pow(X_diff, 2.0) + Math.Pow(Y_diff, 2.0));
            return Math.Abs(distance);
        }




    }







}
