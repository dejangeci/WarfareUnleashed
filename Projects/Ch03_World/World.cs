namespace Ch03_World
{
    using System;
    using System.Collections.Generic;
    using SFML.Graphics;
    using SFML.Window;

    internal class World
    {
        private enum Layer
        {
            Background,
            Air,
            LayerCount
        }

        private RenderWindow window;
        private View worldView;
        private TextureHolder textures;

        private SceneNode sceneGraph;
        private List<SceneNode> sceneLayers;

        private FloatRect worldBounds;
        private Vector2f spawnPosition;
        private float scrollSpeed;
        private Aircraft playerAircraft;

        public World(RenderWindow window)
        {
            this.window = window;
            worldView = new View(this.window.DefaultView); // Make a copy of the default view, so we don't modify it by accident. Fixed in later version of SFML.Net
            textures = new TextureHolder();

            sceneGraph = new SceneNode();
            sceneLayers = new List<SceneNode>();

            worldBounds = new FloatRect(0, 0, worldView.Size.X, 2000);
            spawnPosition = new Vector2f(worldView.Size.X / 2, worldBounds.Height - (worldView.Size.Y / 2));
            scrollSpeed = -50;

            LoadTextures();
            BuildScene();

            // Prepare the view
            worldView.Center = spawnPosition;
        }

        public void Update(TimeSpan dt)
        {
            // Scroll the world
            worldView.Move(new Vector2f(0, scrollSpeed * (dt.Milliseconds / 1000f)));

            // Move the player sidewards (plane scouts follow the main aircraft)
            var position = playerAircraft.Position;
            var velocity = playerAircraft.GetVelocity();

            // If player touches borders, flip its X velocity
            if (position.X <= worldBounds.Left + 150 ||
                position.X >= worldBounds.Left + worldBounds.Width - 150)
            {
                velocity.X = -velocity.X;
                playerAircraft.SetVelocity(velocity);
            }

            // Apply movements
            sceneGraph.Update(dt);
        }

        public void Draw()
        {
            window.SetView(worldView);
            window.Draw(sceneGraph);
        }

        private void LoadTextures()
        {
            textures.Load(Textures.ID.Eagle, "Media/Textures/Eagle.png");
            textures.Load(Textures.ID.Raptor, "Media/Textures/Raptor.png");
            textures.Load(Textures.ID.Desert, "Media/Textures/Desert.png");
        }

        private void BuildScene()
        {
            // Initialize the different layers
            for (int i = 0; i < (int)Layer.LayerCount; i++)
            {
                var layer = new SceneNode();
                sceneLayers.Add(layer);

                sceneGraph.AttachChild(layer);
            }

            // Prepare the tiled background
            var texture = textures.Get(Textures.ID.Desert);
            var textureRect = new IntRect((int)worldBounds.Left, (int)worldBounds.Top, (int)worldBounds.Width, (int)worldBounds.Height);
            texture.Repeated = true;

            // Add the background sprite to the scene
            var backgroundSprite = new SpriteNode(texture, textureRect);
            backgroundSprite.Position = new Vector2f(worldBounds.Left, worldBounds.Top);
            sceneLayers[(int)Layer.Background].AttachChild(backgroundSprite);

            // Add player's aircraft
            playerAircraft = new Aircraft(Aircraft.Type.Eagle, textures);
            playerAircraft.Position = spawnPosition;
            playerAircraft.SetVelocity(40f, scrollSpeed);
            sceneLayers[(int)Layer.Air].AttachChild(playerAircraft);

            // Add two escorting aircrafts, placed relatively to the main plane
            var leftEscort = new Aircraft(Aircraft.Type.Raptor, textures);
            leftEscort.Position = new Vector2f(-80f, 50f);
            playerAircraft.AttachChild(leftEscort);

            var rightEscort = new Aircraft(Aircraft.Type.Raptor, textures);
            rightEscort.Position = new Vector2f(80f, 50f);
            playerAircraft.AttachChild(rightEscort);
        }
    }
}
