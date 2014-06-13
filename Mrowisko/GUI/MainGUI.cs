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
using Logic;
using Logic.Building;

namespace GUI
{
    public class MainGUI
    {

        public InteractiveModel selectedModel;
        public List<InteractiveModel> selectedModels;
        private Control control;
        public Control Control
        {
            get { return control; }
            set { control = value; }
        }

        enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }
        const int NUMBER_OF_BUTTONS = 17,
            MENU_BUTTON_IDX = 0,
            UP_BUTTON_IDX = 1,
            DOWN_BUTTON_IDX = 2,
            ATTACK_ANT_BUTTON_IDX = 3,
            DEFENCE_ANT_BUTTON_IDX = 4,
            RUN_ANT_BUTTON_IDX = 5,
            SAVE_BUTTON_IDX = 6,
            PAUSE_BUTTON_IDX = 7,
            PLAY_BUTTON_IDX = 8,

            B1_BUTTON_IDX = 9,
            B2_BUTTON_IDX = 10,
            B3_BUTTON_IDX = 11,

            RESUME_T_BUTTON_IDX = 12,
            SAVE_T_BUTTON_IDX = 13,
            LOAD_T_BUTTON_IDX = 14,
            OPTIONS_T_BUTTON_IDX = 15,
            EXIT_T_BUTTON_IDX = 16,

            BUTTON_HEIGHT = 54,
            BUTTON_WIDTH = 342;
        // Color background_color;
        Color[] button_color = new Color[NUMBER_OF_BUTTONS];
        Rectangle[] button_rectangle = new Rectangle[NUMBER_OF_BUTTONS];
        BState[] button_state = new BState[NUMBER_OF_BUTTONS];
        Texture2D[] button_texture = new Texture2D[NUMBER_OF_BUTTONS];
        double[] button_timer = new double[NUMBER_OF_BUTTONS];
        //mouse pressed and mouse just pressed
        bool mpressed, prev_mpressed = false;
        //mouse location in window
        int mx, my;
        double frame_time;


        bool unitMenuON = true;
        bool buildMenuON = false;
        bool maxMenu = false;
        bool mainMenuOn = false;


        const int NUMBER_OF_BARS = 12,
            INFO_BAR_IDX = 0,
            MAP_BAR_IDX = 1,
            Q_MENU_BAR = 2,
            B_BAR_IDX = 3,
            U_BAR_IDX = 4,
            MAX_UNIT_BAR = 5,
            MIN_UNIT_BAR = 6,
            PAUSE_MENU_BACK = 7,
            MINI_MAP = 8,
            FRIEND_SIGN = 9,
            NEUTRAL_SIGN = 10,
            ENEMY_SIGN = 11;

        Color[] itf_color = new Color[NUMBER_OF_BARS];
        Rectangle[] itf_rectangle = new Rectangle[NUMBER_OF_BARS];
        Texture2D[] itf_texture = new Texture2D[NUMBER_OF_BARS];

        public MainGUI(ContentManager content, Control c)
        {
            Initialize();
            LoadContent(content);
            this.control = c;
        }

       public void Initialize()
        {

            /// Interface ----------------------------------------------------------------------------------------

            int x = 800 / 2 - BUTTON_WIDTH / 2;
            int y = 480 / 2 -
                NUMBER_OF_BUTTONS / 2 * BUTTON_HEIGHT -
                (NUMBER_OF_BUTTONS % 2) * BUTTON_HEIGHT / 2;
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                button_state[i] = BState.UP;
                button_color[i] = Color.White;
                button_timer[i] = 0.0;
                button_rectangle[i] = new Rectangle(x, y, BUTTON_WIDTH, BUTTON_HEIGHT);
                y += BUTTON_HEIGHT;
            }


            //=== === === BARS
            itf_color[INFO_BAR_IDX] = Color.White;
            itf_rectangle[INFO_BAR_IDX] = new Rectangle(164, 20, 722, 49);

            itf_color[MAP_BAR_IDX] = Color.White;
            itf_rectangle[MAP_BAR_IDX] = new Rectangle(1024, 20, 325, 270);

            itf_color[Q_MENU_BAR] = Color.White;
            itf_rectangle[Q_MENU_BAR] = new Rectangle(886, 20, 138, 49);


