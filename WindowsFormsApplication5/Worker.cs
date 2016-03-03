using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class Worker : Component, ILoadable, IUpdateable, ICollisionEnter
    {
        Animator animator;
        Transform transform;
        AnimationName animationName;
        
        public float Gold { get; set; } = 0;
        public bool Wait { get; set; } = false;

        public Worker(GameObject gameObject) : base(gameObject)
        {
        }

        public void LoadContent()
        {
            animator = (Animator)gameObject.GetComponent(Components.Animator);
            transform = (Transform)gameObject.GetComponent(Components.Transform);
            CreateAnimation();
            animator.PlayAnimation(AnimationName.Standard);
        }

        public void Update(float deltaTime)
        {
            if (Wait)
            {
                animationName = AnimationName.Standard;
            }
            else if (Gold == 0)
            {
                transform.Translate(new Vector2(2, 0));
                animationName = AnimationName.Right;
            }
            else if (Gold > 0)
            {
                transform.Translate(new Vector2(-2, 0));
                animationName = AnimationName.Left;
            }

            animator.PlayAnimation(animationName);
        }

        private void CreateAnimation()
        {
            animator.CreateAnimation(AnimationName.Standard, new Animation(1, 0, 0, 72, 67, 0));
            animator.CreateAnimation(AnimationName.Left, new Animation(4, 72, 0, 72, 67, 3));
            animator.CreateAnimation(AnimationName.Right, new Animation(4, 144, 0, 72, 67, 3));
        }

        public void OnCollisionEnter(Collider other)
        {
            Bank b = (Bank)other.GameObject.GetComponent(Components.Bank);
            Crystal c = (Crystal)other.GameObject.GetComponent(Components.Crystal);

            if (b != null && Gold > 0 || c != null)
            {
                Wait = true;
            }
        }
    }
}
