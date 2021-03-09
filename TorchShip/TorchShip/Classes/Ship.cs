using System;
using System.Collections.Generic;
using System.Text;

namespace TorchShip.Classes
{
    class Ship
    {
        public Ship(double a, double e, double l, double w, double h)
        {
            maxA = a;
            maxE = e;
            length = l;
            width = w;
            height = h;
        }

        public double maxA, maxE, length, width, height;
    }
}
