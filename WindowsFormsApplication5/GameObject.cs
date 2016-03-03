using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication5
{
    enum Components
    {
        Transform,
        SpriteRender,
        Animator,
        Collider,
        Worker
    }

    class GameObject : Component, ILoadable, IUpdateable, IDrawable, ICollisionEnter
    {
        List<Component> componentList;
        bool isLoaded;

        public GameObject(Vector2 position) : base()
        {
            gameObject = this;

            this.componentList = new List<Component>();
            componentList.Add(new Transform(this, position));

            isLoaded = false;
        }

        public Component GetComponent(Components componentType)
        {
            return componentList.Find(x => x.GetType().Name == componentType.ToString());
        }

        public void AddComponent(Component component)
        {
            componentList.Add(component);
        }

        public void LoadContent()
        {
            if (!isLoaded)
            {
                foreach (Component component in componentList)
                {
                    if (component is ILoadable)
                    {
                        (component as ILoadable).LoadContent();
                    }
                }
                isLoaded = true;
            }
        }

        public void Update(float deltaTime)
        {
            foreach (Component component in componentList)
            {
                if (component is IUpdateable)
                {
                    (component as IUpdateable).Update(deltaTime);
                }
            }
        }

        public void Draw(Graphics dc)
        {
            foreach (Component component in componentList)
            {
                if (component is IDrawable)
                {
                    (component as IDrawable).Draw(dc);
                }
            }
        }

        public void OnCollisionEnter(Collider other)
        {
            foreach (Component component in componentList)
            {
                if (component is ICollisionEnter)
                {
                    (component as ICollisionEnter).OnCollisionEnter(other);
                }
            }
        }
    }
}
