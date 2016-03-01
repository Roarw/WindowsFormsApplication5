using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication5
{
    enum AnimationName
    {
        Standard
    }

    class Animator: Component, ILoadable, IUpdateable
    {
        Dictionary<AnimationName, Animation> animations;
        SpriteRender spriteRender;
        Rectangle[] rectangles;
        AnimationName animationName;
        float fps;
        int currentIndex;
        float timeElapsed;

        public Dictionary<AnimationName, Animation> Animations { get { return animations; } }
        public AnimationName AnimationName { get { return animationName; } }

        public Animator(GameObject gameObject) : base(gameObject)
        {
            this.animations = new Dictionary<AnimationName, Animation>();
        }

        public void LoadContent()
        {
            this.spriteRender = (SpriteRender)gameObject.GetComponent(Components.SpriteRender);
        }

        public void Update(float deltaTime)
        {
            rectangles = animations[animationName].Rectangles;
            timeElapsed += deltaTime;
            currentIndex = (int)(timeElapsed * fps);

            if (currentIndex >= rectangles.Length)
            {
                timeElapsed = 0;
                currentIndex = 0;
            }

            spriteRender.Rectangle = rectangles[currentIndex];
        }

        public void CreateAnimation(AnimationName animationName)
        {
            if (this.animationName != animationName)
            {
                this.rectangles = animations[animationName].Rectangles;
                this.spriteRender.Rectangle = rectangles[0];
                this.animationName = animationName;
                this.fps = animations[animationName].FPS;
                timeElapsed = 0;
                currentIndex = 0;
            }
        }
    }
}
