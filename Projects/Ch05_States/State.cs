namespace Ch05_States
{
    using System;
    using SFML.Graphics;
    using SFML.Window;

    internal abstract class State
    {
        private StateStack stack;
        private Context context;

        public State(StateStack stack, Context context)
        {
            this.stack = stack;
            this.context = context;
        }

        public abstract void Draw();
        public abstract bool Update(TimeSpan dt);
        public abstract bool HandleKeyboardInput(Keyboard.Key key, bool isPressed);

        protected void RequestStackPush(States.ID stateId)
        {
            stack.PushState(stateId);
        }

        protected void RequestStackPop()
        {
            stack.PopState();
        }

        protected void RequestStateClear()
        {
            stack.ClearStates();
        }

        protected Context GetContext()
        {
            return context;
        }

        public struct Context
        {
            public RenderWindow Window;
            public TextureHolder Textures;
            public FontHolder Fonts;
            public Player Player;

            public Context(RenderWindow window, TextureHolder textures, FontHolder fonts, Player player)
            {
                Window = window;
                Textures = textures;
                Fonts = fonts;
                Player = player;
            }
        }
    }
}
