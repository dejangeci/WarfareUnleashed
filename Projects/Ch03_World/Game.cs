namespace Ch03_World
{
  using SFML.Graphics;
  using SFML.System;
  using SFML.Window;

  internal class Game
  {
    private readonly Time timePerFrame = Time.FromSeconds(1 / 60f);

    private RenderWindow window;
    private World world;

    private Font font;
    private Text statisticsText;
    private Time statisticsUpdateTime;
    private int statisticsNumFrames;

    public Game()
    {
      window = new RenderWindow(new VideoMode(640, 480), "World", Styles.Close);

      // Different from original code since C# events are used instead of polling
      window.Closed += (o, a) => window.Close();
      window.KeyPressed += (o, a) => HandlePlayerInput(a.Code, true);
      window.KeyReleased += (o, a) => HandlePlayerInput(a.Code, false);

      world = new World(window);

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
      world.Update(elapsedTime);
    }

    private void Render()
    {
      window.Clear();
      world.Draw();

      window.SetView(window.DefaultView);
      window.Draw(statisticsText);
      window.Display();
    }

    private void UpdateStatistics(Time elapsedTime)
    {
      statisticsUpdateTime += elapsedTime;
      statisticsNumFrames += 1;

      if (statisticsUpdateTime >= Time.FromSeconds(1))
      {
        statisticsText.DisplayedString = string.Format("Frames / Second = {0}\nMicroseconds / Frame = {1}", statisticsNumFrames, statisticsUpdateTime.AsMicroseconds() / statisticsNumFrames);

        statisticsUpdateTime -= Time.FromSeconds(1);
        statisticsNumFrames = 0;
      }
    }

    private void HandlePlayerInput(Keyboard.Key key, bool isPressed)
    {
    }
  }
}
