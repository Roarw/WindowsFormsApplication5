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
        private static Graphics dc;
        private static BufferedGraphics backBuffer;
        private DateTime endTime;
        private static float deltaTime;

        public static List<Collider> Colliders { get; } = new List<Collider>();
        public static List<GameObject> Objects { get; } = new List<GameObject>();

        public static float DeltaTime { get { return deltaTime; } }
        public static Graphics DC { get { return dc; } }
        public static BufferedGraphics BackBuffer { get { return backBuffer; } }

        /// sets the value of the variables and properties
        /// Also runs the setupworld method
        public GameWorld(Graphics dc, Rectangle displayRectangle)
        {
            backBuffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);
            GameWorld.dc = backBuffer.Graphics;
        }
        /// Runs the setup, instantiate lists etc. Puts all the information into the lists/arrays.
        public void SetupWorld()
        {

            for (int i = 0; i < 5; i++)
            {
                CreateWorkerThread(new Vector2((100+i * 25), i * 25));
            }

            MakeBank(new Vector2(0, 0));
            MakeCrystal(new Vector2(500, 0));
            MakeCrystal(new Vector2(500, 50));
            MakeCrystal(new Vector2(500, 100));
        }

        private void CreateWorkerThread(Vector2 position)
        {
            Thread t = new Thread(() => new ThreadHandler(CSObject(position)));
            t.IsBackground = true;
            t.Start();
        }

        private GameObject CSObject(Vector2 position)
        {
            GameObject object1 = new GameObject(position);
            object1.AddComponent(new SpriteRender(object1, "Pic/spritesheet.png", 0));
            object1.AddComponent(new Collider(object1));

            ///Animator and a component setting up animations are neccesary to make the Animator work.
            ///And the order which they are to be added is: Animator -> Component, to make the animator work.
            object1.AddComponent(new Animator(object1));
            object1.AddComponent(new Worker(object1));

            object1.LoadContent();
            Objects.Add(object1);

            return object1;
        }

        private void MakeCrystal(Vector2 position)
        {
            GameObject object1 = new GameObject(position);
            object1.AddComponent(new SpriteRender(object1, "Pic/Minerals.png", 0));
            object1.AddComponent(new Collider(object1));

            object1.AddComponent(new Crystal(object1));

            object1.LoadContent();
            Objects.Add(object1);
        }

        private void MakeBank(Vector2 position)
        {
            GameObject object1 = new GameObject(position);
            object1.AddComponent(new SpriteRender(object1, "Pic/Nexus.png", 0));
            object1.AddComponent(new Collider(object1));

            object1.AddComponent(new Bank(object1));

            object1.LoadContent();
            Objects.Add(object1);
        }

        /// Runs the draw method and reads all the button presses.
        public void GameLoop()
        {
            DateTime startTime = DateTime.Now;
            TimeSpan timeSpan = startTime - endTime;
            int milliseconds = timeSpan.Milliseconds > 0 ? timeSpan.Milliseconds : 1;
            deltaTime = 1000 / milliseconds;
            endTime = DateTime.Now;

            //Update() is run by the threads.
            Draw();
        }

        ///// Draws everything in the game, and clears the screen
        private void Draw()
        {
            dc.Clear(Color.White);

            foreach (GameObject go in Objects)
            {
                go.Draw(dc);
            }

            backBuffer.Render();
            
        }
    }
}
