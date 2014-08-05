namespace Ch06_Menus.GUI
{
    using SFML.Graphics;
    using SFML.Window;

    internal class Label : Component
    {
        private Text text;

        public Label(string text, FontHolder fonts)
        {
            this.text = new Text(text, fonts.Get(Fonts.ID.Main), 16);
        }

        public override bool IsSelectable()
        {
            return false;
        }

        public override void HandleKeyboardEvent(Keyboard.Key key, bool isPressed)
        {
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(text, states);
        }

        public void SetText(string text)
        {
            this.text.DisplayedString = text;
        }
    }
}
