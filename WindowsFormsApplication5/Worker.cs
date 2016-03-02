using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class Worker : Component, ILoadable, IUpdateable
    {
        Animator animator;

        public Worker(GameObject gameObject) : base(gameObject)
        {

        }

        public void LoadContent()
        {
            animator = (Animator)gameObject.GetComponent(Components.Animator);
            CreateAnimation();
            animator.PlayAnimation(AnimationName.Left);
        }

        public void Update(float deltaTime)
        {
            Transform t = (Transform)gameObject.GetComponent(Components.Transform);
            if (t.Position.X < 100)
            {
                t.Translate(new Vector2(1, 0));
            }
            else
            {
                Thread.Sleep(50000);
            }
            
        }

        private void CreateAnimation()
        {
            animator.CreateAnimation(AnimationName.Standard, new Animation(1, 0, 0, 72, 67, 0));
            animator.CreateAnimation(AnimationName.Left, new Animation(4, 72, 0, 72, 67, 10));
            animator.CreateAnimation(AnimationName.Right, new Animation(4, 144, 0, 72, 67, 10));
        }
    }
}
