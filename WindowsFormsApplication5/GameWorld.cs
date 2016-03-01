using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApplication5
{
    class GameWorld
    {
        private Graphics dc;
        private BufferedGraphics backBuffer;
        private DateTime endTime;
        private float deltaTime;

        public static List<Collider> Colliders { get; } = new List<Collider>();
        public static List<GameObject> Objects { get; } = new List<GameObject>();

        /// sets the value of the variables and properties
        /// Also runs the setupworld method
        public GameWorld(Graphics dc, Rectangle displayRectangle)
        {
            this.dc = dc;
            backBuffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);
            this.dc = backBuffer.Graphics;
        }
        // /// Runs the setup, instantiate lists etc. Puts all the information into the lists/arrays.
        public void SetupWorld()
        {
            CSObject(new Vector2(10, 10));
            CSObject(new Vector2(0, 0));
        }

        private void CSObject(Vector2 position)
        {
            GameObject object1 = new GameObject(position);
            object1.AddComponent(new SpriteRender(object1, "Orc.png", 0));
            object1.AddComponent(new Collider(object1));

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

            Update(deltaTime);
            Draw();
        }

        private void Update(float deltaTime)
        {
            foreach (GameObject go in Objects)
            {
                go.Update(deltaTime);
            }
        }

        /// Draws everything in the game, and clears the screen
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
