using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SummonersTale.Forms;
using SummonersTale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Controls
{
    public class RightLeftSelector : Control
    {
        #region Event Region

        public event EventHandler SelectionChanged;

        #endregion

        #region Field Region

        private readonly List<string> _items = new();

        private readonly Texture2D _leftTexture;
        private readonly Texture2D _rightTexture;

        private Color _selectedColor = Color.Red;
        private int _maxItemWidth;
        private int _selectedItem;
        private Rectangle _leftSide = new();
        private Rectangle _rightSide = new();
        private int _yOffset;

        #endregion

        #region Property Region

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; }
        }

        public int SelectedIndex
        {
            get { return _selectedItem; }
            set { _selectedItem = (int)MathHelper.Clamp(value, 0f, _items.Count); }
        }

        public string SelectedItem
        {
            get { return Items[_selectedItem]; }
        }

        public List<string> Items
        {
            get { return _items; }
        }

        public int MaxItemWidth
        {
            get { return _maxItemWidth; }
            set { _maxItemWidth = value; }
        }

        #endregion

        #region Constructor Region

        public RightLeftSelector(Texture2D leftArrow, Texture2D rightArrow)
        {
            _leftTexture = leftArrow;
            _rightTexture = rightArrow;
            TabStop = true;
            Color = Color.White;
        }

        #endregion

        #region Method Region

        public void SetItems(string[] items, int maxWidth)
        {
            this._items.Clear();

            foreach (string s in items)
                this._items.Add(s);

            _maxItemWidth = maxWidth;
        }

        protected void OnSelectionChanged()
        {
            SelectionChanged?.Invoke(this, null);
        }

        #endregion

        #region Abstract Method Region

        public override void Update(GameTime gameTime)
        {
            HandleMouseInput();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawTo = _position;

            _spriteFont = ControlManager.SpriteFont;

            _yOffset = (int)((Size.Y - _spriteFont.MeasureString("W").Y) / 2);
            _leftSide = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_size.X,
                (int)_size.Y);

            //if (selectedItem != 0)
            spriteBatch.Draw(_leftTexture, _leftSide, Color.White);
            //else
            //    spriteBatch.Draw(stopTexture, drawTo, Color.White);

            drawTo.X += Size.X + 5f;

            float itemWidth = _spriteFont.MeasureString(_items[_selectedItem]).X;
            float offset = (_maxItemWidth - Size.X) / 2;

            Vector2 off = new(offset, _yOffset);

            if (_hasFocus)
                spriteBatch.DrawString(_spriteFont, _items[_selectedItem], drawTo + off, _selectedColor);
            else
                spriteBatch.DrawString(_spriteFont, _items[_selectedItem], drawTo + off, Color);

            drawTo.X += _maxItemWidth + 5f;

            _rightSide = new Rectangle(
                (int)drawTo.X, 
                (int)drawTo.Y,
                (int)_size.X,
                (int)_size.Y);

            //if (selectedItem != items.Count - 1)
            spriteBatch.Draw(_rightTexture, _rightSide, Color.White);
            //else
            //    spriteBatch.Draw(stopTexture, drawTo, Color.White);
        }

        public override void HandleInput()
        {
            if (_items.Count == 0)
                return;

            if (Xin.WasKeyReleased(Keys.Left))
            {
                _selectedItem--;
                if (_selectedItem < 0)
                    _selectedItem = this.Items.Count - 1;
                OnSelectionChanged();
            }

            if (Xin.WasKeyReleased(Keys.Right))
            {
                _selectedItem++;
                if (_selectedItem >= _items.Count)
                    _selectedItem = 0;
                OnSelectionChanged();
            }
        }

        private void HandleMouseInput()
        {
            if (Xin.WasMouseReleased(MouseButton.Left))
            {
                Point mouse = Xin.MouseAsPoint;

                if (_leftSide.Scale(Settings.Scale).Contains(mouse))
                {
                    _selectedItem--;
                    if (_selectedItem < 0)
                        _selectedItem = this.Items.Count - 1;
                    OnSelectionChanged();
                }

                if (_rightSide.Scale(Settings.Scale).Contains(mouse))
                {
                    _selectedItem++;
                    if (_selectedItem >= _items.Count)
                        _selectedItem = 0;
                    OnSelectionChanged();
                }
            }

            if (Xin.TouchReleased())
            {
                if (_leftSide.Scale(Settings.Scale).Contains(Xin.TouchLocation))
                {
                    _selectedItem--;
                    if (_selectedItem < 0)
                        _selectedItem = this.Items.Count - 1;
                    OnSelectionChanged();
                }

                if (_rightSide.Scale(Settings.Scale).Contains(Xin.TouchLocation))
                {
                    _selectedItem++;
                    if (_selectedItem >= _items.Count)
                        _selectedItem = 0;
                    OnSelectionChanged();
                }
            }
        }

        #endregion
    }
}
