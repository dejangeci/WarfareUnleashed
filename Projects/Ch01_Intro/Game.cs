namespace Ch01_Intro
{
  using SFML.Graphics;
  using SFML.System;
  using SFML.Window;

  internal class Game
  {
    private const float PlayerSpeed = 100;
    private readonly Time timePerFrame = Time.FromSeconds(1 / 60f);

    private RenderWindow window;
    private Texture texture;
    private Sprite player;
    private Font font;
    private Text statisticsText;
    private Time statisticsUpdateTime;

    private int statisticsNumFrames;
    private bool isMovingUp;
    private bool isMovingDown;
    private bool isMovingLeft;
    private bool isMovingRight;

    public Game()
    {
      window = new RenderWindow(new VideoMode(640, 480), "SFML Application");

      // Different from original code since C# events are used instead of polling
      window.Closed += (o, a) => window.Close();
      window.KeyPressed += (o, a) => HandlePlayerInput(a.Code, true);
      window.KeyReleased += (o, a) => HandlePlayerInput(a.Code, false);

      try
      {
        texture = new Texture("Media/Textures/Eagle.png");
      }
      catch
      {
        // Handle loading error
      }

      player = new Sprite();
      player.Texture = texture;
      player.Position = new Vector2f(100, 100);

      font = new Font("Media/Sansation.ttf");
      statisticsText = new Text();
      statisticsText.Font = font;
      statisticsText.Position = new Vector2f(5, 5);
      statisticsText.CharacterSize = 10;
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

          ProcessEvents();
          Update(timePerFrame);
        }

        UpdateStatistics(elapsedTime);
        Render();
      }
    }

    private void ProcessEvents()
    {
      window.DispatchEvents();
    }

    private void Update(Time elapsedTime)
    {
      var movement = new Vector2f(0, 0);

      if (isMovingUp)
        movement.Y -= PlayerSpeed;
      if (isMovingDown)
        movement.Y += PlayerSpeed;
      if (isMovingLeft)
        movement.X -= PlayerSpeed;
      if (isMovingRight)
        movement.X += PlayerSpeed;

      player.Position += movement * elapsedTime.AsSeconds();
    }

    private void Render()
    {
      window.Clear();
      window.Draw(player);
      window.Draw(statisticsText);
      window.Display();
    }

    private void UpdateStatistics(Time elapsedTime)
    {
      statisticsUpdateTime += elapsedTime;
      statisticsNumFrames += 1;

      if (statisticsUpdateTime >= Time.FromSeconds(1))
      {
        statisticsText.DisplayedString = string.Format("Frames / Second = {0}\nMicroseconds / Frame = {1}\nUse WSAD to move.", statisticsNumFrames, statisticsUpdateTime.AsMicroseconds() / statisticsNumFrames);

        statisticsUpdateTime -= Time.FromSeconds(1);
        statisticsNumFrames = 0;
      }
    }

    private void HandlePlayerInput(Keyboard.Key key, bool isPressed)
    {
      if (key == Keyboard.Key.W)
        isMovingUp = isPressed;
      else if (key == Keyboard.Key.S)
        isMovingDown = isPressed;
      else if (key == Keyboard.Key.A)
        isMovingLeft = isPressed;
      else if (key == Keyboard.Key.D)
        isMovingRight = isPressed;
    }
  }
}
