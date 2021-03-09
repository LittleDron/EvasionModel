using System;
using System.Collections.Generic;
using System.Text;

namespace TorchShip.Classes
{
    class Evasion
    {
        public Evasion()
        {
        }

        public double[] NewHit(Ammo ammo, Ship ship, double distanse, bool startByNose, bool endByNose)
        {
            double[] outData;
            ammo.SetHit(distanse);
            if (startByNose)
                outData = DodgingNose(ammo, ship, endByNose);
            else
                outData = Dodging(ammo, ship, endByNose);
            if (ship.maxA * Math.Pow(ammo.GetHitTime(), 2) / 2 < ship.length)
            {
                probabilityHit = 1;
                return outData;
            }
            double shipArea;
            if (endByNose)
                shipArea = ship.width * ship.height;
            else
                shipArea = ship.length * (ship.width + ship.height) / 2;
            if (shipArea > area)
                probabilityHit = 1;
            else
                probabilityHit = shipArea / area;
            return outData;
        }

        double TurnTime(double e, double alfa)
        {
            alfa = alfa / 2;
            return 2 * Math.Sqrt(alfa / e);
        }

        double[] DodgingNose(Ammo ammo, Ship ship, bool endByNose)
        {
            double timeForEvasion = ammo.GetHitTime();
            timeForEvasion = timeForEvasion - TurnTime(ship.maxE, Math.PI / 2);
            if(endByNose)
                timeForEvasion = timeForEvasion - TurnTime(ship.maxE, Math.PI / 2);
            area = 0;
            double[] r = new double[1];
            r[0] = ship.maxA * timeForEvasion * timeForEvasion / 2;
            if (timeForEvasion > 0)
                area = Math.PI * Math.Pow(r[0], 2);
            return r;
        }

        double[] Dodging(Ammo ammo, Ship ship, bool endByNose)
        {
            double timeForEvasion, timeHit, alfa;
            area = 0;
            double[] r = new double[1000];
            timeHit = ammo.GetHitTime();
            for (int i = 0; i < 1000; i++)
            {
                timeForEvasion = timeHit;
                if (i < 500)
                    alfa = i * 2 * Math.PI / 1000;
                else
                    alfa = 2 * Math.PI - i * 2 * Math.PI / 1000;
                timeForEvasion = timeForEvasion - TurnTime(ship.maxE, alfa);
                if (endByNose)
                    timeForEvasion = timeForEvasion - TurnTime(ship.maxE, alfa);
                if (timeForEvasion > 0)
                {
                    r[i] = ship.maxA * timeForEvasion * timeForEvasion / 2;
                    area = area + (Math.PI / 1000) * Math.Pow(r[i], 2);
                }
                else
                    r[i] = 0;
            }
            return r;
        }

        public double GetProbabilityHit()
        {
            return probabilityHit;
        }

        public void NewLineHit(Ammo ammo, Ship ship, double distanse)
        {
            ammo.SetHit(distanse);
            double timeForEvasion = ammo.GetHitTime();
            area = 0;
            double r = ship.maxA * timeForEvasion * timeForEvasion / 2;
            if (r < ship.length)
                probabilityHit = 1;
            else
                probabilityHit = ship.length / r;
        }

        public int GetQHit(bool endByNose, Ship ship)
        {
            double shipArea;
            if (endByNose)
                shipArea = ship.width * ship.height;
            else
                shipArea = ship.length * (ship.width + ship.height) / 2;
            if (shipArea > area)
                return 1;
            else
                return Convert.ToInt32(Math.Ceiling( area / shipArea));
        }

        double area, probabilityHit;
    }
}

