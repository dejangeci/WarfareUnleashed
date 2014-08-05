namespace Ch04_Input
{
    using System.ComponentModel;
    using SFML.Graphics;
    using SFML.Window;

    internal class Aircraft : Entity
    {
        internal enum Type
        {
            Eagle,
            Raptor
        }

        private Type type;
        private Sprite sprite;

        public static Textures.ID ToTextureID(Type type)
        {
            switch (type)
            {
                case Type.Eagle:
                    return Textures.ID.Eagle;
                case Type.Raptor:
                    return Textures.ID.Raptor;
            }

            return Textures.ID.Eagle;
        }

        public Aircraft(Type type, TextureHolder textures)
        {
            this.type = type;
            sprite = new Sprite(textures.Get(ToTextureID(type)));

            var bounds = sprite.GetLocalBounds();
            sprite.Origin = new Vector2f(bounds.Width / 2f, bounds.Height / 2f);
        }

        protected override void DrawCurrent(RenderTarget target, RenderStates states)
        {
            target.Draw(sprite, states);
        }

        public override Category GetCategory()
        {
            switch (type)
            {
                case Type.Eagle:
                    return Category.PlayerAircraft;
                default:
                    return Category.EnemyAircraft;
            }
        }
    }
}
