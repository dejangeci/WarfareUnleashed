namespace Ch05_States
{
  using SFML.Graphics;
  using SFML.System;
  using SFML.Window;

  internal class Application
  {
    private readonly Time timePerFrame = Time.FromSeconds(1 / 60f);

    private RenderWindow window;
    private TextureHolder textures;
    private FontHolder fonts;
    private Player player;

    private StateStack stateStack;
    private State.Context stateStackContext;

    private Text statisticsText;
    private Time statisticsUpdateTime;
    private int statisticsNumFrames;

    public Application()
    {
      window = new RenderWindow(new VideoMode(640, 480), "States", Styles.Close);

      // Different from original code since C# events are used instead of polling
      window.Closed += (o, a) => window.Close();
      window.KeyPressed += (o, a) => HandleKeyboardInput(a.Code, true);
      window.KeyReleased += (o, a) => HandleKeyboardInput(a.Code, false);

      textures = new TextureHolder();
      fonts = new FontHolder();
      player = new Player();

      stateStackContext = new State.Context(window, textures, fonts, player);
      stateStack = new StateStack(stateStackContext);

      statisticsText = new Text();

      window.SetKeyRepeatEnabled(false);

      fonts.Load(Fonts.ID.Main, "Media/Sansation.ttf");
      textures.Load(Textures.ID.TitleScreen, "Media/Textures/TitleScreen.png");

      statisticsText.Font = fonts.Get(Fonts.ID.Main);
      statisticsText.Position = new Vector2f(5, 5);
      statisticsText.CharacterSize = 10;

      RegisterStates();
      stateStack.PushState(States.ID.Title);
    }

    public void Run()
    {
      var clock = new Clock();
      var timeSinceLastUpdate = Time.Zero;

      while (window.IsOpen)
      {
        var elapsedTime = clock.ElapsedTime;
        clock.Restart();
        timeSinceLastUpdate += elapsedTime;

        while (timeSinceLastUpdate > timePerFrame)
        {
          timeSinceLastUpdate -= timePerFrame;

          ProcessInput();
          Update(timePerFrame);

          // Check inside this loop, because stack might be empty before update() call
          if (stateStack.IsEmpty())
          {
            window.Close();
          }
        }

        UpdateStatistics(elapsedTime);
        Render();
      }
    }

    private void ProcessInput()
    {
      window.DispatchEvents();
    }

    private void HandleKeyboardInput(Keyboard.Key key, bool isPressed)
    {
      stateStack.HandleKeyboardInput(key, isPressed);
    }

    private void Update(Time dt)
    {
      stateStack.Update(dt);
    }

    private void Render()
    {
      window.Clear();

      stateStack.Draw();

      window.SetView(window.DefaultView);
      window.Draw(statisticsText);

      window.Display();
    }

    private void UpdateStatistics(Time dt)
    {
      statisticsUpdateTime += dt;
      statisticsNumFrames += 1;

      if (statisticsUpdateTime >= Time.FromSeconds(1))
      {
        statisticsText.DisplayedString = string.Format("FPS: {0}", statisticsNumFrames);

        statisticsUpdateTime -= Time.FromSeconds(1);
        statisticsNumFrames = 0;
      }
    }

    private void RegisterStates()
    {
      stateStack.RegisterState<TitleState>(States.ID.Title, () => { return new TitleState(stateStack, stateStackContext); });
      stateStack.RegisterState<MenuState>(States.ID.Menu, () => { return new MenuState(stateStack, stateStackContext); });
      stateStack.RegisterState<GameState>(States.ID.Game, () => { return new GameState(stateStack, stateStackContext); });
      stateStack.RegisterState<PauseState>(States.ID.Pause, () => { return new PauseState(stateStack, stateStackContext); });
    }
  }
}
