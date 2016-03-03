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

        private static float deltaTime;

        public static List<Collider> Colliders { get; } = new List<Collider>();
        public static List<GameObject> Objects { get; } = new List<GameObject>();

        public static float DeltaTime { get { return deltaTime; } }

        /// sets the value of the variables and properties
        /// Also runs the setupworld method
        public GameWorld(Graphics dc, Rectangle displayRectangle)
        {
            backBuffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);
            this.dc = backBuffer.Graphics;
            threads = new List<Thread>();
            b = Brushes.Gold;
            f = new Font("Arial", 16);
        }

        /// Runs the setup, instantiate lists etc. Puts all the information into the lists/arrays.
        public void SetupWorld()
        {
            CreateBankThread(new Vector2(0, 30));
            CreateCrystalThread(new Vector2(1200, 0));

            for (int i = 0; i < 10; i++)
            {
                CreateWorkerThread(new Vector2(100 + i * 5, 15 + i * 15));
            }
            
            //Loads content.
            foreach (GameObject go in Objects)
            {
                go.LoadContent();
            }
            foreach (Thread t in threads)
            {
                t.Start();
            }
        }
        
        /// Below is our builder methods.
        private void CreateWorkerThread(Vector2 position)
        {
            GameObject worker = WorkerBuilder(position);
            Thread t = new Thread(() => new ThreadHandler(worker));
            t.IsBackground = true;
            threads.Add(t);
        }

        private void CreateBankThread(Vector2 position)
        {
            GameObject bank = BankBuilder(position);
            Thread t = new Thread(() => new ThreadHandler(bank));
            t.IsBackground = true;
            threads.Add(t);
        }

        private void CreateCrystalThread(Vector2 position)
        {
            GameObject crystal = CrystalBuilder(position);
            Thread t = new Thread(() => new ThreadHandler(crystal));
            t.IsBackground = true;
            threads.Add(t);
        }

        private GameObject WorkerBuilder(Vector2 position)
        {
            GameObject go = new GameObject(position);
            go.AddComponent(new SpriteRender(go, "Pic/spritesheet.png", 0));
            go.AddComponent(new Collider(go));

            ///Animator and a component setting up animations are neccesary to make the Animator work.
            ///And the order which they are to be added is: Animator -> Component, to make the animator work.
            go.AddComponent(new Animator(go));
            go.AddComponent(new Worker(go));

            Objects.Add(go);
            return go;
        }

        private GameObject CrystalBuilder(Vector2 position)
        {
            GameObject go = new GameObject(position);
            go.AddComponent(new SpriteRender(go, "Pic/Crystal.png", 0));
            go.AddComponent(new Collider(go));
            go.AddComponent(new Crystal(go));

            Objects.Add(go);
            return go;
        }

        private GameObject BankBuilder(Vector2 position)
        {
            GameObject go = new GameObject(position);
            go.AddComponent(new SpriteRender(go, "Pic/Nexus.png", 0));
            go.AddComponent(new Collider(go));
            go.AddComponent(new Bank(go));

            Objects.Add(go);
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

            //Update(); is called by ThreadHandler.
            Draw();
        }

        /// Draws everything in the game, and clears the screen
        private void Draw()
        {
            dc.Clear(Color.DarkOliveGreen);

            foreach (GameObject go in Objects)
            {
                go.Draw(dc);
            }

            dc.DrawString(Bank.Balace + "", f, b, 0, 0);

            backBuffer.Render();
            
        }
    }
}
