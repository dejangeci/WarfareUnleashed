namespace Ch06_Menus
{
    using System;
    using System.Collections.Generic;
    using Ch06_Menus.GUI;
    using SFML.Graphics;
    using SFML.Window;

    internal class MenuState : State
    {
        private Sprite backgroundSprite;
        private Container guiContainer;

        public MenuState(StateStack stack, Context context)
            : base(stack, context)
        {
            backgroundSprite = new Sprite();
            guiContainer = new Container();

            var texture = context.Textures.Get(Textures.ID.TitleScreen);
            backgroundSprite.Texture = texture;

            var playButton = new Button(context.Fonts, context.Textures);
            playButton.Position = new Vector2f(100, 250);
            playButton.SetText("Play");
            playButton.SetCallback(() =>
            {
                RequestStackPop();
                RequestStackPush(States.ID.Game);
            });

            var settingsButton = new Button(context.Fonts, context.Textures);
            settingsButton.Position = new Vector2f(100, 300);
            settingsButton.SetText("Settings");
            settingsButton.SetCallback(() =>
            {
                RequestStackPush(States.ID.Settings);
            });

            var exitButton = new Button(context.Fonts, context.Textures);
            exitButton.Position = new Vector2f(100, 350);
            exitButton.SetText("Exit");
            exitButton.SetCallback(() =>
            {
                RequestStackPop();
            });

            guiContainer.Pack(playButton);
            guiContainer.Pack(settingsButton);
            guiContainer.Pack(exitButton);
        }

        public override void Draw()
        {
            var window = GetContext().Window;

            window.SetView(window.DefaultView);

            window.Draw(backgroundSprite);
            window.Draw(guiContainer);
        }

        public override bool Update(TimeSpan dt)
        {
            return true;
        }

        public override bool HandleKeyboardInput(Keyboard.Key key, bool isPressed)
        {
            guiContainer.HandleKeyboardEvent(key, isPressed);

            return false;
        }
    }
}