            itf_color[B_BAR_IDX] = Color.White;
            itf_rectangle[B_BAR_IDX] = new Rectangle(712, 676, 316, 84);

            itf_color[U_BAR_IDX] = Color.White;
            itf_rectangle[U_BAR_IDX] = new Rectangle(712, 676, 316, 84);

            itf_color[MAX_UNIT_BAR] = Color.White;
            itf_rectangle[MAX_UNIT_BAR] = new Rectangle(1024, 438, 325, 321);

            itf_color[MIN_UNIT_BAR] = Color.White;
            itf_rectangle[MIN_UNIT_BAR] = new Rectangle(1024, 676, 325, 84);

            itf_color[PAUSE_MENU_BACK] = Color.White;
            itf_rectangle[PAUSE_MENU_BACK] = new Rectangle(1, 1, 1366, 768);

            itf_color[MINI_MAP] = Color.White;
            itf_rectangle[MINI_MAP] = new Rectangle(1036, 28, 300, 250);

            //=== === === BUTTONS
            // menu

            button_state[MENU_BUTTON_IDX] = BState.UP;
            button_color[MENU_BUTTON_IDX] = Color.White;
            button_timer[MENU_BUTTON_IDX] = 0.0;
            button_rectangle[MENU_BUTTON_IDX] = new Rectangle(20, 20, 67, 50);

            // save

            button_state[SAVE_BUTTON_IDX] = BState.UP;
            button_color[SAVE_BUTTON_IDX] = Color.White;
            button_timer[SAVE_BUTTON_IDX] = 0.0;
            button_rectangle[SAVE_BUTTON_IDX] = new Rectangle(900, 30, 30, 30);

            // pause

            button_state[PAUSE_BUTTON_IDX] = BState.UP;
            button_color[PAUSE_BUTTON_IDX] = Color.White;
            button_timer[PAUSE_BUTTON_IDX] = 0.0;
            button_rectangle[PAUSE_BUTTON_IDX] = new Rectangle(940, 30, 30, 30);

            // play

            button_state[PLAY_BUTTON_IDX] = BState.UP;
            button_color[PLAY_BUTTON_IDX] = Color.White;
            button_timer[PLAY_BUTTON_IDX] = 0.0;
            button_rectangle[PLAY_BUTTON_IDX] = new Rectangle(980, 30, 30, 30);

            //=== BUDYNKI -----------------------------------------------------------

            // B1

            button_state[B1_BUTTON_IDX] = BState.UP;
            button_color[B1_BUTTON_IDX] = Color.White;
            button_timer[B1_BUTTON_IDX] = 0.0;
            button_rectangle[B1_BUTTON_IDX] = new Rectangle(845, 680, 51, 52);

            // B2

            button_state[B2_BUTTON_IDX] = BState.UP;
            button_color[B2_BUTTON_IDX] = Color.White;
            button_timer[B2_BUTTON_IDX] = 0.0;
            button_rectangle[B2_BUTTON_IDX] = new Rectangle(905, 680, 51, 52);

            // B3

            button_state[B3_BUTTON_IDX] = BState.UP;
            button_color[B3_BUTTON_IDX] = Color.White;
            button_timer[B3_BUTTON_IDX] = 0.0;
            button_rectangle[B3_BUTTON_IDX] = new Rectangle(965, 680, 51, 52);

            //=== BUDYNKI -----------------------------------------------------------

            // defense

            button_state[DEFENCE_ANT_BUTTON_IDX] = BState.UP;
            button_color[DEFENCE_ANT_BUTTON_IDX] = Color.White;
            button_timer[DEFENCE_ANT_BUTTON_IDX] = 0.0;
            button_rectangle[DEFENCE_ANT_BUTTON_IDX] = new Rectangle(845, 680, 51, 52);

            // attack

            button_state[ATTACK_ANT_BUTTON_IDX] = BState.UP;
            button_color[ATTACK_ANT_BUTTON_IDX] = Color.White;
            button_timer[ATTACK_ANT_BUTTON_IDX] = 0.0;
            button_rectangle[ATTACK_ANT_BUTTON_IDX] = new Rectangle(905, 680, 51, 52);

            // RUN

            button_state[RUN_ANT_BUTTON_IDX] = BState.UP;
            button_color[RUN_ANT_BUTTON_IDX] = Color.White;
            button_timer[RUN_ANT_BUTTON_IDX] = 0.0;
            button_rectangle[RUN_ANT_BUTTON_IDX] = new Rectangle(965, 680, 51, 52);

