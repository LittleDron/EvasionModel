using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TorchShip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Classes.Ammo ammo = CreatAmmo();
            Classes.Ship ship = CreatShip();

            if (ammo != null && ship != null)
                if (hitProbability.Checked)
                {
                    ammo.SetAcceleration();
                    accelerationDistanse.Text = ammo.GetAccelerationDistanse().ToString();
                    accelerationTime.Text = ammo.GetAccelerationTime().ToString();

                    Graph graph = new Graph(ammo, ship);
                    graph.GetProbabilityImage(pictureBox1);
                    graph.GetAmmoImage(pictureBox2);
                }
                else
                {
                    double distanse, speed, corner;
                    Classes.Volley volley;
                    Classes.Evasion evasion;
                    try
                    {
                        distanse = Convert.ToDouble(shipDistanse.Text);
                        speed = Convert.ToDouble(shipSpeed.Text);
                        corner = Convert.ToDouble(shipCorner.Text);
                    }
                    catch
                    {
                        return;
                    }
                    try
                    {
                        volley = new Classes.Volley(ammo, speed, corner, distanse);
                        evasion = new Classes.Evasion();
                        evasion.NewHit(ammo, ship, volley.GetHitDistansy(), startByNose.Checked, endByNose.Checked);

                        SpeedHit.Text = volley.GetHitSpeed().ToString();
                        EnergyHit.Text = (Math.Pow(volley.GetHitSpeed(), 2) * ammo.GetHitMass() / (2 * 4.184e9)).ToString();
                        DistanseHit.Text = volley.GetHitDistansy().ToString();
                        TimeHit.Text = ammo.GetHitTime().ToString();
                        if (activeAmmo.Checked)
                            MassHit.Text = (ammo.GetHitMass() - Convert.ToDouble(finalMass.Text)).ToString();
                        else
                            MassHit.Text = "0";
                        probabilityHit.Text = evasion.GetProbabilityHit().ToString();
                        QHit.Text = evasion.GetQHit(endByNose.Checked, ship).ToString();
                    }
                    catch
                    {
                        return;
                    }
                }

            /*Deflection deflection;
            Evasion evasion;
            Ship ship;
            try
            {
                deflection = new Deflection(Convert.ToDouble(shipEnemyDistance.Text), Convert.ToDouble(ammoSpeed.Text));
                ship = new Ship(Convert.ToDouble(width.Text), Convert.ToDouble(height.Text), Convert.ToDouble(this.shipSpeed.Text), Convert.ToDouble(maxAcceleration), Convert.ToDouble(maxAngularAcceleration.Text));
                evasion = new Evasion(deflection, ship, true, true);
            }
            catch
            {

            }
            finally
            {

            }*/

        }

        Classes.Ammo CreatAmmo()
        {
            double startSpeed, startMass, endMass, exhaustVelocity, massConsumption;
            try
            {
                startSpeed = Convert.ToDouble(ammoSpeed.Text);
            }
            catch
            {
                Console.WriteLine("начальная скорость снаряда");
                return null;
            }
            try
            {
                startMass = Convert.ToDouble(initialMass.Text);
            }
            catch
            {
                Console.WriteLine("начальная масса снаряда");
                return null;
            }

            if (activeAmmo.Checked)
            {
                try
                {
                    endMass = Convert.ToDouble(finalMass.Text);
                }
                catch
                {
                    Console.WriteLine("конечная масса снаряда");
                    return null;
                }
                try
                {
                    exhaustVelocity = Convert.ToDouble(this.exhaustVelocity.Text);
                }
                catch
                {
                    Console.WriteLine("скорость истечения");
                    return null;
                }
                try
                {
                    massConsumption = Convert.ToDouble(this.massConsumption.Text);
                }
                catch
                {
                    Console.WriteLine("потребление массы");
                    return null;
                }
                return new Classes.Ammo(startSpeed, startMass, endMass, exhaustVelocity, massConsumption);
            }
            else
                return new Classes.Ammo(startSpeed, startMass);
        }
        
        Classes.Ship CreatShip()
        {
            double maxA, maxE, length, width, height;

            try
            {
                maxA = Convert.ToDouble(maxAcceleration.Text);
            }
            catch
            {
                Console.WriteLine("максимальне ускорение");
                return null;
            }
            try
            {
                maxE = Convert.ToDouble(maxAngularAcceleration.Text);
            }
            catch
            {
                Console.WriteLine("максимальное угловое ускорение");
                return null;
            }
            try
            {
                length = Convert.ToDouble(this.length.Text);
            }
            catch
            {
                Console.WriteLine("длина");
                return null;
            }
            try
            {
                width = Convert.ToDouble(this.width.Text);
            }
            catch
            {
                Console.WriteLine("ширина");
                return null;
            }
            try
            {
                height = Convert.ToDouble(this.height.Text);
            }
            catch
            {
                Console.WriteLine("высота");
                return null;
            }
            return new Classes.Ship(maxA, maxE, length, width, height);
        }

        private void hitProbability_CheckedChanged(object sender, EventArgs e)
        {
            shipSpeed.ReadOnly = true;
            shipCorner.ReadOnly = true;
            shipDistanse.ReadOnly = true;
        }

        private void numberOfShots_CheckedChanged(object sender, EventArgs e)
        {
            shipSpeed.ReadOnly = false;
            shipCorner.ReadOnly = false;
            shipDistanse.ReadOnly = false;
        }

        private void activeAmmo_CheckedChanged(object sender, EventArgs e)
        {
            if (activeAmmo.Checked)
                mass.Text = "Начальная масса кг";
            else
                mass.Text = "Масса снаряда кг";
            bool readOnly = !activeAmmo.Checked;
            finalMass.ReadOnly = readOnly;
            exhaustVelocity.ReadOnly = readOnly;
            massConsumption.ReadOnly = readOnly;
        }

        
    }
}
