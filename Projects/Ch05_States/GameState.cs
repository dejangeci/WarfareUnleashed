namespace Ch05_States
{
  using SFML.Window;
  using System;

  internal class GameState : State
  {
    private World world;
    private Player player;

    public GameState(StateStack stack, Context context)
      : base(stack, context)
    {
      world = new World(context.Window);
      player = context.Player;
    }

    public override void Draw()
    {
      world.Draw();
    }

    public override bool Update(TimeSpan dt)
    {
      world.Update(dt);

      var commands = world.GetCommandQueue();
      player.HandleRealtimeInput(commands);

      return true;
    }

    public override bool HandleKeyboardInput(Keyboard.Key key, bool isPressed)
    {
      // Game input handling
      var commands = world.GetCommandQueue();
      player.HandleKeyboardInput(key, isPressed, commands);

      // Escape pressed, trigger the pause screen
      if (isPressed && key == Keyboard.Key.Escape)
      {
        RequestStackPush(States.ID.Pause);
      }

      return true;
    }
  }
}
