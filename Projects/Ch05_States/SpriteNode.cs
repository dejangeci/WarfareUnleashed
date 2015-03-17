namespace Ch05_States
{
  using SFML.Graphics;

  internal class SpriteNode : SceneNode
  {
    private Sprite sprite;

    public SpriteNode(Texture texture)
    {
      sprite = new Sprite(texture);
    }

    public SpriteNode(Texture texture, IntRect rect)
    {
      sprite = new Sprite(texture, rect);
    }

    protected override void DrawCurrent(RenderTarget target, RenderStates states)
    {
      target.Draw(sprite, states);
    }
  }
}
