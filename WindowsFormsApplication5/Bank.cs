using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class Bank : Component, ICollisionEnter
    {
        object thisLock = new object();
        static float balance;

        public static int Balace { get { return (int)balance; } }

        public Bank(GameObject gameObject) : base(gameObject)
        {
            balance = 0;
        }

        public void OnCollisionEnter(Collider other)
        {
            Worker w = (Worker)other.GameObject.GetComponent(Components.Worker);

            if (w != null)
            {
                Monitor.Enter(thisLock);
                
                Thread.Sleep(1500);
                balance += w.Gold;
                w.Gold = 0;
                w.Wait = false;

                Monitor.Exit(thisLock);
            }
        }
    }
}
