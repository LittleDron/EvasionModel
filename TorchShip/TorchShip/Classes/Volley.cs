using System;
using System.Collections.Generic;
using System.Text;

namespace TorchShip.Classes
{
    class Volley
    {
        public Volley(Ammo ammo, double shipSpeed, double shipCorner, double shipDistanse)
        {
            this.shipSpeed = shipSpeed;
            this.shipCorner = Math.PI * shipCorner / 180;
            this.shipDistanse = shipDistanse;
            SetHitDistansy(ammo);
        }

        void SetHitDistansy(Ammo ammo)
        {
            ammo.SetHit(shipDistanse);

            double x = 0, y = 0, dx, dy;
            dx = -shipSpeed * Math.Cos(shipCorner);
            dy = shipSpeed * Math.Sin(shipCorner);

            for (int i=0; i<1000; i++)
            {
                x = shipDistanse + dx * ammo.GetHitTime();
                y = dy * ammo.GetHitTime();
                hitDistanse = Math.Sqrt(x * x + y * y);
                ammo.SetHit(hitDistanse);
            }
            SetHitSpeed(ammo, x, y);
        }

        void SetHitSpeed(Ammo ammo, double x, double y)
        {
            double shipX, shipY, ammoX, ammoY, m;

            shipX = x - shipDistanse;
            shipY = y;
            m = Math.Sqrt(shipX * shipX + shipY * shipY);
            if (m > 0)
            {
                shipX = shipX * shipSpeed / m;
                shipY = shipY * shipSpeed / m;
            }

            ammoX = x;
            ammoY = y;
            m = Math.Sqrt(ammoX * ammoX + ammoY * ammoY);
            if (m > 0)
            {
                ammoX = ammoX * ammo.GetHitSpeed() / m;
                ammoY = ammoY * ammo.GetHitSpeed() / m;
            }

            ammoX = ammoX - shipX;
            ammoY = ammoY - shipY;
            hitSpeed = Math.Sqrt(ammoX * ammoX + ammoY * ammoY);
        }

        public double GetHitDistansy()
        {
            return hitDistanse;
        }

        public double GetHitSpeed()
        {
            return hitSpeed;
        }

        double shipSpeed, shipCorner, shipDistanse;

        double hitDistanse, hitSpeed;
    }
}
