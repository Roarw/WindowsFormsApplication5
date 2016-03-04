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

            GameWorld.AddCollider(this);
        }

        public void Update(float deltaTime)
        {
            /// The reason we copy the Colliders list is to avoid two threads accessing the same list, at the same time.
            /// If we make a lock on the actual Colliders list we get some sort of starvation.
            List<Collider> colliders = new List<Collider>();
            lock (GameWorld.collidersLock)
            {
                colliders = new List<Collider>(GameWorld.Colliders);
            }
            CheckCollision(colliders);
        }

        private void CheckCollision(List<Collider> colliders)
        {
            ///Make sure no two threads acces the Colliders at the same time.
            foreach (Collider other in colliders)
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
