namespace Ch05_States
{
  using SFML.Graphics;
  using SFML.System;

  internal class LoadingState : State
  {
    private Text loadingText;
    private RectangleShape progressBarBackground;
    private RectangleShape progressBar;

    private ParallelTask loadingTask;

    public LoadingState(StateStack stack, Context context)
      : base(stack, context)
    {
      loadingText = new Text();
      progressBarBackground = new RectangleShape();
      progressBar = new RectangleShape();

      loadingTask = new ParallelTask();

      var window = context.Window;
      var font = context.Fonts.Get(Fonts.ID.Main);
      var viewSize = window.GetView().Size;

      loadingText.Font = font;
      loadingText.DisplayedString = "Loading Resources";
      Utility.CenterOrigin(loadingText);
      loadingText.Position = new Vector2f(viewSize.X / 2, (viewSize.Y / 2) + 50);

      progressBarBackground.FillColor = Color.White;
      progressBarBackground.Size = new Vector2f(viewSize.X - 20, 10);
      progressBarBackground.Position = new Vector2f(10, loadingText.Position.Y + 40);

      progressBar.FillColor = new Color(100, 100, 100);
      progressBar.Size = new Vector2f(200, 10);
      progressBar.Position = new Vector2f(10, loadingText.Position.Y + 40);

      SetCompletion(0);

      loadingTask.Execute();
    }

    public override void Draw()
    {
      var window = GetContext().Window;

      window.SetView(window.DefaultView);

      window.Draw(loadingText);
      window.Draw(progressBarBackground);
      window.Draw(progressBar);
    }

    public override bool Update(Time dt)
    {
      // Update the progress bar from the remote task or finish it
      if (loadingTask.IsFinished())
      {
        RequestStackPop();
        RequestStackPush(States.ID.Game);
      }
      else
      {
        SetCompletion(loadingTask.GetCompletion());
      }

      return true;
    }

    public override bool HandleKeyboardInput(SFML.Window.Keyboard.Key key, bool isPressed)
    {
      return true;
    }

    public void SetCompletion(float percent)
    {
      // clamp
      if (percent > 1)
      {
        percent = 1;
      }

      progressBar.Size = new Vector2f(progressBarBackground.Size.X * percent, progressBar.Size.Y);
    }
  }
}
