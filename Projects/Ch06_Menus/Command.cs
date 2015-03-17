namespace Ch06_Menus
{
  using System;

  internal abstract class Command
  {
    public Category Category { get; set; }

    public abstract void Execute(SceneNode subject, TimeSpan dt);
  }
}
