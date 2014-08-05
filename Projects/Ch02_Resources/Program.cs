namespace Ch02_Resources
{
    using System;
    using System.Windows.Forms;
    using SFML.Graphics;
    using SFML.Window;

    // Resource ID for Texture
    namespace Textures
    {
        internal enum ID
        {
            Landscape,
            Airplane,
        }
    }

    internal class Program
    {
        private static void Main()
        {
            var window = new RenderWindow(new VideoMode(640, 480), "Resources");
            window.SetFramerateLimit(20);

            // Different from original code since C# events are used instead of polling
            window.Closed += (o, a) => window.Close();
            window.KeyPressed += (o, a) => window.Close();

            // Try to load resources
            var textures = new TextureHolder();
            try
            {
                textures.Load(Textures.ID.Landscape, "Media/Textures/Desert.png");
                textures.Load(Textures.ID.Airplane, "Media/Textures/Eagle.png");
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString());
                return;
            }

            // Access resources
            var landscape = new Sprite(textures.Get(Textures.ID.Landscape));
            var airplane = new Sprite(textures.Get(Textures.ID.Airplane));
            airplane.Position = new Vector2f(200, 200);

            while (window.IsOpen())
            {
                window.DispatchEvents();

                window.Clear();
                window.Draw(landscape);
                window.Draw(airplane);
                window.Display();
            }
        }
    }
}
