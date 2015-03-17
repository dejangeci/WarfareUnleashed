﻿namespace Ch06_Menus
{
  using System;

  [Flags]
  internal enum Category
  {
    None = 0,
    Scene = 1 << 0,
    PlayerAircraft = 1 << 1,
    AlliedAircraft = 1 << 2,
    EnemyAircraft = 1 << 3,
  }
}
