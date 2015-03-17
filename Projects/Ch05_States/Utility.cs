namespace Ch05_States
{
  using SFML.Graphics;
  using SFML.System;

  internal class Utility
  {
    public static void CenterOrigin(Sprite sprite)
    {
      var bounds = sprite.GetLocalBounds();
      sprite.Origin = new Vector2f(bounds.Width / 2, bounds.Height / 2);
    }

    public static void CenterOrigin(Text text)
    {
      var bounds = text.GetLocalBounds();
      text.Origin = new Vector2f(bounds.Width / 2, bounds.Height / 2);
    }
  }
}
