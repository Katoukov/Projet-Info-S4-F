using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD1
{
    class MyPixel
    {
        private int red;
        private int green;
        private int blue;
      


        public MyPixel(int red, int green, int blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
           

        }
        /// <summary>
        /// Nuance de gris, poids de chaque couleur trouve sur internet
        /// </summary>
        /// <returns></returns>
        public MyPixel GreyScale()
        {
            int grey = (int)(0.2126 * this.red + 0.7152 * this.green + 0.0722 * this.blue);
            MyPixel GreyPixel = new MyPixel(grey, grey, grey);
            return GreyPixel;
        }


      
        public int PixelG
        {
            get { return green; }
            set { green = value; }
        }
        public int PixelB
        {
            get { return blue; }
            set { blue = value; }
        }

        public int PixelR
        {
            get { return red; }
            set { red = value; }
        }

       /// <summary>
       /// Prend les 4 premiers bits de la premiere image et les 4 derniers de la deuxieme image et en fait un seul pixel
       /// </summary>
       /// <param name="pixel"></param>
       /// <returns></returns>
        public MyPixel RassemblerPixels(MyPixel pixel)
        {
            MyPixel PixelFinal = new MyPixel(0,0,0);

            string R2 = Convert.ToString(pixel.PixelR, 2).PadLeft(8, '0');
            string G2 = Convert.ToString(pixel.PixelG, 2).PadLeft(8, '0');
            string B2 = Convert.ToString(pixel.PixelB, 2).PadLeft(8, '0');

            string R1 = Convert.ToString(this.PixelR, 2).PadLeft(8, '0');
            string G1 = Convert.ToString(this.PixelG, 2).PadLeft(8, '0');
            string B1 = Convert.ToString(this.PixelB, 2).PadLeft(8, '0');

                string Rfinal = R1.Substring(0,4) + R2.Substring(0,4);
                string Gfinal = G1.Substring(0, 4) + G2.Substring(0, 4);
            string Bfinal = B1.Substring(0, 4) + B2.Substring(0, 4);

            PixelFinal.PixelR = Convert.ToInt32(Rfinal, 2);
            PixelFinal.PixelG = Convert.ToInt32(Gfinal, 2);
            PixelFinal.PixelB = Convert.ToInt32(Bfinal, 2);
            return PixelFinal;
        }
        /// <summary>
        /// Prend les 4 premiers bits de l'image encodee et rempli de 0 
        /// </summary>
        /// <returns></returns>
        public MyPixel DesassemblerPixels()
        {
            MyPixel PixelFinal = new MyPixel(0, 0, 0);

            string R = Convert.ToString(this.PixelR, 2).PadLeft(8, '0');
            string G = Convert.ToString(this.PixelG, 2).PadLeft(8, '0');
            string B = Convert.ToString(this.PixelB, 2).PadLeft(8, '0');

            string Rfinal = R.Substring(4, 4) + "0"+"0"+ "0" + "0";
            string Gfinal = G.Substring(4, 4) + "0" + "0" + "0" + "0";
            string Bfinal = B.Substring(4, 4) + "0" + "0" + "0" + "0";

            PixelFinal.PixelR = Convert.ToInt32(Rfinal, 2);
            PixelFinal.PixelG = Convert.ToInt32(Gfinal, 2);
            PixelFinal.PixelB = Convert.ToInt32(Bfinal, 2);

            return PixelFinal;

        }
      
        


    }
}
