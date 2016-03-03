using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class Bank : Component, ICollisionEnter, IUpdateable
    {
        Random random = new Random();
        object thisLock = new object();
        static float balance;

        public static int Balace { get { return (int)balance; } }

        public Bank(GameObject gameObject) : base(gameObject)
        {
            balance = 0;
        }

        public void Update(float deltaTime)
        {
            /// The worker causing the bank thread to sleep gives time for the gold to raise to 70,
            /// before 50 gold is spent on a worker. Thus giving a bunker a chance to spawn.
            /// This can only occur because the Collider's update is called before the Bank's update.
            if (balance > 70)
            {
                System.Diagnostics.Debug.WriteLine("Make a bunker.");
                balance -= 70;
            }
            if (balance > 50)
            {
                int i = random.Next(10);
                GameWorld.CreateWorkerThread(new Vector2(120, 15 + i * 15)).Start();
                balance -= 50;
            }
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
