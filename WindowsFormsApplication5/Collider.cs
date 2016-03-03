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
        Transform transform;

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    (int)(transform.Position.X), (int)(transform.Position.Y),
                    spriteRender.Rectangle.Width, spriteRender.Rectangle.Height);
            }
        }

        public Collider(GameObject gameObject) : base(gameObject)
        {
            this.otherColliders = new List<Collider>();
        } 

        public void LoadContent()
        {
            transform = (Transform)gameObject.GetComponent(Components.Transform);
            this.spriteRender = (SpriteRender)gameObject.GetComponent(Components.SpriteRender);
            GameWorld.Colliders.Add(this);
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
                        gameObject.OnCollisionStay(other);

                        if (!otherColliders.Contains(other))
                        {
                            otherColliders.Add(other);
                            GameObject.OnCollisionEnter(other);
                            //System.Diagnostics.Debug.WriteLine("Added collider: " + otherColliders.Count);
                        }
                    }
                    else
                    {
                        if (otherColliders.Contains(other))
                        {
                            otherColliders.Remove(other);
                            //System.Diagnostics.Debug.WriteLine("Removed collider: " + otherColliders.Count);
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
