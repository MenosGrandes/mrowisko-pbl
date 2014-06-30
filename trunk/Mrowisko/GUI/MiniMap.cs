using Logic;
using Logic.Building.AllieBuilding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUI
{
   public class MiniMap
    {

       public const int x=1043;
      public const int y=510;
       public const int width=312;
       public const int height=248;
                   





      private Vector2 cameraPosition;
       private Texture2D enemyTexture,neutralTexture,allieTexture,cameraTexture;
       private List<InteractiveModel> models1;
       public MiniMap(List<InteractiveModel> models)
       {
           enemyTexture = StaticHelpers.StaticHelper.Content.Load<Texture2D>("Textures/Map_Content/enemySign");
           neutralTexture = StaticHelpers.StaticHelper.Content.Load<Texture2D>("Textures/Map_Content/NeutralSign");
           allieTexture = StaticHelpers.StaticHelper.Content.Load<Texture2D>("Textures/Map_Content/friendlySign");
           cameraTexture = StaticHelpers.StaticHelper.Content.Load<Texture2D>("Textures/Map_Content/cameraTexture");

           models1 = models;
       }
       public void addObjects(InteractiveModel model)
       {
           models1.Add(model);
       }
       public void UpdateMinimap(GameTime time,Vector2 cameraPos)
       {
              foreach(InteractiveModel m in models1)
              {
                  m.miniMapPosition = new Vector2(x + width * (m.Model.Position.X / 3075),  y+ height * (m.Model.Position.Z / 3075));
              }
              cameraPosition = new Vector2(x + width * (cameraPos.X / 3075), y + height * (cameraPos.Y / 3075));

       }
       public void Draw(SpriteBatch sp)
       {
           sp.Draw(cameraTexture, cameraPosition, Color.White);
              foreach(InteractiveModel m in models1)
              {

                  if (m.GetType().BaseType == typeof(Ant)) {
                      sp.Draw(allieTexture, m.miniMapPosition, Color.White);
                  }
                  else if(m.GetType().BaseType == typeof(Allie) || m.GetType().BaseType == typeof(AllieBuilding))
                  {
                      sp.Draw(neutralTexture, m.miniMapPosition, Color.White);
                  }
                  else
                  {
                      sp.Draw(enemyTexture, m.miniMapPosition, Color.White);

                  }

              }
       }
    }
}
