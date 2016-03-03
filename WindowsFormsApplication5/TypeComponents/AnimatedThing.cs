using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class AnimatedThing : Component, ILoadable
    {
        Animator animator;

        public AnimatedThing(GameObject gameObject) : base(gameObject)
        {

        }

        public void LoadContent()
        {
            animator = (Animator)gameObject.GetComponent(Components.Animator);
            CreateAnimation();
            animator.PlayAnimation(AnimationName.Left);
        }

        private void CreateAnimation()
        {
            animator.CreateAnimation(AnimationName.Standard, new Animation(1, 0, 0, 72, 67, 0));
            animator.CreateAnimation(AnimationName.Left, new Animation(4, 72, 0, 72, 67, 10));
            animator.CreateAnimation(AnimationName.Right, new Animation(4, 144, 0, 72, 67, 10));    
        }
    }
}
