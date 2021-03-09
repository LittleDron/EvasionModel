using System;
using System.Collections.Generic;
using System.Text;

namespace TorchShip.Classes
{
    class Ammo
    {
        public Ammo(double startSpeed, double startMass, double endMass, double exhaustVelocity, double massConsumption)
        {
            active = true;
            this.startMass = startMass;
            this.endMass = endMass;
            this.startSpeed = startSpeed;
            this.exhaustVelocity = exhaustVelocity;
            this.massConsumption = massConsumption;
        }

        public Ammo(double startSpeed, double startMass)
        {
            active = false;
            this.startMass = startMass;
            this.startSpeed = startSpeed;
        }

        public void SetHit(double distanse)
        {
            if(active == false)
            {
                activeHit = false;
                hitMass = startMass;
                hitSpeed = startSpeed;
                hitTime = distanse / startSpeed;
            }
            else
            {
                hitMass = startMass;
                hitSpeed = startSpeed;
                hitTime = 0;
                double dT = 0.001;
                double thrust = exhaustVelocity * massConsumption;
                activeHit = true;
                while (hitMass > endMass && distanse>0)
                {
                    hitTime = hitTime + dT;

                    distanse = distanse - hitSpeed * dT;
                    hitSpeed = hitSpeed + thrust * dT / hitMass;
                    hitMass = hitMass - massConsumption * dT;
                }
                if (distanse > 0)
                {
                    hitTime = hitTime + distanse / hitSpeed;
                    activeHit = false;
                }
            }
        }

        public void SetAcceleration()
        {
            if (active == false)
            {
                accelerationTime = 0;
                accelerationDistanse = 0;
            }
            else
            {
                hitMass = startMass;
                hitSpeed = startSpeed;
                double dT = 0.001;
                double thrust = exhaustVelocity * massConsumption;

                accelerationDistanse = 0;
                accelerationTime = 0;

                while (hitMass > endMass)
                {
                    accelerationTime = accelerationTime + dT;

                    accelerationDistanse = accelerationDistanse + hitSpeed * dT;
                    hitSpeed = hitSpeed + thrust * dT / hitMass;
                    hitMass = hitMass - massConsumption * dT;
                }
            }
        }

        public double GetHitMass()
        {
            return hitMass;
        }

        public double GetHitSpeed()
        {
            return hitSpeed;
        }

        public double GetHitTime()
        {
            return hitTime;
        }

        public double GetStartSpeed()
        {
            return startSpeed;
        }

        public bool GetActiveHit()
        {
            return activeHit;
        }

        public bool GetActive()
        {
            return active;
        }

        public double GetAccelerationDistanse()
        {
            return accelerationDistanse;
        }

        public double GetAccelerationTime()
        {
            return accelerationTime;
        }

        bool active, activeHit;
        double startSpeed, startMass, endMass, exhaustVelocity, massConsumption;

        double hitMass, hitSpeed, hitTime;
        double accelerationDistanse, accelerationTime;
    }
}
