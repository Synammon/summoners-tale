﻿using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework.Input.Touch;

namespace SummonersTale
{
    public enum MouseButton { Left, Right };

    public class Xin : GameComponent
    {
        private static KeyboardState keyboardState;
        private static KeyboardState lastKeyboardState;
        private static MouseState mouseState;
        private static TouchCollection lastTouchLocations;
        private static MouseState lastMouseState;
        private static TouchCollection touchLocations;

        public static KeyboardState KeyboardState { get { return keyboardState; } }
        public static MouseState MouseState { get { return mouseState; } }

        public static KeyboardState LastKeyboardState { get { return lastKeyboardState; } }
        public static MouseState LastMouseState { get { return lastMouseState; } }

        public static Point MouseAsPoint
        {
            get { return new Point(MouseState.X, MouseState.Y); }
        }

        public static Point LastMouseAsPoint
        {
            get { return new Point(LastMouseState.X, LastMouseState.Y); }
        }

        public Xin(Game game) : base(game)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            lastTouchLocations = touchLocations;
            touchLocations = TouchPanel.GetState();

            base.Update(gameTime);
        }

        public static bool IsKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static bool WasKeyDown(Keys key)
        {
            return lastKeyboardState.IsKeyDown(key);
        }

        public static bool WasKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        public static bool WasKeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
        }

        public static bool IsMouseDown(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => mouseState.LeftButton == ButtonState.Pressed,
                MouseButton.Right => mouseState.RightButton == ButtonState.Pressed,
                _ => false,
            };
        }

        public static bool WasMouseDown(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => lastMouseState.LeftButton == ButtonState.Pressed,
                MouseButton.Right => lastMouseState.RightButton == ButtonState.Pressed,
                _ => false,
            };
        }

        public static bool WasMousePressed(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released,
                MouseButton.Right => mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released,
                _ => false,
            };
        }

        public static bool WasMouseReleased(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed,
                MouseButton.Right => mouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed,
                _ => false,
            };
        }

        public static List<Keys> KeysPressed()
        {
            List<Keys> keys = new();

            Keys[] current = keyboardState.GetPressedKeys();
            Keys[] last = lastKeyboardState.GetPressedKeys();

            foreach (Keys key in current)
            {
                if (!last.Contains(key))
                {
                    keys.Add(key);
                }
            }

            return keys;
        }

        public static List<Keys> KeysReleased()
        {
            List<Keys> keys = new();

            Keys[] current = keyboardState.GetPressedKeys();
            Keys[] last = lastKeyboardState.GetPressedKeys();

            foreach (Keys key in current)
            {
                if (last.Contains(key))
                {
                    keys.Add(key);
                }
            }

            return keys;
        }

        public static TouchCollection TouchPanelState
        {
            get { return touchLocations; }
        }

        public static TouchCollection LastTouchPanelState
        {
            get { return lastTouchLocations; }
        }

        public static bool TouchReleased()
        {
            TouchCollection tc = touchLocations;

            if (tc.Count > 0 &&
                tc[0].State == TouchLocationState.Released)
            {
                return true;
            }

            return false;
        }

        public static bool TouchPressed()
        {
            return (touchLocations.Count > 0 &&
                (touchLocations[0].State == TouchLocationState.Pressed));
        }

        public static bool TouchMoved()
        {
            return (touchLocations.Count > 0 &&
                (touchLocations[0].State == TouchLocationState.Moved));
        }

        public static Vector2 TouchLocation
        {
            get
            {
                Vector2 result = Vector2.Zero - Vector2.One;

                if (touchLocations.Count > 0)
                {
                    if (touchLocations[0].State == TouchLocationState.Pressed ||
                        touchLocations[0].State == TouchLocationState.Moved)
                    {
                        result = touchLocations[0].Position;
                    }
                }

                return result;
            }
        }

        public static Vector2 TouchReleasedAt
        {
            get
            {
                Vector2 result = Vector2.Zero;

                if (touchLocations.Count > 0)
                {
                    if (touchLocations[0].State == TouchLocationState.Released)
                    {
                        result = touchLocations[0].Position;
                    }
                }

                return result;
            }
        }

        public static bool WasKeyPressed()
        {
            return
                keyboardState.GetPressedKeyCount() > 0 &&
                lastKeyboardState.GetPressedKeyCount() == 0;
        }
    }
}
