namespace Ch04_Input
{
  using SFML.Graphics;
  using SFML.System;
  using System.Collections.Generic;

  internal class SceneNode : Transformable, Drawable
  {
    private SceneNode parent;
    private List<SceneNode> children;

    public SceneNode()
    {
      children = new List<SceneNode>();
    }

    public void AttachChild(SceneNode child)
    {
      child.parent = this;
      children.Add(child);
    }

    public SceneNode DetachChild(SceneNode child)
    {
      var index = children.FindIndex(sn => sn == child);

      var foundChild = children[index];
      foundChild.parent = null;
      children.RemoveAt(index);

      return foundChild;
    }

    public void Update(Time dt)
    {
      UpdateCurrent(dt);
      UpdateChildren(dt);
    }

    protected virtual void UpdateCurrent(Time dt)
    {
      // Do nothing by default
    }

    private void UpdateChildren(Time dt)
    {
      foreach (var child in children)
      {
        child.Update(dt);
      }
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
      // Apply transform of current node
      states.Transform *= Transform;

      // Draw node and children with changed transform
      DrawCurrent(target, states);
      DrawChildren(target, states);
    }

    protected virtual void DrawCurrent(RenderTarget target, RenderStates states)
    {
      // Do nothing by default
    }

    protected void DrawChildren(RenderTarget target, RenderStates states)
    {
      foreach (var child in children)
      {
        child.Draw(target, states);
      }
    }

    public Vector2f GetWorldPosition()
    {
      return GetWorldTransform() * new Vector2f();
    }

    public Transform GetWorldTransform()
    {
      var transform = Transform.Identity;

      for (var node = this; node != null; node = node.parent)
      {
        transform = node.Transform * transform;
      }

      return transform;
    }

    public void OnCommand(Command command, Time dt)
    {
      // Command current node, if category matches
      var category = this.GetCategory();
      if ((command.Category & category) == category)
      {
        command.Execute(this, dt);
      }

      // Command children
      foreach (var child in children)
      {
        child.OnCommand(command, dt);
      }
    }

    public virtual Category GetCategory()
    {
      return Category.Scene;
    }
  }
}
