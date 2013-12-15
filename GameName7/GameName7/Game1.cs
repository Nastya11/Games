#region File Description
//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// Это пример, который показывает как управлять состояниями игры
    /// и переключаться между экранами игры, такими как: меню загрузки, самой игра,
    /// меню паузы и.т.д. Основной класс крайне прост, всё самое интересное происходит в
    /// ScreenManager component.
    /// </summary>
    public class GameStateManagementGame : Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

#if ZUNE
        int BufferWidth = 272;
        int BufferHeight = 480;
#elif IPHONE
        int BufferWidth = 320;
        int BufferHeight = 480;
#else
        int BufferWidth = 272;
        int BufferHeight = 480;
#endif
        #endregion

        #region Initialization


        /// <summary>
        /// Основной конструктор игры
        /// </summary>
        public GameStateManagementGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = BufferWidth;
            graphics.PreferredBackBufferHeight = BufferHeight;

            // Создаётся screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Активируются начальные экраны.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }


        #endregion

        #region Draw


        /// <summary>
        /// Вызывается, когда игре необходимо прорисовать саму себя
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // Настоящая прорисовка происходит внутри screen manager
            base.Draw(gameTime);
        }


        #endregion
    }
}
