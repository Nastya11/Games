﻿#region File Description
//-----------------------------------------------------------------------------
// InputState.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// Вспомогательный класс для чтения состояния с геймпада или тачскрина, 
    /// записыввающий предыдущее и настоящее состояние обоих устройств, а так же осуществляет и
    /// запрашивает методы для высокоуровневых действий, таких как "переместиться вверх по меню"
    /// или "поставить игру на паузу".
    /// </summary>
    public class InputState
    {
        #region Fields

        public const int MaxInputs = 4;

        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;

        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

        public TouchCollection TouchState;

        public readonly List<GestureSample> Gestures = new List<GestureSample>();

        #endregion

        #region Initialization


        /// <summary>
        /// Конструкция ново-введённого значения
        /// </summary>
        public InputState()
        {
            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];

            LastKeyboardStates = new KeyboardState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];//Проверка: есть ли геймпад
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Считывает последнее состояние клавиатуры и  геймпада
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                LastKeyboardStates[i] = CurrentKeyboardStates[i];
                LastGamePadStates[i] = CurrentGamePadStates[i];

                CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                // Проверяет, был ли геймпад присоединён
                // или мы можем утверждать, что его нет
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }

            TouchState = TouchPanel.GetState(); //Примечание 1: Работа с тачпадом

            Gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                Gestures.Add(TouchPanel.ReadGesture());
            }
        }


        /// <summary>
        /// Помощник для поверки нажатия клавиши, при этом обновлении. Праметр
        /// управления Игроком(controllingPlayer), указывает: состояние какого игрока считывать.
        /// При значении этого пареметра null данные принимаютя от любого игрока. Когда замечено
        /// нажатие клавиши, выводящая конструкция playerIndex возвращает значение игрока, нажавшего на клавишу.
        ///Примечание 2: Для мультиплеера
        /// </summary>
        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer,
                                            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Считывает ввод с определённого игрока.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentKeyboardStates[i].IsKeyDown(key) &&
                        LastKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                // Принимает ввод с любого игрока.
                return (IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Four, out playerIndex));
            }
        }


        /// <summary>
        /// Помощник для поверки нажатия клавиши, при этом обновлении. Праметр
        /// управления Игроком(controllingPlayer), указывает: состояние какого игрока считывать.
        /// При значении этого пареметра null данные принимаютя от любого игрока. Когда замечено
        /// нажатие клавиши, выводящая конструкция playerIndex возвращает значение игрока, нажавшего на клавишу.
        ///Примечание 2: Для мультиплеера
        /// </summary>
        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Считывает ввод с определённого игрока.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button) &&
                        LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                // Принимает ввод с любого игрока.
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }


        /// <summary>
        /// Проверят введение события "выбор меню".Праметр
        /// управления Игроком(controllingPlayer), указывает: состояние какого игрока считывать
        /// При значении этого пареметра null данные принимаютя от любого игрока. Когда замечено
        /// нажатие клавиши, выводящая конструкция playerIndex возвращает значение игрока, нажавшего на клавишу..
        /// </summary>
        public bool IsMenuSelect(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
                   IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Проверят введение события "закрытие меню".Праметр
        /// управления Игроком(controllingPlayer), указывает: состояние какого игрока считывать
        /// При значении этого пареметра null данные принимаютя от любого игрока. Когда замечено
        /// нажатие клавиши, выводящая конструкция playerIndex возвращает значение игрока, нажавшего на клавишу..
        /// </summary>
        public bool IsMenuCancel(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Проверят введение события "меню вверх".Праметр
        /// управления Игроком(controllingPlayer), указывает: состояние какого игрока считывать
        /// При значении этого пареметра null данные принимаютя от любого игрока.
        /// </summary>
        public bool IsMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Проверят введение события "меню вниз".Праметр
        /// управления Игроком(controllingPlayer), указывает: состояние какого игрока считывать
        /// При значении этого пареметра null данные принимаютя от любого игрока.
        /// </summary>
        public bool IsMenuDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Проверят введение события "пауза меню меню".Праметр
        /// управления Игроком(controllingPlayer), указывает: состояние какого игрока считывать
        /// При значении этого пареметра null данные принимаютя от любого игрока.
        /// </summary>
        public bool IsPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }


        #endregion
    }
}
