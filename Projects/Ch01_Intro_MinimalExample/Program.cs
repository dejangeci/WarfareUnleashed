namespace Ch01_Intro_MinimalExample
{
    using SFML.Graphics;
    using SFML.Window;

    internal class Program
    {
        private static void Main()
        {
            var window = new RenderWindow(new VideoMode(640, 480), "SFML Application");
            window.Closed += (o, a) => window.Close();

            var shape = new CircleShape();
            shape.Radius = 40f;
            shape.Position = new Vector2f(100f, 100f);
            shape.FillColor = Color.Cyan;

            while (window.IsOpen())
            {
                window.DispatchEvents();

                window.Clear();
                window.Draw(shape);
                window.Display();
            }
        }
    }
}
