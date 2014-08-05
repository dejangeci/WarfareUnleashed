namespace Ch06_Menus
{
    using System.Collections.Generic;
    using Ch06_Menus.GUI;
    using SFML.Graphics;
    using SFML.Window;

    internal class SettingsState : State
    {
        private Sprite backgroundSprite;
        private Container guiContainer;
        private Button[] bindingButtons;
        private Label[] bindingLabels;

        public SettingsState(StateStack stack, Context context)
            : base(stack, context)
        {
            backgroundSprite = new Sprite();
            guiContainer = new Container();
            bindingButtons = new Button[(int)Player.Action.ActionCount];
            bindingLabels = new Label[(int)Player.Action.ActionCount];

            backgroundSprite.Texture = context.Textures.Get(Textures.ID.TitleScreen);

            // Build key binding buttons and labels
            AddButtonLabel(Player.Action.MoveLeft, 150, "Move Left", context);
            AddButtonLabel(Player.Action.MoveRight, 200, "Move Right", context);
            AddButtonLabel(Player.Action.MoveUp, 250, "Move Up", context);
            AddButtonLabel(Player.Action.MoveDown, 300, "Move Down", context);

            UpdateLabels();

            var backButton = new Button(context.Fonts, context.Textures);
            backButton.Position = new Vector2f(80, 375);
            backButton.SetText("Back");
            backButton.SetCallback(() =>
            {
                RequestStackPop();
            });

            guiContainer.Pack(backButton);
        }

        public override void Draw()
        {
            var window = GetContext().Window;

            window.Draw(backgroundSprite);
            window.Draw(guiContainer);
        }

        public override bool Update(System.TimeSpan dt)
        {
            return true;
        }

        public override bool HandleKeyboardInput(SFML.Window.Keyboard.Key key, bool isPressed)
        {
            bool isKeyBinding = false;

            // Iterate through all key binding buttons to see if they are being pressed, waiting for the user to enter a key
            for (int action = 0; action < (int)Player.Action.ActionCount; ++action)
            {
                if (bindingButtons[action].IsActive())
                {
                    isKeyBinding = true;
                    if (!isPressed)
                    {
                        GetContext().Player.AssignKey((Player.Action)action, key);
                        bindingButtons[action].Deactivate();
                    }
                    break;
                }
            }

            // If pressed button changed key bindings, update labels; otherwise consider other buttons in container
            if (isKeyBinding)
            {
                UpdateLabels();
            }
            else
            {
                guiContainer.HandleKeyboardEvent(key, isPressed);
            }

            return false;
        }

        private void UpdateLabels()
        {
            var player = GetContext().Player;

            for (int i = 0; i < (int)Player.Action.ActionCount; ++i)
            {
                var key = player.GetAssignedKey((Player.Action)i);
                bindingLabels[i].SetText(key.ToString());
            }
        }

        private void AddButtonLabel(Player.Action action, float y, string text, Context context)
        {
            var actionIndex = (int)action;

            bindingButtons[actionIndex] = new Button(context.Fonts, context.Textures);
            bindingButtons[actionIndex].Position = new Vector2f(80, y);
            bindingButtons[actionIndex].SetText(text);
            bindingButtons[actionIndex].SetToggle(true);

            bindingLabels[actionIndex] = new Label(string.Empty, context.Fonts);
            bindingLabels[actionIndex].Position = new Vector2f(300, y + 15);

            guiContainer.Pack(bindingButtons[actionIndex]);
            guiContainer.Pack(bindingLabels[actionIndex]);
        }
    }
}
