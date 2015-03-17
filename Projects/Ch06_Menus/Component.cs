namespace Ch06_Menus.GUI
{
  using SFML.Graphics;
  using SFML.Window;

  internal abstract class Component : Transformable, Drawable
  {
    private bool isSelected;
    private bool isActive;

    public abstract bool IsSelectable();

    public bool IsSelected()
    {
      return isSelected;
    }

    public virtual void Select()
    {
      isSelected = true;
    }

    public virtual void Deselect()
    {
      isSelected = false;
    }

    public virtual bool IsActive()
    {
      return isActive;
    }

    public virtual void Activate()
    {
      isActive = true;
    }

    public virtual void Deactivate()
    {
      isActive = false;
    }

    public abstract void HandleKeyboardEvent(Keyboard.Key key, bool isPressed);

    public abstract void Draw(RenderTarget target, RenderStates states);
  }
}
