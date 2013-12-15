#region File Description
//-----------------------------------------------------------------------------
// LoadingScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// Экроан загрузки координирует переходы между системой меню и игрой. 
    /// Обычно один экран исчезает, в это же время другой экран появляется, заменяя его.
    /// Большие преходы могут занимать больше времени для загрузки данных. 
    /// Перед тем как начнется загрузка игры все переходы должны быть завершены.
    /// 
    /// Это делается следующим образом:
    /// -Дать команду всем существующим эранам исчезнуть
    /// -Активировать экран загрузки, который будет появляться во время предидущего действия
    /// -Экран загрузки проверяет состояние предидущих экранов
    /// -Когда он видит, что они завершили свое исчезновение, он активирует следующий экран, которому для загрузки данных может потребоваться время.
    /// Экран загрузки будет единственным экраном, выводящимся на дисплей, пока происходит загрузка игры.
    /// 
    /// </summary>
    class LoadingScreen : GameScreen
    {
        #region Fields

        bool loadingIsSlow;
        bool otherScreensAreGone;

        GameScreen[] screensToLoad;

        #endregion

        #region Initialization


        /// <summary>
        /// Конструктор приватный: экран загрузки следует активировать через статичесский метод загрузки Load.
        /// </summary>
        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow,
                              GameScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Активация экрана загруззки.
        /// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
                                PlayerIndex? controllingPlayer,
                                params GameScreen[] screensToLoad)
        {
            //Даем команду текущим экранам исчезнуть.
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            //Создаем и активируем экран загрузки.
            LoadingScreen loadingScreen = new LoadingScreen(screenManager,
                                                            loadingIsSlow,
                                                            screensToLoad);

            screenManager.AddScreen(loadingScreen, controllingPlayer);
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Делаем update экрана загрузки.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //Если все предидущие экраны завершили свое изчезновение, пора выполнить загрузку. 
            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen, ControllingPlayer);
                    }
                }

                //После того как загрузка завершена, используем ResetElapsedTime чтобы сказать игровому механизму времени,
                //что мы только что завершили очень длинную конструкцию и ему не следует пытаться ее прервать/догнать.
                ScreenManager.Game.ResetElapsedTime();
            }
        }


        /// <summary>
        /// Рисуем экран загрузки.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //Проверяем завершились ли предидущие экраны. Это очень важно для того, чтобы все выглядело хорошо. 
            //Нам нужно обращяться к конструкции без них, прежде чем мы выполним загрузку.
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            //Экрану загрузки требуется время для выполнения, так что мы отобразим загрузку.
            //этот параметр говорит нам сколько времени займет загрузка.
            if (loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                const string message = "Loading...";

                //Выравниваем текст в окне отображения.
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (viewportSize - textSize) / 2;

                Color color = Color.White * TransitionAlpha;

                //Рисуем текст.
                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }


        #endregion
    }
}
