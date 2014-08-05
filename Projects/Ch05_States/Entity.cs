namespace Ch05_States
{
    using System;
    using SFML.Window;

    internal abstract class Entity : SceneNode
    {
        private Vector2f velocity = new Vector2f();

        public void SetVelocity(Vector2f velocity)
        {
            this.velocity = velocity;
        }

        public void SetVelocity(float x, float y)
        {
            velocity.X = x;
            velocity.Y = y;
        }

        public void Accelerate(Vector2f velocity)
        {
            this.velocity += velocity;
        }

        public void Accelerate(float x, float y)
        {
            this.velocity.X += x;
            this.velocity.Y += y;
        }

        public Vector2f GetVelocity()
        {
            return velocity;
        }

        protected override void UpdateCurrent(TimeSpan dt)
        {
            Position += velocity * (dt.Milliseconds / 1000f);
        }
    }
}
