namespace Ch06_Menus.GUI
{
    using System;
    using SFML.Graphics;
    using SFML.Window;

    internal class Button : Component
    {
        private Action callback;
        private Texture normalTexture;
        private Texture selectedTexture;
        private Texture pressedTexture;
        private Sprite sprite;
        private Text text;
        private bool isToggle;

        public Button(FontHolder fonts, TextureHolder textures)
        {
            normalTexture = textures.Get(Textures.ID.ButtonNormal);
            selectedTexture = textures.Get(Textures.ID.ButtonSelected);
            pressedTexture = textures.Get(Textures.ID.ButtonPressed);
            sprite = new Sprite();
            text = new Text(string.Empty, fonts.Get(Fonts.ID.Main), 16);

            sprite.Texture = normalTexture;
            var bounds = sprite.GetLocalBounds();
            text.Position = new Vector2f(bounds.Width / 2, bounds.Height / 2);
        }

        public void SetCallback(Action callback)
        {
            this.callback = callback;
        }

        public void SetText(string text)
        {
            this.text.DisplayedString = text;
            Utility.CenterOrigin(this.text);
        }

        public void SetToggle(bool flag)
        {
            isToggle = flag;
        }

        public override bool IsSelectable()
        {
            return true;
        }

        public override void Select()
        {
            base.Select();

            sprite.Texture = selectedTexture;
        }

        public override void Deselect()
        {
            base.Deselect();

            sprite.Texture = normalTexture;
        }

        public override void Activate()
        {
            base.Activate();

            // If we are toggle then we should show that the button is pressed and thus "toggled".
            if (isToggle)
            {
                sprite.Texture = pressedTexture;
            }

            if (callback != null)
            {
                callback();
            }

            // If we are not a toggle then deactivate the button since we are just momentarily activated.
            if (!isToggle)
            {
                Deactivate();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();

            if (isToggle)
            {
                // Reset texture to right one depending on if we are selected or not.
                if (IsSelected())
                {
                    sprite.Texture = selectedTexture;
                }
                else
                {
                    sprite.Texture = normalTexture;
                }
            }
        }

        public override void HandleKeyboardEvent(Keyboard.Key key, bool isPressed)
        {
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(sprite, states);
            target.Draw(text, states);
        }
    }
}
