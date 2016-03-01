﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication5
{
    class SpriteRender : Component, ILoadable, IDrawable
    {
        string spriteName;
        Image sprite;
        float depth;
        Rectangle rectangle;

        public Image Sprite { get { return sprite; } }
        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }

        public SpriteRender(GameObject gameObject, string spriteName, float depth) : base(gameObject)
        {
            this.spriteName = spriteName;
            this.depth = depth;
        }

        public void LoadContent()
        {
            this.sprite = Image.FromFile(spriteName);
            this.rectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }

        public void Draw(Graphics dc)
        {
            Transform transform = (Transform)GameObject.GetComponent(Components.Transform);
            dc.DrawImage(sprite, transform.Position.X, transform.Position.Y);
        }
    }
}