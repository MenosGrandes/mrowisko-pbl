using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Controlers
{
    public static class WindowController
    {
        public static Form window;

        public static void setWindowSize(int Width, int Height, bool fullscreen)
        {
           if(fullscreen)
            {
            StaticHelpers.StaticHelper.DeviceManager.IsFullScreen = true;
            }   
           else
           {
            StaticHelpers.StaticHelper.DeviceManager.IsFullScreen = false;
            StaticHelpers.StaticHelper.DeviceManager.PreferredBackBufferHeight = Height;
            StaticHelpers.StaticHelper.DeviceManager.PreferredBackBufferWidth = Width;
            StaticHelpers.StaticHelper.DeviceManager.ApplyChanges();
           } 
        }
    }

}
