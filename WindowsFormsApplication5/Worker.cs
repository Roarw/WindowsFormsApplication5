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
        Transform transform;
        bool gold;

        public Worker(GameObject gameObject) : base(gameObject)
        {

        }

        public void LoadContent()
        {
            animator = (Animator)gameObject.GetComponent(Components.Animator);
            transform = (Transform)gameObject.GetComponent(Components.Transform);
            CreateAnimation();
            animator.PlayAnimation(AnimationName.Left);
        }

        public void Update(float deltaTime)
        {
            if (transform.Position.X <= 100 && !gold)
            {
                transform.Translate(new Vector2(1, 0));
            }

            else if (transform.Position.X > 100 && transform.Position.X < 120 && !gold)
            {
                Thread.Sleep(1000);
                gold = true;
            }

            else if (gold && transform.Position.X >= 0)
            {
                transform.Translate(new Vector2(-1, 0));
            }

            else if (transform.Position.X < 0 && transform.Position.X > -20 && gold)
            {
                Thread.Sleep(1000);
                gold = false;
            }
        }

        private void CreateAnimation()
        {
            animator.CreateAnimation(AnimationName.Standard, new Animation(1, 0, 0, 72, 67, 0));
            animator.CreateAnimation(AnimationName.Left, new Animation(4, 72, 0, 72, 67, 1));
            animator.CreateAnimation(AnimationName.Right, new Animation(4, 144, 0, 72, 67, 1));
        }
    }
}
