using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;
using Controlers.CursorEnum;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using Map;

namespace Controlers
{
    public static class MouseCursorController
    {
        private static List<Cursor> cursors = new List<Cursor>();
        public static CursorStage stage;

        public static void LoadCustomCursor(string path)
        {
            IntPtr hCurs = LoadCursorFromFile(path);
            if (hCurs == IntPtr.Zero) throw new Win32Exception();
            var curs = new Cursor(hCurs);
            // Note: force the cursor to own the handle so it gets released properly
            var fi = typeof(Cursor).GetField("ownHandle", BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(curs, true);

            cursors.Add(curs);
        }
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadCursorFromFile(string path);


        public static void Update()
        {
               switch (stage )
            {
                case CursorStage.   Attack: WindowController.window.Cursor = cursors[0]; break;
                case CursorStage.Go: WindowController.window.Cursor = cursors[1]; break;
                case CursorStage.Gater: WindowController.window.Cursor = cursors[2]; break;
                case CursorStage.Normal: WindowController.window.Cursor = cursors[3]; break;
            }
        }
    }
    public static class QuadNodeHelper
{
        public static List<QuadNode> quadNodeList;
        public static Vector3 getIntersectedQuadNode(Ray intersected)
        {

            foreach (QuadNode q in QuadNodeController.QuadNodeList)
            {
                if ((intersected.Intersects(q.Bounds)) != null)
                {
                    return q.Bounds.Min + (q.Bounds.Max - q.Bounds.Min) / 2;
                }
            }

            return Vector3.Zero;
        }
}
    
}
