namespace Ch04_Input
{
    using System;
    using System.Collections.Generic;
    using SFML.Graphics;
    using SFML.Window;

    internal class World
    {
        private RenderWindow window;
        private View worldView;
        private TextureHolder textures;

        private SceneNode sceneGraph;
        private List<SceneNode> sceneLayers;
        private CommandQueue commandQueue;

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
            commandQueue = new CommandQueue();

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
            // Scroll the world, reset player velocity
            worldView.Move(new Vector2f(0, scrollSpeed * (dt.Milliseconds / 1000f)));
            playerAircraft.SetVelocity(0, 0);

            // Forward commands to scene graph, adapt velocity (scrolling, diagonal correction)
            while (!commandQueue.IsEmpty())
            {
                sceneGraph.OnCommand(commandQueue.Pop(), dt);
            }

            AdaptPlayerVelocity();

            // Regular update step, adapt position (correct if outside view)
            sceneGraph.Update(dt);
            AdaptPlayerPosition();
        }

        public void Draw()
        {
            window.SetView(worldView);
            window.Draw(sceneGraph);
        }

        public CommandQueue GetCommandQueue()
        {
            return commandQueue;
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
        }

        private void AdaptPlayerPosition()
        {
            // Keep player's position inside the screen bounds, at least borderDistance units from the border
            Vector2f viewBoundsPosition = worldView.Center - (worldView.Size / 2);
            var viewBounds = new FloatRect(viewBoundsPosition.X, viewBoundsPosition.Y, worldView.Size.X, worldView.Size.Y);
            const float BorderDistance = 40;

            Vector2f position = playerAircraft.Position;
            position.X = Math.Max(position.X, viewBounds.Left + BorderDistance);
            position.X = Math.Min(position.X, viewBounds.Left + viewBounds.Width - BorderDistance);
            position.Y = Math.Max(position.Y, viewBounds.Top + BorderDistance);
            position.Y = Math.Min(position.Y, viewBounds.Top + viewBounds.Height - BorderDistance);
            playerAircraft.Position = position;
        }

        private void AdaptPlayerVelocity()
        {
            Vector2f velocity = playerAircraft.GetVelocity();

            // If moving diagonally, reduce velocity (to have always same velocity)
            if (velocity.X != 0 && velocity.Y != 0)
            {
                playerAircraft.SetVelocity(velocity / (float)Math.Sqrt(2));
            }

            // Add scrolling velocity
            playerAircraft.Accelerate(0, scrollSpeed);
        }

        private enum Layer
        {
            Background,
            Air,
            LayerCount
        }
    }
}
