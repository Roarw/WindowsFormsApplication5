using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class Crystal : Component, ICollisionEnter
    {
        object thisLock = new object();

        public Crystal(GameObject gameObject) : base(gameObject)
        {

        }

        public void OnCollisionEnter(Collider other)
        {
            Worker w = (Worker)other.GameObject.GetComponent(Components.Worker);

            if (w != null && w.Gold == 0)
            {
                lock(thisLock)
                {
                    Thread.Sleep(500);
                    w.Gold += 20;
                    w.Wait = false;
                }
            }
        }
    }
}
