﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class ThreadHandler
    {
        private GameObject gameObject;
        private bool isAlive;

        public ThreadHandler(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.isAlive = true;

            Update();
        }

        private void Update()
        {
            while (isAlive)
            {
                gameObject.Update(GameWorld.DeltaTime);

                Thread.Sleep(5);
            }
        }
    }
}