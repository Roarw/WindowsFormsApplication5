using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication5
{
    class Collider : Component, ILoadable, IUpdateable
    {
        SpriteRender spriteRender;
        List<Collider> otherColliders;

        public Rectangle CollisionBox
        {
            get
            {
                Transform transform = (Transform)gameObject.GetComponent(Components.Transform);
                return new Rectangle(
                    (int)(transform.Position.X), (int)(transform.Position.Y),
                    spriteRender.Rectangle.Width, spriteRender.Rectangle.Height);

            }
        }

        public Collider(GameObject gameObject) : base(gameObject)
        {
            GameWorld.Colliders.Add(this);

            this.otherColliders = new List<Collider>();
        } 

        public void LoadContent()
        {
            this.spriteRender = (SpriteRender)gameObject.GetComponent(Components.SpriteRender);
        }

        public void Update(float deltaTime)
        {
            CheckCollision();
        }

        private void CheckCollision()
        {
            foreach (Collider other in GameWorld.Colliders)
            {
                if (other != this)
                {
                    if (IsCollidingWith(other))
                    {
                        if (!otherColliders.Contains(other))
                        {
                            System.Diagnostics.Debug.WriteLine("Added collider");
                            otherColliders.Add(other);
                        }
                    }
                    else
                    {
                        if (otherColliders.Contains(other))
                        {
                            System.Diagnostics.Debug.WriteLine("Removed collider");
                            otherColliders.Remove(other);
                        }
                    }
                }
            }
        }

        private bool IsCollidingWith(Collider other)
        {
            return CollisionBox.IntersectsWith(other.CollisionBox);
        }
    }
}
