using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.Forms
{
    public class ControlManager : List<Control>
    {
        #region Fields and Properties

        int _selectedControl = -1;
        bool _acceptInput = true;

        static SpriteFont _spriteFont;

        public static SpriteFont SpriteFont
        {
            get { return _spriteFont; }
        }

        public bool AcceptInput
        {
            get { return _acceptInput; }
            set { _acceptInput = value; }
        }

        #endregion

        #region Event Region

        public event EventHandler FocusChanged;

        #endregion

        #region Constructors

        private ControlManager()
            : base()
        {
        }

        public ControlManager(SpriteFont spriteFont, int capacity)
            : base(capacity)
        {
            ControlManager._spriteFont = spriteFont;
        }

        public ControlManager(SpriteFont spriteFont, IEnumerable<Control> collection) :
            base(collection)
        {
            ControlManager._spriteFont = spriteFont;
        }

        #endregion

        #region Methods

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Control control in this)
            {
                if (control != null)
                {
                    spriteBatch.Begin();
                    control.Draw(spriteBatch);
                    spriteBatch.End();
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Count == 0)
            {
                return;
            }

            foreach (Control c in this)
            {
                if (c.Enabled)
                {
                    c.Update(gameTime);
                }
            }

            foreach (Control c in this)
            {
                if (c.HasFocus)
                {
                    c.HandleInput();
                    break;
                }
            }

            foreach (Control c in this)
            {
                if (!Xin.WasMouseReleased(MouseButton.Left)) continue;

                Rectangle location = new(
                    new((int)c.Position.X, (int)c.Position.Y),
                    new((int)c.Size.X, (int)c.Size.Y));

                Point mouse = Xin.MouseAsPoint;

                if (!location.Contains(mouse)) continue;

                foreach (Control d in this)
                {
                    d.HasFocus = false;
                }

                c.HasFocus = true;
            }

            if (!AcceptInput)
            {
                return;
            }

            if (Xin.WasKeyReleased(Keys.Tab) && (Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
            {
                PreviousControl();
            }

            if (Xin.WasKeyReleased(Keys.Tab) && !(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
            {
                NextControl();
            }
        }



        public void NextControl()
        {
            if (Count == 0)
            {
                return;
            }

            int currentControl = _selectedControl;

            this[_selectedControl].HasFocus = false;

            do
            {
                _selectedControl++;

                if (_selectedControl == Count)
                {
                    _selectedControl = 0;
                }

                if (this[_selectedControl].TabStop && this[_selectedControl].Enabled)
                {
                    FocusChanged?.Invoke(this[_selectedControl], null);

                    break;
                }

            } while (currentControl != _selectedControl);

            this[_selectedControl].HasFocus = true;
        }

        public void PreviousControl()
        {
            if (Count == 0)
            {
                return;
            }

            int currentControl = _selectedControl;

            this[_selectedControl].HasFocus = false;

            do
            {
                _selectedControl--;

                if (_selectedControl < 0)
                {
                    _selectedControl = Count - 1;
                }

                if (this[_selectedControl].TabStop && this[_selectedControl].Enabled)
                {
                    FocusChanged?.Invoke(this[_selectedControl], null);

                    break;
                }
            } while (currentControl != _selectedControl);

            this[_selectedControl].HasFocus = true;
        }

        #endregion
    }
}
