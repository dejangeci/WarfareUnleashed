namespace Ch05_States
{
  using SFML.Graphics;
  using SFML.System;
  using SFML.Window;

  internal class TitleState : State
  {
    private Sprite backgroundSprite;
    private Text text;

    private bool showText;
    private Time textEffectTime;

    public TitleState(StateStack stack, Context context)
      : base(stack, context)
    {
      backgroundSprite = new Sprite();
      text = new Text();
      showText = true;
      textEffectTime = Time.Zero;

      backgroundSprite.Texture = context.Textures.Get(Textures.ID.TitleScreen);

      text.Font = context.Fonts.Get(Fonts.ID.Main);
      text.DisplayedString = "Press any key to start";
      Utility.CenterOrigin(text);
      text.Position = context.Window.GetView().Size / 2;
    }

    public override void Draw()
    {
      var window = GetContext().Window;
      window.Draw(backgroundSprite);

      if (showText) window.Draw(text);
    }

    public override bool Update(Time dt)
    {
      textEffectTime += dt;

      if (textEffectTime >= Time.FromSeconds(.5f))
      {
        showText = !showText;
        textEffectTime = Time.Zero;
      }

      return true;
    }

    public override bool HandleKeyboardInput(Keyboard.Key key, bool isPressed)
    {
      // If any key is pressed, trigger the next screen
      if (isPressed)
      {
        RequestStackPop();
        RequestStackPush(States.ID.Menu);
      }

      return true;
    }
  }
}
