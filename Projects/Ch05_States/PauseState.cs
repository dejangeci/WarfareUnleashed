namespace Ch05_States
{
  using SFML.Graphics;
  using SFML.System;
  using SFML.Window;
  using System;

  internal class PauseState : State
  {
    private Sprite backgroundSprite;
    private Text pausedText;
    private Text instructionText;

    public PauseState(StateStack stack, Context context)
      : base(stack, context)
    {
      backgroundSprite = new Sprite();
      pausedText = new Text();
      instructionText = new Text();

      var font = context.Fonts.Get(Fonts.ID.Main);
      var viewSize = context.Window.GetView().Size;

      pausedText.Font = font;
      pausedText.DisplayedString = "Game Paused";
      pausedText.CharacterSize = 70;
      Utility.CenterOrigin(pausedText);
      pausedText.Position = new Vector2f(0.5f * viewSize.X, 0.4f * viewSize.Y);

      instructionText.Font = font;
      instructionText.DisplayedString = "(Press Backspace to return to the main menu)";
      Utility.CenterOrigin(instructionText);
      instructionText.Position = new Vector2f(0.5f * viewSize.X, 0.6f * viewSize.Y);
    }

    public override void Draw()
    {
      var window = GetContext().Window;
      window.SetView(window.DefaultView);

      var backgroundShape = new RectangleShape();
      backgroundShape.FillColor = new Color(0, 0, 0, 150);
      backgroundShape.Size = window.GetView().Size;

      window.Draw(backgroundShape);
      window.Draw(pausedText);
      window.Draw(instructionText);
    }

    public override bool Update(TimeSpan dt)
    {
      return false;
    }

    public override bool HandleKeyboardInput(Keyboard.Key key, bool isPressed)
    {
      if (!isPressed) return false;

      if (key == Keyboard.Key.Escape)
      {
        // Escape pressed, remove itself to return to the game
        RequestStackPop();
      }

      if (key == Keyboard.Key.BackSpace)
      {
        // BackSpace pressed, clear stack and go to main menu
        RequestStateClear();
        RequestStackPush(States.ID.Menu);
      }

      return false;
    }
  }
}
