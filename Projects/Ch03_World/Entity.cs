namespace Ch03_World
{
  using SFML.System;
  using System;

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
