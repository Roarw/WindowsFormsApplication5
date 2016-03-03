using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication5
{
    class Component
    {
        protected GameObject gameObject;

        public GameObject GameObject { get { return gameObject; } }

        public Component(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public Component()
        {

        }
    }
}
