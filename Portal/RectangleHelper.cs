using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame
{
    public class RectangleHelper
    {
        public bool TouchTopOf(Rectangle r1, Rectangle r2)
        {
            return (r1.Bottom >= r2.Top - 5 &&
                    r1.Bottom <= r2.Top + 5 &&
                    r1.Right >= r2.Left &&
                    r1.Left <= r2.Right); 
        }

        public bool TouchBottomOf(Rectangle r1, Rectangle r2)
        {
            return (r1.Top <= r2.Bottom + 10 &&
                    r1.Top >= r2.Bottom - 10 &&
                    r1.Right >= r2.Left + 7 &&
                    r1.Left <= r2.Right - 7); 
        }

        public bool TouchLeftOf(Rectangle r1, Rectangle r2)
        {
            return (r1.Right <= r2.Left + 5 &&
                    r1.Right >= r2.Left - 5 &&
                    r1.Top <= r2.Bottom - 5 &&
                    r1.Bottom >= r2.Top + 5);
        }

        public bool TouchRightOf(Rectangle r1, Rectangle r2)
        {
            return (r1.Left >= r2.Right - 5 &&
                    r1.Left <= r2.Right + 5 &&
                    r1.Top <= r2.Bottom - 5 &&
                    r1.Bottom >= r2.Top + 5);
        }
    }
}
