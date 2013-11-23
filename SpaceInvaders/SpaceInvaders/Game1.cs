#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

#endregion


namespace SpaceInvaders
{

    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;
        int a = 0;
        Texture2D gameBackground;
        Rectangle laserPlace;
        Ship player = new Ship();
        List<Asteroid> asteroids = new List<Asteroid>();
        List<Laser> lasers = new List<Laser>();
        uint lives = 3;
        private Texture2D lazerPic;
        Rectangle asteroidPlace;
        Rectangle lifeplace = new Rectangle(750, 450, 50, 30);
        Texture2D lifePicture1;
        Texture2D lifePicture2;
        Texture2D lifePicture3;
        Texture2D lifePicture4;
        private bool isPlaying;
        public Game1()
            : base()
        {
            try
            {
                graphics = new GraphicsDeviceManager(this);
            }
            catch { };

            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.picture = Content.Load<Texture2D>("SpaceShip");
            gameBackground = Content.Load<Texture2D>("012_18");
            player.placeHolder = player.picture.Bounds;
            lifePicture1 = Content.Load<Texture2D>("lives3.png");

            lifePicture2 = Content.Load<Texture2D>("lives2.png");
            lifePicture3 = Content.Load<Texture2D>("life.png");
            lifePicture4 = Content.Load<Texture2D>("lives0.png");
            lazerPic = Content.Load<Texture2D>("Laser.png");

        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
            if(lives > 0)
            {
            ParseKeys();
            if (player.placeHolder.X == 0)
            {
                player.placeHolder.X = player.placeHolder.X + 50;

            }
            else if (player.placeHolder.X == 650)
            {
                player.placeHolder.X = player.placeHolder.X - 50;
            }
            else if (player.placeHolder.Y == 0)
            {
                player.placeHolder.Y = player.placeHolder.Y + 50;
            }
            else if (player.placeHolder.Y == 430)
            {
                player.placeHolder.Y = player.placeHolder.Y - 50;
            }
            if (a != 0)
            {
                a = a - 10;
            }
            Random rnd = new Random();
            CreateAsteroids(rnd);

            for (int i = 0; i < lasers.Count; i++)
            {
                Laser newLaser = lasers[i];

                newLaser.distance = 25;
                newLaser.placeHolder.X = newLaser.placeHolder.X + newLaser.distance;
                laserPlace = newLaser.placeHolder;
                if (laserPlace.Intersects(asteroidPlace))
                {
                    lasers.Remove(newLaser);
                }
                if (laserPlace.X == 750)
                {
                    lasers.Remove(newLaser);
                }
            }
            for (int i = 0; i < asteroids.Count; i++)
            {
                Asteroid newAst = asteroids[i];
                newAst.distance = 10;
                newAst.placeHolder.X = newAst.placeHolder.X - newAst.distance;
                asteroidPlace = newAst.placeHolder;
                if (asteroidPlace.X == 0)
                {
                    asteroids.Remove(newAst);
                }
                if (asteroidPlace.Intersects(player.placeHolder))
                {
                    lives--;
                        asteroids.Remove(newAst);
                    
                }
                if (asteroidPlace.Intersects(laserPlace))
                {
                       asteroids.Remove(newAst);
                  
                }

            }

        }
        }

        private void CreateAsteroids(Random rnd)
        {

            if (rnd.Next(0, 50) == 3)
            {

                var newAst = new Asteroid();
                newAst.picture = Content.Load<Texture2D>("Asteroide");
                newAst.placeHolder = newAst.picture.Bounds;
                newAst.placeHolder.X = 750;
                newAst.placeHolder.Y = rnd.Next(0, 500);
                asteroids.Add(newAst);


            }

        }

        private void ParseKeys()
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left))
                player.placeHolder.X -= 1;

            if (state.IsKeyDown(Keys.Right))
                player.placeHolder.X += 1;

            if (state.IsKeyDown(Keys.Up))
                player.placeHolder.Y -= 1;

            if (state.IsKeyDown(Keys.Down))
                player.placeHolder.Y += 1;


            if (state.IsKeyDown(Keys.Space))
            {
                if (a == 0)
                {
                    a = 300;
                    var newLaser = new Laser();
                    newLaser.picture = lazerPic;
                    newLaser.placeHolder = newLaser.picture.Bounds;
                    newLaser.placeHolder.X = player.placeHolder.Right;
                    newLaser.placeHolder.Y = player.placeHolder.Y + player.placeHolder.Height / 2;
                    lasers.Add(newLaser);
                }


            }
        }


        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);




            spriteBatch.Begin();
            spriteBatch.Draw(gameBackground, gameBackground.Bounds, Color.White);
            spriteBatch.Draw(player.picture, player.placeHolder, Color.White);
            foreach (var ast in asteroids)
                spriteBatch.Draw(ast.picture, ast.placeHolder, Color.White);
            foreach (var las in lasers)
                spriteBatch.Draw(las.picture, las.placeHolder, Color.White);
            if (lives == 3)
            {
                spriteBatch.Draw(lifePicture1, lifeplace, Color.White);
            }
            else if (lives == 2)
            {
                spriteBatch.Draw(lifePicture2, lifeplace, Color.White);
            }
            else if (lives == 1)
            {
                spriteBatch.Draw(lifePicture3, lifeplace, Color.White);
            }
            else if (lives == 0)
            {
                spriteBatch.Draw(lifePicture4, lifeplace, Color.White);
            }
            spriteBatch.End();
        }
    }
}
    

        
    
