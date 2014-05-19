using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic;
namespace SimpleStaticHelpers
{
   public static class CreatorController
    {
       public static List<InteractiveModel> models=new List<InteractiveModel>();
       public static ContentManager content;
       public static GraphicsDevice device;
    }
}
