namespace Ch06_Menus
{
  using Ch06_Menus.GUI;
  using SFML.Graphics;
  using SFML.System;
  using SFML.Window;

  internal class PauseState : State
  {
    private Sprite backgroundSprite;
    private Text pausedText;
    private Container guiContainer;

    public PauseState(StateStack stack, Context context)
      : base(stack, context)
    {
      backgroundSprite = new Sprite();
      pausedText = new Text();
      guiContainer = new Container();

      var font = context.Fonts.Get(Fonts.ID.Main);
      var windowSize = context.Window.Size;

      pausedText.Font = font;
      pausedText.DisplayedString = "Game Paused";
      pausedText.CharacterSize = 70;
      Utility.CenterOrigin(pausedText);
      pausedText.Position = new Vector2f(0.5f * windowSize.X, 0.4f * windowSize.Y);

      var returnButton = new Button(context.Fonts, context.Textures);
      returnButton.Position = new Vector2f((0.5f * windowSize.X) - 100, (0.4f * windowSize.Y) + 75);
      returnButton.SetText("Return");
      returnButton.SetCallback(() =>
      {
        RequestStackPop();
      });

      var backToMenuButton = new Button(context.Fonts, context.Textures);
      backToMenuButton.Position = new Vector2f((0.5f * windowSize.X) - 100, (0.4f * windowSize.Y) + 125);
      backToMenuButton.SetText("Back to menu");
      backToMenuButton.SetCallback(() =>
      {
        RequestStateClear();
        RequestStackPush(States.ID.Menu);
      });

      guiContainer.Pack(returnButton);
      guiContainer.Pack(backToMenuButton);
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
      window.Draw(guiContainer);
    }

    public override bool Update(Time dt)
    {
      return false;
    }

    public override bool HandleKeyboardInput(Keyboard.Key key, bool isPressed)
    {
      guiContainer.HandleKeyboardEvent(key, isPressed);
      return false;
    }
  }
}
