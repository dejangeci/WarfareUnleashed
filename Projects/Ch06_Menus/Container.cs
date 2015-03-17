namespace Ch06_Menus.GUI
{
  using SFML.Graphics;
  using SFML.Window;
  using System.Collections.Generic;

  internal class Container : Component
  {
    private List<Component> children;
    private int selectedChild;

    public Container()
    {
      children = new List<Component>();
      selectedChild = -1;
    }

    public void Pack(Component component)
    {
      children.Add(component);

      if (!HasSelection() && component.IsSelectable())
      {
        Select(children.Count - 1);
      }
    }

    public override bool IsSelectable()
    {
      return false;
    }

    public override void HandleKeyboardEvent(Keyboard.Key key, bool isPressed)
    {
      // If we have selected a child then give it events
      if (HasSelection() && children[selectedChild].IsActive())
      {
        children[selectedChild].HandleKeyboardEvent(key, isPressed);
      }
      else if (!isPressed)
      {
        if (key == Keyboard.Key.W || key == Keyboard.Key.Up)
        {
          SelectPrevious();
        }
        else if (key == Keyboard.Key.S || key == Keyboard.Key.Down)
        {
          SelectNext();
        }
        else if (key == Keyboard.Key.Return || key == Keyboard.Key.Space)
        {
          if (HasSelection())
          {
            children[selectedChild].Activate();
          }
        }
      }
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
      states.Transform *= Transform;

      foreach (var child in children)
      {
        target.Draw(child, states);
      }
    }

    private bool HasSelection()
    {
      return selectedChild >= 0;
    }

    private void Select(int index)
    {
      if (children[index].IsSelectable())
      {
        if (HasSelection())
        {
          children[selectedChild].Deselect();
        }

        children[index].Select();
        selectedChild = index;
      }
    }

    private void SelectNext()
    {
      if (!HasSelection()) return;

      // Search next component that is selectable, wrap around if necessary
      int next = selectedChild;
      do
      {
        next = (next + 1) % children.Count;
      }
      while (!children[next].IsSelectable());

      // Select that component
      Select(next);
    }

    private void SelectPrevious()
    {
      if (!HasSelection()) return;

      // Search previous component that is selectable, wrap around if necessary
      int prev = selectedChild;
      do
      {
        prev = (prev + children.Count - 1) % children.Count;
      }
      while (!children[prev].IsSelectable());

      // Select that component
      Select(prev);
    }
  }
}
