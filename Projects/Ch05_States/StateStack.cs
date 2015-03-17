namespace Ch05_States
{
  using SFML.Window;
  using System;
  using System.Collections.Generic;

  internal class StateStack
  {
    public enum Action
    {
      Push,
      Pop,
      Clear
    }

    private List<State> stack;
    private List<PendingChange> pendingList;

    private State.Context context;
    private Dictionary<States.ID, Func<State>> factories;

    public StateStack(State.Context context)
    {
      stack = new List<State>();
      pendingList = new List<PendingChange>();
      this.context = context;
      factories = new Dictionary<States.ID, Func<State>>();
    }

    public void RegisterState<TState>(States.ID stateId, Func<TState> stateFactory) where TState : State
    {
      factories.Add(stateId, stateFactory);
    }

    public void Update(TimeSpan dt)
    {
      // Iterate from top to bottom, stop as soon as update() returns false
      for (int i = stack.Count - 1; i >= 0; i--)
      {
        if (!stack[i].Update(dt))
        {
          break;
        }
      }

      ApplyPendingChanges();
    }

    public void Draw()
    {
      foreach (var state in stack)
      {
        state.Draw();
      }
    }

    public void HandleKeyboardInput(Keyboard.Key key, bool isPressed)
    {
      // Iterate from top to bottom, stop as soon as handleEvent() returns false
      for (int i = stack.Count - 1; i >= 0; i--)
      {
        if (!stack[i].HandleKeyboardInput(key, isPressed))
        {
          break;
        }
      }

      ApplyPendingChanges();
    }

    public void PushState(States.ID stateId)
    {
      pendingList.Add(new PendingChange(Action.Push, stateId));
    }

    public void PopState()
    {
      pendingList.Add(new PendingChange(Action.Pop));
    }

    public void ClearStates()
    {
      pendingList.Add(new PendingChange(Action.Clear));
    }

    public bool IsEmpty()
    {
      return stack.Count == 0;
    }

    private State CreateState(States.ID stateId)
    {
      Func<State> stateFactoryDelegate;
      var found = factories.TryGetValue(stateId, out stateFactoryDelegate);

      return stateFactoryDelegate(); // Execute it
    }

    private void ApplyPendingChanges()
    {
      foreach (var change in pendingList)
      {
        switch (change.Action)
        {
          case Action.Push:
            stack.Add(CreateState(change.StateId));
            break;

          case Action.Pop:
            stack.RemoveAt(stack.Count - 1);
            break;

          case Action.Clear:
            stack.Clear();
            break;
        }
      }

      pendingList.Clear();
    }

    private struct PendingChange
    {
      public Action Action;
      public States.ID StateId;

      public PendingChange(Action action, States.ID stateID = States.ID.None)
      {
        Action = action;
        StateId = stateID;
      }
    }
  }
}
