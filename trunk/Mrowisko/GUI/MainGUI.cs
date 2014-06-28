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
        public List<Unit> selectedModels;
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
        const int NUMBER_OF_BUTTONS = 20,
            MENU_BUTTON_IDX = 0,
            SAVE_BUTTON_IDX = 1,
            PAUSE_BUTTON_IDX = 2,
            PLAY_BUTTON_IDX = 3,

            RESUME_T_BUTTON_IDX = 4,
            SAVE_T_BUTTON_IDX = 5,
            LOAD_T_BUTTON_IDX = 6,
            OPTIONS_T_BUTTON_IDX = 7,
            EXIT_T_BUTTON_IDX = 8,

            UNIT1_BUTTON_IDX = 9,
            UNIT2_BUTTON_IDX = 10,
            UNIT3_BUTTON_IDX = 11,
            UNIT4_BUTTON_IDX = 12,
            UNIT5_BUTTON_IDX = 13,

            B1_BUTTON_IDX = 14,
            B2_BUTTON_IDX = 15,
            B3_BUTTON_IDX = 16,

            ATTACK_ANT_BUTTON_IDX = 17,
            DEFENCE_ANT_BUTTON_IDX = 18,
            RUN_ANT_BUTTON_IDX = 19,

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
        bool unitSelected = true;
        bool mainMenuOn = false;


        const int NUMBER_OF_BARS = 6,
            MAIN_BAR_IDX = 0,
            PAUSE_MENU_BACK = 1,
            MINI_MAP = 2,
            FRIEND_SIGN = 3,
            NEUTRAL_SIGN = 4,
            ENEMY_SIGN = 5;

        Color[] bar_color = new Color[NUMBER_OF_BARS];
        Rectangle[] bar_rectangle = new Rectangle[NUMBER_OF_BARS];
        Texture2D[] bar_texture = new Texture2D[NUMBER_OF_BARS];

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
            bar_color[MAIN_BAR_IDX] = Color.White;
            bar_rectangle[MAIN_BAR_IDX] = new Rectangle(0, 0, 1366, 768);

            bar_color[PAUSE_MENU_BACK] = Color.White;
            bar_rectangle[PAUSE_MENU_BACK] = new Rectangle(1, 1, 1366, 768);

            bar_color[MINI_MAP] = Color.White;
            bar_rectangle[MINI_MAP] = new Rectangle(1043, 510, 312, 248);

            //=== === === BUTTONS
            // menu

            button_state[MENU_BUTTON_IDX] = BState.UP;
            button_color[MENU_BUTTON_IDX] = Color.White;
            button_timer[MENU_BUTTON_IDX] = 0.0;
            button_rectangle[MENU_BUTTON_IDX] = new Rectangle(10, 6, 69, 69);

            // save

            button_state[SAVE_BUTTON_IDX] = BState.UP;
            button_color[SAVE_BUTTON_IDX] = Color.White;
            button_timer[SAVE_BUTTON_IDX] = 0.0;
            button_rectangle[SAVE_BUTTON_IDX] = new Rectangle(90, 45, 29, 29);

            // pause

            button_state[PAUSE_BUTTON_IDX] = BState.UP;
            button_color[PAUSE_BUTTON_IDX] = Color.White;
            button_timer[PAUSE_BUTTON_IDX] = 0.0;
            button_rectangle[PAUSE_BUTTON_IDX] = new Rectangle(128, 6, 29, 29);

            // play

            button_state[PLAY_BUTTON_IDX] = BState.UP;
            button_color[PLAY_BUTTON_IDX] = Color.White;
            button_timer[PLAY_BUTTON_IDX] = 0.0;
            button_rectangle[PLAY_BUTTON_IDX] = new Rectangle(90, 6, 29, 29);

            //=== jednostki -------------------------------------------------

            button_state[UNIT1_BUTTON_IDX] = BState.UP;
            button_color[UNIT1_BUTTON_IDX] = Color.White;
            button_timer[UNIT1_BUTTON_IDX] = 0.0;
            button_rectangle[UNIT1_BUTTON_IDX] = new Rectangle(11, 691, 69, 69);

            button_state[UNIT2_BUTTON_IDX] = BState.UP;
            button_color[UNIT2_BUTTON_IDX] = Color.White;
            button_timer[UNIT2_BUTTON_IDX] = 0.0;
            button_rectangle[UNIT2_BUTTON_IDX] = new Rectangle(101, 691, 69, 69);

            button_state[UNIT3_BUTTON_IDX] = BState.UP;
            button_color[UNIT3_BUTTON_IDX] = Color.White;
            button_timer[UNIT3_BUTTON_IDX] = 0.0;
            button_rectangle[UNIT3_BUTTON_IDX] = new Rectangle(191, 691, 69, 69);

            button_state[UNIT4_BUTTON_IDX] = BState.UP;
            button_color[UNIT4_BUTTON_IDX] = Color.White;
            button_timer[UNIT4_BUTTON_IDX] = 0.0;
            button_rectangle[UNIT4_BUTTON_IDX] = new Rectangle(281, 691, 69, 69);

            button_state[UNIT5_BUTTON_IDX] = BState.UP;
            button_color[UNIT5_BUTTON_IDX] = Color.White;
            button_timer[UNIT5_BUTTON_IDX] = 0.0;
            button_rectangle[UNIT5_BUTTON_IDX] = new Rectangle(371, 691, 69, 69);



            //=== BUDYNKI -----------------------------------------------------------

            // B1

            button_state[B1_BUTTON_IDX] = BState.UP;
            button_color[B1_BUTTON_IDX] = Color.White;
            button_timer[B1_BUTTON_IDX] = 0.0;
            button_rectangle[B1_BUTTON_IDX] = new Rectangle(691, 756, 69, 69);

            // B2

            button_state[B2_BUTTON_IDX] = BState.UP;
            button_color[B2_BUTTON_IDX] = Color.White;
            button_timer[B2_BUTTON_IDX] = 0.0;
            button_rectangle[B1_BUTTON_IDX] = new Rectangle(691, 856, 69, 69);
            // B3

            button_state[B3_BUTTON_IDX] = BState.UP;
            button_color[B3_BUTTON_IDX] = Color.White;
            button_timer[B3_BUTTON_IDX] = 0.0;
            button_rectangle[B3_BUTTON_IDX] = new Rectangle(691, 956, 69, 69);
            //=== BUDYNKI -----------------------------------------------------------

            // defense

            button_state[DEFENCE_ANT_BUTTON_IDX] = BState.UP;
            button_color[DEFENCE_ANT_BUTTON_IDX] = Color.White;
            button_timer[DEFENCE_ANT_BUTTON_IDX] = 0.0;
            button_rectangle[DEFENCE_ANT_BUTTON_IDX] = new Rectangle(756, 691, 69, 69);

            // attack

            button_state[ATTACK_ANT_BUTTON_IDX] = BState.UP;
            button_color[ATTACK_ANT_BUTTON_IDX] = Color.White;
            button_timer[ATTACK_ANT_BUTTON_IDX] = 0.0;
            button_rectangle[ATTACK_ANT_BUTTON_IDX] = new Rectangle(846, 691, 69, 69);

            // RUN

            button_state[RUN_ANT_BUTTON_IDX] = BState.UP;
            button_color[RUN_ANT_BUTTON_IDX] = Color.White;
            button_timer[RUN_ANT_BUTTON_IDX] = 0.0;
            button_rectangle[RUN_ANT_BUTTON_IDX] = new Rectangle(936, 691, 69, 69);


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

        }
        public void LoadContent(ContentManager Content)
        {

            /// Interface -------------------------------------------------------------------------------

            button_texture[MENU_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/menuButton");

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

            button_texture[UNIT1_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/standardAntButton");
            button_texture[UNIT2_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/standardAntButton");
            button_texture[UNIT3_BUTTON_IDX] =
              Content.Load<Texture2D>(@"Textures/Button/standardAntButton");
            button_texture[UNIT4_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/standardAntButton");
            button_texture[UNIT5_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/standardAntButton");
            
            button_texture[B1_BUTTON_IDX] =
                Content.Load<Texture2D>(@"Textures/Button/b1Button");
            button_texture[B2_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/b2Button");
            button_texture[B3_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/b3Button");

            button_texture[ATTACK_ANT_BUTTON_IDX] =
                Content.Load<Texture2D>(@"Textures/Button/attackButton");
            button_texture[DEFENCE_ANT_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/defenceButton");
            button_texture[RUN_ANT_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/runOutButton");

            button_texture[SAVE_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/saveButton");
            button_texture[PAUSE_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/pauseButton");
            button_texture[PLAY_BUTTON_IDX] =
               Content.Load<Texture2D>(@"Textures/Button/playButton");




            //bars
            bar_texture[MAIN_BAR_IDX] =
                Content.Load<Texture2D>(@"Textures/Bars/bars_complex");
            bar_texture[PAUSE_MENU_BACK] =
                Content.Load<Texture2D>(@"Textures/Bars/PAUSE_MENU_BACK");
            bar_texture[MINI_MAP] =
                Content.Load<Texture2D>(@"Textures/Map_Content/MiniMap");
            bar_texture[FRIEND_SIGN] =
                Content.Load<Texture2D>(@"Textures/Map_Content/friendlySign");
            bar_texture[NEUTRAL_SIGN] =
               Content.Load<Texture2D>(@"Textures/Map_Content/neutralSign");
            bar_texture[ENEMY_SIGN] =
                Content.Load<Texture2D>(@"Textures/Map_Content/enemySign");
        }
        public void Update(GameTime gameTime)
        {

            ///interface --------------------------------------------------------------------------------
            if (control.selectedObject != null)
            {
                selectedModel = control.selectedObject;
                if (selectedModel.GetType().IsSubclassOf(typeof(Ant)))
                {
                    unitMenuON = true;
                    buildMenuON = false;
                }
                else if (selectedModel.GetType().IsSubclassOf(typeof(Building)))
                {
                    buildMenuON = true;
                    unitMenuON = false;

                }

            }
            if (control.SelectedModels.Count > 0)
            {
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

            update_buttons();  //--------------------------------------------------------------------------------------------------------------------------------- uncomment and fix

        }









        public void Draw(SpriteBatch spriteBatch)
        {
            // BAR           full main bar -------------
            spriteBatch.Draw(bar_texture[MAIN_BAR_IDX], bar_rectangle[MAIN_BAR_IDX], bar_color[MAIN_BAR_IDX]);
            //mini map
            spriteBatch.Draw(bar_texture[MINI_MAP], bar_rectangle[MINI_MAP], bar_color[MINI_MAP]);

            //BUTTONS         ------------------------
            //MainGUI menu button
            spriteBatch.Draw(button_texture[MENU_BUTTON_IDX], button_rectangle[MENU_BUTTON_IDX], button_color[MENU_BUTTON_IDX]);
            // pause , play, save menu
            for (int i = SAVE_BUTTON_IDX; i <= PLAY_BUTTON_IDX; i++)
                spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);


            //panel jednostek
            if (unitSelected)
            {
                spriteBatch.Draw(button_texture[UNIT1_BUTTON_IDX], button_rectangle[UNIT1_BUTTON_IDX], button_color[UNIT1_BUTTON_IDX]);
                spriteBatch.Draw(button_texture[UNIT2_BUTTON_IDX], button_rectangle[UNIT2_BUTTON_IDX], button_color[UNIT2_BUTTON_IDX]);
                spriteBatch.Draw(button_texture[UNIT3_BUTTON_IDX], button_rectangle[UNIT3_BUTTON_IDX], button_color[UNIT3_BUTTON_IDX]);
                spriteBatch.Draw(button_texture[UNIT4_BUTTON_IDX], button_rectangle[UNIT4_BUTTON_IDX], button_color[UNIT4_BUTTON_IDX]);
                spriteBatch.Draw(button_texture[UNIT5_BUTTON_IDX], button_rectangle[UNIT5_BUTTON_IDX], button_color[UNIT5_BUTTON_IDX]);
            }

            //panel budynków
            if (buildMenuON)
            {
                spriteBatch.Draw(button_texture[B1_BUTTON_IDX], button_rectangle[B1_BUTTON_IDX], button_color[B1_BUTTON_IDX]);
                spriteBatch.Draw(button_texture[B2_BUTTON_IDX], button_rectangle[B2_BUTTON_IDX], button_color[B2_BUTTON_IDX]);
                spriteBatch.Draw(button_texture[B3_BUTTON_IDX], button_rectangle[B3_BUTTON_IDX], button_color[B3_BUTTON_IDX]);
            }
            else //panel ob³ugi jednostek
            {
                spriteBatch.Draw(button_texture[ATTACK_ANT_BUTTON_IDX], button_rectangle[ATTACK_ANT_BUTTON_IDX], button_color[ATTACK_ANT_BUTTON_IDX]);
                spriteBatch.Draw(button_texture[DEFENCE_ANT_BUTTON_IDX], button_rectangle[DEFENCE_ANT_BUTTON_IDX], button_color[DEFENCE_ANT_BUTTON_IDX]);
                spriteBatch.Draw(button_texture[RUN_ANT_BUTTON_IDX], button_rectangle[RUN_ANT_BUTTON_IDX], button_color[RUN_ANT_BUTTON_IDX]);
            }




            //Pause Menu ----------
            if (mainMenuOn)
            {
                spriteBatch.Draw(bar_texture[PAUSE_MENU_BACK], bar_rectangle[PAUSE_MENU_BACK], bar_color[PAUSE_MENU_BACK]);
                for (int i = RESUME_T_BUTTON_IDX; i <= EXIT_T_BUTTON_IDX; i++)
                    spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);
            }
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
                        button_color[i] = Color.Brown;
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
                        button_color[i] = Color.RosyBrown;
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
                    Console.WriteLine("attack!!@");
                    break;
                case DEFENCE_ANT_BUTTON_IDX:
                    Console.WriteLine("attack!!@");
                    break;
                case RUN_ANT_BUTTON_IDX:
                    Console.WriteLine("run!!@");
                    break;

                case B1_BUTTON_IDX:
                    if ((selectedModel.GetType() == typeof(BuildingPlace)))
                        ((BuildingPlace)selectedModel).Build1();
                    break;
                case B2_BUTTON_IDX:
                    //if ((selectedModel.GetType() == typeof(BuildingPlace)))
                     //   ((BuildingPlace)selectedModel).BuildHyacyntFarm();
                    break;
                case B3_BUTTON_IDX:
                    if ((selectedModel.GetType() == typeof(BuildingPlace)))
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
                default:
                    break;
            }
        }
    }
}