            //=== menu wyœwietlane -------------------------------------------------

            button_state[RESUME_T_BUTTON_IDX] = BState.UP;
            button_color[RESUME_T_BUTTON_IDX] = Color.White;
            button_timer[RESUME_T_BUTTON_IDX] = 0.0;
            button_rectangle[RESUME_T_BUTTON_IDX] = new Rectangle(133, 307, 342, 54);

            button_state[SAVE_T_BUTTON_IDX] = BState.UP;
            button_color[SAVE_T_BUTTON_IDX] = Color.White;
            button_timer[SAVE_T_BUTTON_IDX] = 0.0;
            button_rectangle[SAVE_T_BUTTON_IDX] = new Rectangle(133, 361, 342, 54);

            button_state[LOAD_T_BUTTON_IDX] = BState.UP;
            button_color[LOAD_T_BUTTON_IDX] = Color.White;
            button_timer[LOAD_T_BUTTON_IDX] = 0.0;
            button_rectangle[LOAD_T_BUTTON_IDX] = new Rectangle(133, 415, 342, 54);

            button_state[OPTIONS_T_BUTTON_IDX] = BState.UP;
            button_color[OPTIONS_T_BUTTON_IDX] = Color.White;
            button_timer[OPTIONS_T_BUTTON_IDX] = 0.0;
            button_rectangle[OPTIONS_T_BUTTON_IDX] = new Rectangle(133, 469, 342, 54);

            button_state[EXIT_T_BUTTON_IDX] = BState.UP;
            button_color[EXIT_T_BUTTON_IDX] = Color.White;
            button_timer[EXIT_T_BUTTON_IDX] = 0.0;
            button_rectangle[EXIT_T_BUTTON_IDX] = new Rectangle(133, 523, 342, 54);

            //=== unit menu --------------------------------------------------------

            button_state[UP_BUTTON_IDX] = BState.UP;
            button_color[UP_BUTTON_IDX] = Color.White;
            button_timer[UP_BUTTON_IDX] = 0.0;
            button_rectangle[UP_BUTTON_IDX] = new Rectangle(1028, 679, 57, 25);

