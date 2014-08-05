namespace Ch03_World
{
    using System;
    using System.Collections.Generic;
    using SFML.Graphics;
    using SFML.Window;

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

        public void Update(TimeSpan dt)
        {
            UpdateCurrent(dt);
            UpdateChildren(dt);
        }

        protected virtual void UpdateCurrent(TimeSpan dt)
        {
            // Do nothing by default
        }

        private void UpdateChildren(TimeSpan dt)
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
    }
}
