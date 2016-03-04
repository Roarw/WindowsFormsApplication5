using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApplication5
{
    class GameWorld
    {
        private DateTime endTime;
        private Graphics dc;
        private BufferedGraphics backBuffer;
        private List<Thread> threads;
        private Brush b;
        private Font f;

        private static List<GameObject> objects;
        private static List<GameObject> bunkers;
        private static float deltaTime;
        private static object objectsLock = new object();
        private static object bunkerLock = new object();

        public static List<Collider> Colliders { get; } = new List<Collider>();
        public static object collidersLock = new object();

        public static int ObjectsCount { get { return objects.Count; } }
        public static int BunkersCount { get { return bunkers.Count; } }
        public static float DeltaTime { get { return deltaTime; } }

        /// sets the value of the variables and properties
        public GameWorld(Graphics dc, Rectangle displayRectangle)
        {
            backBuffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);
            this.dc = backBuffer.Graphics;

            objects = new List<GameObject>();
            threads = new List<Thread>();
            b = Brushes.Gold;
            f = new Font("Arial", 16);
            bunkers = new List<GameObject>();
        }

        /// Runs the setup and puts all the information into the lists/arrays.
        public void SetupWorld()
        {
            threads.Add(CreateBankThread(new Vector2(0, 30)));
            threads.Add(CreateCrystalThread(new Vector2(1200, 0)));
            threads.Add(CreateWorkerThread(new Vector2(100 + 4 * 5, 15 + 4 * 15)));
            
            //Loads content.
            foreach (Thread t in threads)
            {
                t.Start();
            }
        }
        
        /// Below is our builder methods.
        /// The CreateWorkerThread is also used by the bank to create new workers.
        public static Thread CreateWorkerThread(Vector2 position)
        {
            GameObject worker = WorkerBuilder(position);
            Thread t = new Thread(() => new TimedThreadHandler(worker));

            t.IsBackground = true;
            return t;
        }

        private Thread CreateBankThread(Vector2 position)
        {
            GameObject bank = BankBuilder(position);
            Thread t = new Thread(() => new ThreadHandler(bank));

            t.IsBackground = true;
            return t;
        }

        private Thread CreateCrystalThread(Vector2 position)
        {
            GameObject crystal = CrystalBuilder(position);
            Thread t = new Thread(() => new ThreadHandler(crystal));

            t.IsBackground = true;
            return t;
        }

        private static GameObject WorkerBuilder(Vector2 position)
        {
            GameObject go = new GameObject(position);
            go.AddComponent(new SpriteRender(go, "Pic/spritesheet.png", 0));
            go.AddComponent(new Collider(go));

            ///Animator and a component setting up animations are neccesary to make the Animator work.
            ///And the order which they are to be added is: Animator -> Component, to make the animator work.
            go.AddComponent(new Animator(go));
            go.AddComponent(new Worker(go));

            go.LoadContent();
            AddObject(go);

            return go;
        }

        private GameObject CrystalBuilder(Vector2 position)
        {
            GameObject go = new GameObject(position);
            go.AddComponent(new SpriteRender(go, "Pic/Crystal.png", 0));
            go.AddComponent(new Collider(go));
            go.AddComponent(new Crystal(go));

            go.LoadContent();
            AddObject(go);
            return go;
        }

        private GameObject BankBuilder(Vector2 position)
        {
            GameObject go = new GameObject(position);
            go.AddComponent(new SpriteRender(go, "Pic/Nexus.png", 0));
            go.AddComponent(new Collider(go));
            go.AddComponent(new Bank(go));

            go.LoadContent();
            AddObject(go);
            return go;
        }

        //Bunkerbuilder is used by the bank to build bunkers.
        public static GameObject BunkerBuilder(Vector2 position)
        {
            GameObject go = new GameObject(position);
            go.AddComponent(new SpriteRender(go, "Pic/Bunker.png", 0));

            go.LoadContent();
            return go;
        }



        /// Keeps track of time and runs the draw method.
        public void GameLoop()
        {
            DateTime startTime = DateTime.Now;
            TimeSpan timeSpan = startTime - endTime;
            int milliseconds = timeSpan.Milliseconds > 0 ? timeSpan.Milliseconds : 1;
            deltaTime = 1000 / milliseconds;
            endTime = DateTime.Now;

            Update();
            Draw();
        }

        /// Updates everything that is not in a thread (bunkers).
        private void Update()
        {
            foreach (GameObject go in bunkers)
            {
                go.Update(deltaTime);
            }
        }

        /// Draws everything in the game, and clears the screen
        private void Draw()
        {
            dc.Clear(Color.DarkOliveGreen);

            lock (bunkerLock)
            {
                foreach (GameObject go in bunkers)
                {
                    go.Draw(dc);
                }
            }

            lock (objectsLock)
            { 
                foreach (GameObject go in objects)
                {
                    go.Draw(dc);
                }
            }

            dc.DrawString(Bank.Balace + "", f, b, 0, 0);

            backBuffer.Render();
        }


        /// Extra methods.
        //RemoveObject and AddObject is used to add/remove thread objects from Objects.
        public static void RemoveObject(GameObject go)
        {
            lock (objectsLock)
            {
                objects.Remove(go);
            }
        }

        public static void AddObject(GameObject go)
        {
            lock (objectsLock)
            {
                objects.Add(go);
            }
        }

        //RemoveCollider and AddCollider is used to add/remove collliders from Colliders.
        public static void RemoveCollider(Collider collider)
        {
            lock (collidersLock)
            {
                Colliders.Remove(collider);
            }
        }

        public static void AddCollider(Collider collider)
        {
            lock (collidersLock)
            {
                Colliders.Add(collider);
            }
        }

        //Used to add a bunkers.
        public static void AddToUpdateList(GameObject go)
        {
            lock (bunkerLock)
            {
                bunkers.Add(go);
            }
        }
    }
}