            button_state[DOWN_BUTTON_IDX] = BState.UP;
            button_color[DOWN_BUTTON_IDX] = Color.White;
            button_timer[DOWN_BUTTON_IDX] = 0.0;
            button_rectangle[DOWN_BUTTON_IDX] = new Rectangle(1028, 441, 57, 25);
       }
        public void LoadContent(ContentManager Content)
       {

           /// Interface -------------------------------------------------------------------------------

           button_texture[MENU_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/menuButton");
           button_texture[ATTACK_ANT_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/attackButton");
           button_texture[DEFENCE_ANT_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/defenceButton");
           button_texture[RUN_ANT_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/runOutButton");
           button_texture[B1_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/b1Button");
           button_texture[B2_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/b2Button");
           button_texture[B3_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/b3Button");

           button_texture[SAVE_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/saveButton");
           button_texture[PAUSE_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/pauseButton");
           button_texture[PLAY_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/playButton");

           button_texture[RESUME_T_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/resumeTButton");
           button_texture[SAVE_T_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/saveTButton");
           button_texture[LOAD_T_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/loadTButton");
           button_texture[OPTIONS_T_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/optionsTButton");
           button_texture[EXIT_T_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/exitTButton");

           button_texture[UP_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/upButton");
           button_texture[DOWN_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/downButton");

           itf_texture[INFO_BAR_IDX] =
               Content.Load<Texture2D>(@"Textures/Bars/infoBar");
           itf_texture[MAP_BAR_IDX] =
               Content.Load<Texture2D>(@"Textures/Bars/mapBar");
           itf_texture[Q_MENU_BAR] =
               Content.Load<Texture2D>(@"Textures/Bars/qMenuBar");
           itf_texture[B_BAR_IDX] =
               Content.Load<Texture2D>(@"Textures/Bars/bBar");
           itf_texture[U_BAR_IDX] =
               Content.Load<Texture2D>(@"Textures/Bars/uBar");
           itf_texture[MAX_UNIT_BAR] =
               Content.Load<Texture2D>(@"Textures/Bars/maxUnitBar");
           itf_texture[MIN_UNIT_BAR] =
               Content.Load<Texture2D>(@"Textures/Bars/minUnitBar");
           itf_texture[PAUSE_MENU_BACK] =
               Content.Load<Texture2D>(@"Textures/Bars/PAUSE_MENU_BACK");
           itf_texture[MINI_MAP] =
               Content.Load<Texture2D>(@"Textures/Map_Content/MiniMap");
           itf_texture[FRIEND_SIGN] =
               Content.Load<Texture2D>(@"Textures/Map_Content/friendlySign");
           itf_texture[NEUTRAL_SIGN] =
               Content.Load<Texture2D>(@"Textures/Map_Content/neutralSign");
           itf_texture[ENEMY_SIGN] =
               Content.Load<Texture2D>(@"Textures/Map_Content/enemySign");


       }
        public void Update(GameTime gameTime)
        {

            ///interface --------------------------------------------------------------------------------
            if (control.selectedObject != null) { 
            selectedModel = control.selectedObject;
                if(selectedModel.GetType().IsSubclassOf(typeof(Ant)))
                {
                     unitMenuON=true;
                     buildMenuON = false;
                }
                else if (selectedModel.GetType().IsSubclassOf(typeof(Building)))
                {
                 buildMenuON=true;
                 unitMenuON = false;

                }

            }
            if (control.SelectedModels.Count > 0) { 
            selectedModels = control.SelectedModels;
            }

            // get elapsed frame time in seconds
            frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            // update mouse variables
            MouseState mouse_state = Mouse.GetState();
            mx = mouse_state.X;
            my = mouse_state.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;

            update_buttons();

        }
        public void Draw(SpriteBatch spriteBatch)
        {

           
            /* tutaj popoprawiaæ*/
        
           
       

            spriteBatch.Draw(button_texture[MENU_BUTTON_IDX], button_rectangle[MENU_BUTTON_IDX], button_color[MENU_BUTTON_IDX]);

            if (unitMenuON)
            {
                for (int i = 0; i <= U_BAR_IDX; i++)
                {
                    spriteBatch.Draw(itf_texture[i], itf_rectangle[i], itf_color[i]);
                }
                for (int i = ATTACK_ANT_BUTTON_IDX; i <= RUN_ANT_BUTTON_IDX; i++)
                    spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);
               
            }
            if(buildMenuON)
            {
                for (int i = 0; i <= B_BAR_IDX; i++)
                {
                    spriteBatch.Draw(itf_texture[i], itf_rectangle[i], itf_color[i]);
                }
                for (int i = B1_BUTTON_IDX; i <= B3_BUTTON_IDX; i++)
                    spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);
            }

            if (mainMenuOn)
            {
                spriteBatch.Draw(itf_texture[PAUSE_MENU_BACK], itf_rectangle[PAUSE_MENU_BACK], itf_color[PAUSE_MENU_BACK]);
                for (int i = RESUME_T_BUTTON_IDX; i < NUMBER_OF_BUTTONS; i++)
                    spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);
            }


            if (maxMenu)
            {
                spriteBatch.Draw(itf_texture[MIN_UNIT_BAR], itf_rectangle[MIN_UNIT_BAR], itf_color[MIN_UNIT_BAR]);
                spriteBatch.Draw(button_texture[UP_BUTTON_IDX], button_rectangle[UP_BUTTON_IDX], button_color[UP_BUTTON_IDX]);
            }
            else
            {
                spriteBatch.Draw(itf_texture[MAX_UNIT_BAR], itf_rectangle[MAX_UNIT_BAR], itf_color[MAX_UNIT_BAR]);
                spriteBatch.Draw(button_texture[DOWN_BUTTON_IDX], button_rectangle[DOWN_BUTTON_IDX], button_color[DOWN_BUTTON_IDX]);
            }

            spriteBatch.Draw(itf_texture[MINI_MAP], itf_rectangle[MINI_MAP], itf_color[MINI_MAP]);
            for (int i = SAVE_BUTTON_IDX; i <= PLAY_BUTTON_IDX; i++)
                spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);

        }
        // wrapper for hit_image_alpha taking Rectangle and Texture
        Boolean hit_image_alpha(Rectangle rect, Texture2D tex, int x, int y)
        {
            return hit_image_alpha(0, 0, tex, tex.Width * (x - rect.X) /
                rect.Width, tex.Height * (y - rect.Y) / rect.Height);
        }

        // wraps hit_image then determines if hit a transparent part of image 
        Boolean hit_image_alpha(float tx, float ty, Texture2D tex, int x, int y)
        {
            if (hit_image(tx, ty, tex, x, y))
            {
                uint[] data = new uint[tex.Width * tex.Height];
                tex.GetData<uint>(data);
                if ((x - (int)tx) + (y - (int)ty) *
                    tex.Width < tex.Width * tex.Height)
                {
                    return ((data[
                        (x - (int)tx) + (y - (int)ty) * tex.Width
                        ] &
                                0xFF000000) >> 24) > 20;
                }
            }
            return false;
        }

        // determine if x,y is within rectangle formed by texture located at tx,ty
        Boolean hit_image(float tx, float ty, Texture2D tex, int x, int y)
        {
            return (x >= tx &&
                x <= tx + tex.Width &&
                y >= ty &&
                y <= ty + tex.Height);
        }

        // determine state and color of button
        void update_buttons()
        {
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {

                if (hit_image_alpha(
                    button_rectangle[i], button_texture[i], mx, my))
                {
                    button_timer[i] = 0.0;
                    if (mpressed)
                    {
                        // mouse is currently down
                        button_state[i] = BState.DOWN;
                        button_color[i] = Color.Goldenrod;
                    }
                    else if (!mpressed && prev_mpressed)
                    {
                        // mouse was just released
                        if (button_state[i] == BState.DOWN)
                        {
                            // button i was just down
                            button_state[i] = BState.JUST_RELEASED;
                        }
                    }
                    else
                    {
                        button_state[i] = BState.HOVER;
                        button_color[i] = Color.Gold;
                    }
                }
                else
                {
                    button_state[i] = BState.UP;
                    if (button_timer[i] > 0)
                    {
                        button_timer[i] = button_timer[i] - frame_time;
                    }
                    else
                    {
                        button_color[i] = Color.White;
                    }
                }

                if (button_state[i] == BState.JUST_RELEASED)
                {
                    take_action_on_button(i);
                }
            }
        }


        // Logic for each button click goes here
        void take_action_on_button(int i)
        {
            //take action corresponding to which button was clicked
            switch (i)
            {
                case MENU_BUTTON_IDX:
                    mainMenuOn = true;
                    //zatrzymaj grê
                    //wyrysuj obrazek
                    //w³¹cz menu
                    break;
                   
                case ATTACK_ANT_BUTTON_IDX:
Console.WriteLine("attack!!@");                    break;
                case DEFENCE_ANT_BUTTON_IDX:
                    Console.WriteLine("attack!!@"); 
                    break;
                case RUN_ANT_BUTTON_IDX:
                    Console.WriteLine("run!!@"); 
                    break;
                  
                case B1_BUTTON_IDX:
                    if ((selectedModel.GetType()==typeof(BuildingPlace)))
                    ((BuildingPlace)selectedModel).Build1();
                    break;
                case B2_BUTTON_IDX:
                    if ((selectedModel.GetType()==typeof(BuildingPlace)))
                    ((BuildingPlace)selectedModel).BuildHyacyntFarm();


                    break;
                case B3_BUTTON_IDX:
                  if ((selectedModel.GetType()==typeof(BuildingPlace)))
                    ((BuildingPlace)selectedModel).BuildDicentraFarm();


                    break;

                case SAVE_BUTTON_IDX:
                    break;
                case PAUSE_BUTTON_IDX:
                    break;
                case PLAY_BUTTON_IDX:
                    break;

                case RESUME_T_BUTTON_IDX:
                    mainMenuOn = false;
                    break;
                case SAVE_T_BUTTON_IDX:
                    break;
                case LOAD_T_BUTTON_IDX:
                    break;
                case OPTIONS_T_BUTTON_IDX:
                    break;
                case EXIT_T_BUTTON_IDX:
                    //this.Exit();
                    break;

                case UP_BUTTON_IDX:
                    maxMenu = false;
                    break;
                case DOWN_BUTTON_IDX:
                    maxMenu = true;
                    break;

                default:
                    break;
            }
        }
    }
}
