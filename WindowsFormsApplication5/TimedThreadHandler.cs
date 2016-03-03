using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class TimedThreadHandler
    {
        private GameObject gameObject;
        private bool isAlive;
        float timeAlive;

        public TimedThreadHandler(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.isAlive = true;
            timeAlive = 0;

            Update();
        }

        private void Update()
        {
            while (isAlive)
            {
                gameObject.Update(GameWorld.DeltaTime);

                Thread.Sleep(5);

                timeAlive += 0.1f;
                if (timeAlive > 100)
                {
                    isAlive = false;
                    GameWorld.RemoveObject(gameObject);
                }
            }
        }
    }
}
