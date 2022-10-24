using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PianoTiles
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputManager iManager = new InputManager();

        private Vector2 screen = new Vector2(720, 1280);

        bool is_game = true;

        private int rows;
        private int cols;
        private Random rand;
        private SpriteFont font;
        private Texture2D texture;
        private int[,] tiles;
        private Color[] colors;
        private Color color0;
        private Color color1;

        int score = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = (int)screen.X;
            _graphics.PreferredBackBufferHeight = (int)screen.Y;
        }

        protected override void Initialize()
        {
            iManager.Initialize();
            iManager.KeyPressed += OnKeyPressed;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            rows = 5;
            cols = 4;
            rand = new Random(0);
            font = Content.Load<SpriteFont>("Font");
            texture = new Texture2D(GraphicsDevice, cols, rows);
            tiles = new int[rows, cols];
            colors = new Color[rows * cols];
            color0 = Color.AntiqueWhite;
            color1 = Color.DarkOliveGreen;

            Randomize();
            Set();
        }

        private void Randomize()
        {
            int rand_num;

            for (int r = 0; r < rows; r++)
            {
                rand_num = rand.Next(cols);
                for (int c = 0; c < cols; c++)
                {
                    if (rand_num == c) tiles[r, c] = 1;
                    else tiles[r, c] = 0;
                }
            }
        }

        private void Set()
        {
            int iter = 0;
            foreach (int i in tiles)
            {
                if (i == 0) colors[iter] = color0;
                else if (i == 1) colors[iter] = color1;
                iter++;
            }

            texture.SetData(colors);
        }

        private void Reset()
        {
            score = 0;
            is_game = true;
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            iManager.Update();

            if (is_game == false)
            {
                if(iManager._keystate.IsKeyDown(Keys.Enter)) Reset();
            }

            base.Update(gameTime);
        }

        private void OnKeyPressed(object sender, InputData i)
        {
            if (tiles[rows - 1, i.Index] == 1) Score();
            else Lose();
        }

        private void Score()
        {
            score++;
            Scroll();
        }

        private void Lose()
        {
            Set();
            is_game = false;
        }

        private void Scroll()
        {
            if (is_game == false) return;

            for (int r = 1; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    tiles[rows - r, c] = tiles[rows - r - 1, c];
                }
            }

            for (int c = 0; c < cols; c++)
            {
                tiles[0, c] = 0;
            }

            tiles[0, rand.Next(cols - 1)] = 1;

            Set();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Red);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(texture, new Rectangle(0, 0, (int)screen.X, (int)screen.Y), Color.White);

            _spriteBatch.DrawString(font, score.ToString(), new Vector2(screen.X / 2, 30), new Color(Color.Red, 255 / 1.2f));

            if (is_game == false)
            {
                _spriteBatch.DrawString(font, "GAMEOVER \n 'enter' TO RESET", new Vector2(50,screen.Y / 3), Color.Red);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
