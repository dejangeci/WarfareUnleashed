namespace Ch04_Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFML.Window;

    internal class AircraftMover : Command
    {
        private Vector2f velocity;

        public AircraftMover(float x, float y)
        {
            velocity = new Vector2f(x, y);
        }

        public override void Execute(SceneNode subject, TimeSpan dt)
        {
            ((Aircraft)subject).Accelerate(velocity);
        }
    }

    internal class Player
    {
        internal enum Action
        {
            MoveLeft,
            MoveRight,
            MoveUp,
            MoveDown,
            ActionCount
        }

        private Dictionary<Keyboard.Key, Action> keyBinding = new Dictionary<Keyboard.Key, Action>();
        private Dictionary<Action, Command> actionBinding = new Dictionary<Action, Command>();

        public Player()
        {
            // Set initial key bindings
            keyBinding.Add(Keyboard.Key.Left, Action.MoveLeft);
            keyBinding.Add(Keyboard.Key.Right, Action.MoveRight);
            keyBinding.Add(Keyboard.Key.Up, Action.MoveUp);
            keyBinding.Add(Keyboard.Key.Down, Action.MoveDown);

            // Set initial action bindings
            InitializeActions();

            // Assign all categories to player's aircraft
            foreach (var pair in actionBinding)
            {
                pair.Value.Category = Category.PlayerAircraft;
            }
        }

        // We cannot use HandleEvent from original code
        // since we dont have window event polling in the C# bindings
        // so this is implemented using C# events
        public void HandleKeyboardInput(Keyboard.Key key, bool isPressed, CommandQueue commands)
        {
            if (isPressed)
            {
                // Check if pressed key appears in key binding, trigger command if so
                Action playerAction;
                bool found = keyBinding.TryGetValue(key, out playerAction);

                if (found && !IsRealtimeAction(playerAction))
                {
                    commands.Push(actionBinding[playerAction]);
                }
            }
        }

        public void HandleRealtimeInput(CommandQueue commands)
        {
            // Traverse all assigned keys and check if they are pressed
            foreach (var pair in keyBinding)
            {
                // If key is pressed, lookup action and trigger corresponding command
                if (Keyboard.IsKeyPressed(pair.Key) && IsRealtimeAction(pair.Value))
                {
                    commands.Push(actionBinding[pair.Value]);
                }
            }
        }

        public void AssignKey(Action action, Keyboard.Key key)
        {
            var foundActions = keyBinding.Where(kvp => kvp.Value == action).ToList();

            // Remove all keys that already map to action
            foreach (var foundAction in foundActions)
            {
                keyBinding.Remove(foundAction.Key);
            }

            // Insert new binding
            keyBinding.Add(key, action);
        }

        public Keyboard.Key GetAssignedKey(Action action)
        {
            foreach (var pair in keyBinding)
            {
                if (pair.Value == action)
                {
                    return pair.Key;
                }
            }

            return Keyboard.Key.Unknown;
        }

        private void InitializeActions()
        {
            const float PlayerSpeed = 200;

            actionBinding.Add(Action.MoveLeft, new AircraftMover(-PlayerSpeed, 0));
            actionBinding.Add(Action.MoveRight, new AircraftMover(+PlayerSpeed, 0));
            actionBinding.Add(Action.MoveUp, new AircraftMover(0, -PlayerSpeed));
            actionBinding.Add(Action.MoveDown, new AircraftMover(0, +PlayerSpeed));
        }

        private static bool IsRealtimeAction(Action action)
        {
            switch (action)
            {
                case Action.MoveLeft:
                case Action.MoveRight:
                case Action.MoveUp:
                case Action.MoveDown:
                    return true;
                default:
                    return false;
            }
        }
    }
}
