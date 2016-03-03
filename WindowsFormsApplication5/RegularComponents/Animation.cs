using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication5
{
    class Animation
    {
        int frames;
        float fps;
        Rectangle[] rectangles;

        public int Frames { get { return frames; } }
        public float FPS { get { return fps; } }
        public Rectangle[] Rectangles { get { return rectangles; } }

        public Animation(int frames, int yPos, int xStartFrame, int width, int height, float fps)
        {
            this.frames = frames;
            this.fps = fps;
            this.rectangles = new Rectangle[frames];

            for (int i = 0; i < frames; i++)
            {
                rectangles[i] = new Rectangle((i + xStartFrame) * width, yPos, width, height);
            }
        }
    }
}
