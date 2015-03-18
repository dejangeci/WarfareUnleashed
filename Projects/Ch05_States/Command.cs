﻿namespace Ch05_States
{
  using SFML.System;

  internal abstract class Command
  {
    public Category Category { get; set; }

    public abstract void Execute(SceneNode subject, Time dt);
  }
}
