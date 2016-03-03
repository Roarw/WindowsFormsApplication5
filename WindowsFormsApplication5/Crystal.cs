using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class Crystal : Component, ICollisionEnter
    {
        public Crystal(GameObject gameObject) : base(gameObject)
        {

        }

        public void OnCollisionEnter(Collider other)
        {
            Worker w = (Worker)other.GameObject.GetComponent(Components.Worker);

            if (w != null)
            {
                w.Gold = true;
            }
        }
    }
}
