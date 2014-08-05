namespace Ch04_Input
{
    using System;
    using System.Diagnostics;
    using SFML.Graphics;
    using SFML.Window;

    internal class Game
    {
        private readonly TimeSpan timePerFrame = TimeSpan.FromSeconds(1 / 60f);

        private RenderWindow window;
        private World world;
        private Player player;

        private Font font;
        private Text statisticsText;
        private TimeSpan statisticsUpdateTime;
        private int statisticsNumFrames;

        public Game()
        {
            window = new RenderWindow(new VideoMode(640, 480), "Input", Styles.Close);

            // Different from original code since C# events are used instead of polling
            window.Closed += (o, a) => window.Close();
            window.KeyPressed += (o, a) => HandlePlayerInput(a.Code, true);
            window.KeyReleased += (o, a) => HandlePlayerInput(a.Code, false);

            world = new World(window);

            player = new Player();

            font = new Font("Media/Sansation.ttf");
            statisticsText = new Text();
            statisticsText.Font = font;
            statisticsText.Position = new Vector2f(5, 5);
            statisticsText.CharacterSize = 10;
        }

        public void Run()
        {
            var stopwatch = new Stopwatch();
            var timeSinceLastUpdate = TimeSpan.Zero;
            while (window.IsOpen())
            {
                var elapsedTime = stopwatch.Elapsed;
                stopwatch.Restart();
                timeSinceLastUpdate += elapsedTime;

                while (timeSinceLastUpdate > timePerFrame)
                {
                    timeSinceLastUpdate -= timePerFrame;

                    ProcessInput();
                    Update(timePerFrame);
                }

                UpdateStatistics(elapsedTime);
                Render();
            }
        }

        private void ProcessInput()
        {
            window.DispatchEvents();

            player.HandleRealtimeInput(world.GetCommandQueue());
        }

        private void Update(TimeSpan elapsedTime)
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

        private void UpdateStatistics(TimeSpan elapsedTime)
        {
            statisticsUpdateTime += elapsedTime;
            statisticsNumFrames += 1;

            if (statisticsUpdateTime >= TimeSpan.FromSeconds(1))
            {
                statisticsText.DisplayedString = string.Format("Frames / Second = {0}\nTime / Update = {1} ticks\nUse arrow keys to move.", statisticsNumFrames, statisticsUpdateTime.Ticks / statisticsNumFrames);

                statisticsUpdateTime -= TimeSpan.FromSeconds(1);
                statisticsNumFrames = 0;
            }
        }

        private void HandlePlayerInput(Keyboard.Key key, bool isPressed)
        {
            player.HandleKeyboardInput(key, isPressed, world.GetCommandQueue());
        }
    }
}
