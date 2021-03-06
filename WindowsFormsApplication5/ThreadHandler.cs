﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class ThreadHandler
    {
        private GameObject gameObject;

        public ThreadHandler(GameObject gameObject)
        {
            this.gameObject = gameObject;

            Update();
        }

        private void Update()
        {
            while (true)
            {
                gameObject.Update(GameWorld.DeltaTime);

                Thread.Sleep(5);
            }
        }
    }
}
