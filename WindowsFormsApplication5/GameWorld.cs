using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace GamwWorld
{
    class GameWorld
    {
        private Graphics dc;
        private BufferedGraphics backBuffer;

        /// sets the value of the variables and properties
        /// Also runs the setupworld method
        public GameWorld(Graphics dc, Rectangle displayRectangle)
        {
            this.dc = dc;
            backBuffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);
            this.dc = backBuffer.Graphics;
            SetupWorld();
        }
        // /// Runs the setup, instantiate lists etc. Puts all the information into the lists/arrays.
        public void SetupWorld()
        {


        }
        /// Runs the draw method and reads all the button presses.
        public void GameLoop()
        {

        }
        /// Draws everything in the game, and clears the screen
        private void Draw()
        {
            dc.Clear(Color.White);
        }
    }
}
