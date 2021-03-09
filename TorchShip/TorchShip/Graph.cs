using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TorchShip
{
    class Graph
    {
        public Graph(Classes.Ammo ammo, Classes.Ship ship)
        {
            SetGraph(ammo, ship);
        }

        void SetGraph(Classes.Ammo ammo, Classes.Ship ship)
        {
            speedAmmo = new double[1];
            speedAmmo[0] = ammo.GetStartSpeed();
            activeAmmo = new bool[1];
            activeAmmo[0] = ammo.GetActive();

            probabilityHits = new double[4][];
            probabilityHits[0] = SetProbabilityHit(ammo, ship, true, true);
            probabilityHits[1] = SetProbabilityHit(ammo, ship, true, false);
            probabilityHits[2] = SetProbabilityHit(ammo, ship, false, true);
            probabilityHits[3] = SetProbabilityHit(ammo, ship, false, false);

            length = probabilityHits[0].Length;
            length = Math.Max(length, probabilityHits[1].Length);
            length = Math.Max(length, probabilityHits[2].Length);
            length = Math.Max(length, probabilityHits[3].Length);
        }

        double[] SetProbabilityHit(Classes.Ammo ammo, Classes.Ship ship, bool startByNose, bool endByNose)
        {
            int distansy = 0;
            double[] probabilityHit = new double[1];
            probabilityHit[0] = 1;
            Classes.Evasion evasion = new Classes.Evasion();
            do
            {
                distansy++;
                evasion.NewHit(ammo, ship, distansy * 1000, startByNose, endByNose);
                probabilityHit = AddProbability(probabilityHit, evasion.GetProbabilityHit());

                if (distansy >= speedAmmo.Length)
                {
                    speedAmmo = AddProbability(speedAmmo, ammo.GetHitSpeed());
                    activeAmmo = AddBool(activeAmmo, ammo.GetActiveHit());
                }
            } while (probabilityHit[distansy] > 0.05);
            return probabilityHit;
        }

        double[] AddProbability(double[] probability, double addProbability)
        {
            double[] newProbability = new double[probability.Length + 1];
            for (int i = 0; i < probability.Length; i++)
                newProbability[i] = probability[i];
            newProbability[probability.Length] = addProbability;
            return newProbability;
        }

        bool[] AddBool(bool[] probability, bool addProbability)
        {
            bool[] newProbability = new bool[probability.Length + 1];
            for (int i = 0; i < probability.Length; i++)
                newProbability[i] = probability[i];
            newProbability[probability.Length] = addProbability;
            return newProbability;
        }

        //------------------------------------------------------------

        public void GetProbabilityImage(PictureBox pictureBox)
        {
            Graphics image = pictureBox.CreateGraphics();
            SetBackground(image, pictureBox);
            
            Pen[] pens = new Pen[5];
            pens[0] = new Pen(Color.Brown);
            pens[1] = new Pen(Color.Red);
            pens[2] = new Pen(Color.Green);
            pens[3] = new Pen(Color.Blue);

            for (int i = 0; i<length; i++)
                for(int j = 0; j<4; j++)
                    SetPoint(i, j, pictureBox, image, pens);
        }

        void SetPoint(int x, int ID, PictureBox pictureBox, Graphics image, Pen[] pens)
        {
            if (x + 1 < probabilityHits[ID].Length)
            {
                image.DrawLine(pens[ID], x * pictureBox.Width / length, Convert.ToInt32(pictureBox.Height * (1 - probabilityHits[ID][x])), (x + 1) * pictureBox.Width / length, Convert.ToInt32(pictureBox.Height * (1 - probabilityHits[ID][x + 1])));
            }
        }

        void SetBackground(Graphics image, PictureBox pictureBox)
        {
            image.Clear(Color.White);

            Pen pen = new Pen(Color.Black);
            Brush brush = new SolidBrush(Color.Black);
            Font font = new Font("Arial Black", 6, FontStyle.Regular, GraphicsUnit.Point);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;


            image.DrawLine(pen, 0, pictureBox.Height / 10, pictureBox.Width, pictureBox.Height / 10);
            image.DrawLine(pen, 0, pictureBox.Height * 9 / 10, pictureBox.Width, pictureBox.Height * 9 / 10);
            image.DrawLine(pen, 0, pictureBox.Height / 4, pictureBox.Width, pictureBox.Height / 4);
            image.DrawLine(pen, 0, pictureBox.Height * 3 / 4, pictureBox.Width, pictureBox.Height * 3 / 4);
            image.DrawLine(pen, 0, pictureBox.Height / 3, pictureBox.Width, pictureBox.Height / 3);
            image.DrawLine(pen, 0, pictureBox.Height * 2 / 3, pictureBox.Width, pictureBox.Height * 2 / 3);
            image.DrawLine(pen, 0, pictureBox.Height / 2, pictureBox.Width, pictureBox.Height / 2);

            image.DrawString("0%", font, brush, 0, pictureBox.Height - 20);
            image.DrawString("10%", font, brush, 0, pictureBox.Height * 9 / 10 - 20);
            image.DrawString("25%", font, brush, 0, pictureBox.Height * 3 / 4 - 20);
            image.DrawString("33%", font, brush, 0, pictureBox.Height * 2 / 3 - 20);
            image.DrawString("50%", font, brush, 0, pictureBox.Height / 2 - 20);
            image.DrawString("66%", font, brush, 0, pictureBox.Height / 3 - 20);
            image.DrawString("75%", font, brush, 0, pictureBox.Height / 4 - 20);
            image.DrawString("90%", font, brush, 0, pictureBox.Height / 10 - 20);
            image.DrawString("100%", font, brush, 0, 0);

            double size = 1.0 * length / pictureBox.Width;
            int step = 1;
            while (step / size < 50)
                step = step * 10;
            int xP = Convert.ToInt32(step / size);
            int xKm = step;
            while (xP < pictureBox.Width)
            {
                image.DrawLine(pen, xP, 0, xP, pictureBox.Height);
                image.DrawString(xKm.ToString(), font, brush, xP, pictureBox.Height - 20);

                xP = xP + Convert.ToInt32(step / size);
                xKm = xKm + step;
            }
        }

        //----------------------------------------------------------------

        public void GetAmmoImage(PictureBox pictureBox)
        {
            double minSpeed = double.MaxValue;
            double maxSpeed = double.MinValue;
            for (int i = 0; i < length; i++)
            {
                if (minSpeed > speedAmmo[i])
                    minSpeed = speedAmmo[i];
                if (maxSpeed < speedAmmo[i])
                    maxSpeed = speedAmmo[i];
            }

            Graphics image = pictureBox.CreateGraphics();
            SetAmmoBackground(image, pictureBox, maxSpeed + 1000, minSpeed - 1000);

            Pen[] pens = new Pen[2];
            pens[0] = new Pen(Color.Blue);
            pens[1] = new Pen(Color.Green, 4);

            for (int i = 0; i < length - 1; i++)
                SetAmmoPoint(i, pictureBox, image, pens, maxSpeed + 1000, minSpeed - 1000);
        }

        void SetAmmoPoint(int x, PictureBox pictureBox, Graphics image, Pen[] pens, double max, double min)
        {
            image.DrawLine(pens[0], x * pictureBox.Width / length, Convert.ToInt32(pictureBox.Height * (1 - (speedAmmo[x] - min) / (max - min))), (x + 1) * pictureBox.Width / length, Convert.ToInt32(pictureBox.Height * (1 - (speedAmmo[x + 1] - min) / (max - min))));
            if(activeAmmo[x+1])
            image.DrawLine(pens[1], x * pictureBox.Width / length, pictureBox.Height, (x + 1) * pictureBox.Width / length, pictureBox.Height);
        }

        void SetAmmoBackground(Graphics image, PictureBox pictureBox, double max, double min)
        {
            image.Clear(Color.White);

            Pen pen = new Pen(Color.Black);
            Brush brush = new SolidBrush(Color.Black);
            Font font = new Font("Arial Black", 6, FontStyle.Regular, GraphicsUnit.Point);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            double size = 1.0 * (max - min) / pictureBox.Height;
            double step = 1;
            while (step / size < 20)
                step = step * 10;
            double start = max / size;

            double xP = start;
            double xKm = 0;
            while (xP > 0)
            {
                image.DrawLine(pen, 0, Convert.ToInt32(xP), pictureBox.Width, Convert.ToInt32(xP));
                image.DrawString(Convert.ToInt32(xKm).ToString(), font, brush, 0, Convert.ToInt32(xP) - 20);

                xP = xP - step / size;
                xKm = xKm + step;
            }

            size = 1.0 * length / pictureBox.Width;
            step = 1;
            while (step / size < 50)
                step = step * 10;
            xP = step / size;
            xKm = step;
            while (xP < pictureBox.Width)
            {
                image.DrawLine(pen, Convert.ToInt32(xP), 0, Convert.ToInt32(xP), pictureBox.Height);
                image.DrawString(xKm.ToString(), font, brush, Convert.ToInt32(xP), pictureBox.Height - 20);

                xP = xP + step / size;
                xKm = xKm + step;
            }
        }

        int length;
        double[][] probabilityHits;
        double[] speedAmmo;
        bool[] activeAmmo;
    }
}
