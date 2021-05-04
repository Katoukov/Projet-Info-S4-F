using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD1
{/// <summary>
/// classe pour definir les operations de bases dans les complexes pour fractale
/// </summary>
    class Complex
    {
        public double a;
        public double b;

        public Complex(double a, double b)
        {
            this.a = a;
            this.b = b;
        }


        public double Module()
        {
            return Math.Sqrt((a * a) + (b * b));

        }

        public void Carre()
        {

            double vartemp = (a * a) - (b * b);
            b = 2 * a * b;
            a = vartemp;
        }

        public void Addition(Complex c)
        {
            a += c.a;
            b += c.b;
        }

      
    }
}
