﻿
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Logic
{
    public class Curve3D
    {

        public Curve curveX = new Curve();
        public Curve curveY = new Curve();
        public Curve curveZ = new Curve();

        public Curve3D(List<PointInTime>points)
        {
            curveX.PostLoop = CurveLoopType.Oscillate;
            curveY.PostLoop = CurveLoopType.Oscillate;
            curveZ.PostLoop = CurveLoopType.Oscillate;

            curveX.PreLoop = CurveLoopType.Oscillate;
            curveY.PreLoop = CurveLoopType.Oscillate;
            curveZ.PreLoop = CurveLoopType.Oscillate;
           foreach(PointInTime point in points)
           {
               AddPoint(point);
           }
            this.SetTangents();
        }
        public void SetTangents()
        {
            CurveKey prev;
            CurveKey current;
            CurveKey next;
            int prevIndex;
            int nextIndex;
            for (int i = 0; i < curveX.Keys.Count; i++)
            {
                prevIndex = i - 1;
                if (prevIndex < 0) prevIndex = i;

                nextIndex = i + 1;
                if (nextIndex == curveX.Keys.Count) nextIndex = i;

                prev = curveX.Keys[prevIndex];
                next = curveX.Keys[nextIndex];
                current = curveX.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                curveX.Keys[i] = current;
                prev = curveY.Keys[prevIndex];
                next = curveY.Keys[nextIndex];
                current = curveY.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                curveY.Keys[i] = current;

                prev = curveZ.Keys[prevIndex];
                next = curveZ.Keys[nextIndex];
                current = curveZ.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                curveZ.Keys[i] = current;
            }
        }
        static void SetCurveKeyTangent(ref CurveKey prev, ref CurveKey cur,
            ref CurveKey next)
        {
            float dt = next.Position - prev.Position;
            float dv = next.Value - prev.Value;
            if (Math.Abs(dv) < float.Epsilon)
            {
                cur.TangentIn = 0;
                cur.TangentOut = 0;
            }
            else
            {
                // The in and out tangents should be equal to the 
                // slope between the adjacent keys.
                cur.TangentIn = dv * (cur.Position - prev.Position) / dt;
                cur.TangentOut = dv * (next.Position - cur.Position) / dt;
            }
        }

        public void AddPoint(PointInTime point)
        {
            curveX.Keys.Add(new CurveKey(point.time, point.point.X));
            curveY.Keys.Add(new CurveKey(point.time, point.point.Y));
            curveZ.Keys.Add(new CurveKey(point.time, point.point.Z));
        }
        public Vector3 GetPointOnCurve(float time)
        {
            Vector3 point = new Vector3();
            point.X = curveX.Evaluate(time);
            point.Y = curveY.Evaluate(time);
            point.Z = curveZ.Evaluate(time);
            return point;
        }
       public void InitCurve()
        {   /*
            float time = 0;
            this.AddPoint(new Vector3(75, 40, 450), time);
            time += 2000;
            this.AddPoint(new Vector3(30, 40, 360), time);
            time += 2000;
            this.AddPoint(new Vector3(120, 40, 300), time);
            time += 2000;
            this.AddPoint(new Vector3(30, 40, 240), time);
            time += 2000;
            this.AddPoint(new Vector3(120, 40, 180), time);
            time += 2000;
            this.AddPoint(new Vector3(750, 40, 120), time);
            time += 2000;
            this.AddPoint(new Vector3(60, 40, 60), time);
            time += 3000;
            this.AddPoint(new Vector3(270, 40, 60), time);
            time += 3000;
            this.AddPoint(new Vector3(60, 40, 210), time);
            time += 2000;
            this.AddPoint(new Vector3(750, 40, 270), time);
            time += 2000;
            this.AddPoint(new Vector3(210, 40, 210), time);
            time += 3000;
            this.AddPoint(new Vector3(420, 40, 60), time);
            time += 3000;
            this.AddPoint(new Vector3(210, 40, 60), time);
             */
            this.SetTangents();
        }
    }
}
