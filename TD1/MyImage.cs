using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace TD1
{



    class MyImage
    {
        int typeFichier;
        int tailleFichier;
        int tailleOffset;
        int bitsDeCouleur;
        int largeur;
        int hauteur;

        MyPixel[,] image;

       
       /// <summary>
       /// lit le fichier bitmap et le transforme en tableau de pixels pour manipulations
       /// </summary>
       /// <param name="namefile"></param>
        public MyImage(string namefile)

        {


            byte[] myfile = File.ReadAllBytes(namefile);


            byte[] typeFichierByte = new byte[4];
            for (int i = 0; i < 2; i++)
            {
                typeFichierByte[i] = myfile[i];
            }
            typeFichier = Convertir_Endian_To_Int(typeFichierByte);
            byte[] tailleFichierByte = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                tailleFichierByte[i] = myfile[i + 2];
            }
            tailleFichier = Convertir_Endian_To_Int(tailleFichierByte);
            byte[] largeurByte = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                largeurByte[i] = myfile[i + 18];
            }
            largeur = Convertir_Endian_To_Int(largeurByte);
            byte[] hauteurByte = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                hauteurByte[i] = myfile[i + 22];
            }
            hauteur = Convertir_Endian_To_Int(hauteurByte);
            byte[] bitsDeCouleurByte = new byte[4];
            for (int i = 0; i < 2; i++)
            {
                bitsDeCouleurByte[i] = myfile[i + 28];
            }
            bitsDeCouleur = Convertir_Endian_To_Int(bitsDeCouleurByte);
            for (int i = 0; i < 4; i++)
            {
                bitsDeCouleurByte[i] = myfile[i + 28];
            }

            int k = 54;
            this.image = new MyPixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    this.image[i, j] = new MyPixel(myfile[k], myfile[k + 1], myfile[k + 2]);
                    k += 3;
                }
            }
        }
        public MyImage( MyPixel[,] matrice)
        {
            this.typeFichier = Convert.ToInt32("BM");
            this.tailleFichier = 54 + matrice.GetLength(0)*3 * matrice.GetLength(1)*3;
            this.Largeur = matrice.GetLength(1)*3;
            this.hauteur = matrice.GetLength(0)*3;
            this.bitsDeCouleur = 24;
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                for (int j = 0; j < matrice.GetLength(1); j++)
                {
                    this.image[i, j].PixelR = matrice[i, j].PixelR;
                    this.image[i, j].PixelG = matrice[i, j].PixelG;
                    this.image[i, j].PixelB = matrice[i, j].PixelB;
                }
            }
        }
       
        public int Hauteur
        {
            get { return hauteur; }
            set { hauteur = value; }

        }

        public int Largeur
        {
            get { return largeur; }
            set { largeur = value; }
        }
        public MyPixel[,] Image
        {
            get { return image; }
            set { image = value; }
        }
        public int TailleFichier
        {
            get { return tailleFichier; }

            set { tailleFichier = value; }
        }




        /// <summary>
        /// transforme le tableau de pixel en image respectant le format bitmap
        /// </summary>
        /// <param name="file"></param>
        public void From_Image_To_File(string file)
        {

            byte[] tailleFichierByte = Convertir_Int_To_Endian(this.tailleFichier);

            byte[] tabResult = new byte[Convertir_Endian_To_Int(tailleFichierByte)];


            tabResult[0] = 66;
            tabResult[1] = 77;

            for (int i = 0; i < 4; i++)
            {
                tabResult[i + 2] = tailleFichierByte[i];
            }

            for (int i = 0; i < 4; i++)
            {
                tabResult[i + 6] = 0;
            }

            byte[] tailleOffsetByte = Convertir_Int_To_Endian(this.tailleOffset);
            for (int i = 0; i < 4; i++)
            {
                tabResult[i + 10] = tailleOffsetByte[i];
            }

            tabResult[14] = 40;
            for (int i = 0; i < 3; i++)
            {
                tabResult[i + 15] = 0;
            }


            byte[] largeurByte = Convertir_Int_To_Endian(this.Largeur);
            for (int i = 0; i < 4; i++)
            {
                tabResult[i + 18] = largeurByte[i];
            }

            byte[] hauteurByte = Convertir_Int_To_Endian(this.Hauteur);
            for (int i = 0; i < 4; i++)
            {
                tabResult[i + 22] = hauteurByte[i];
            }

            tabResult[26] = 1;
            tabResult[27] = 0;

            byte[] bitsDeCouleurByte = Convertir_Int_To_Endian(this.bitsDeCouleur);
            for (int i = 0; i < 4; i++)
            {
                tabResult[i + 28] = bitsDeCouleurByte[i];
            }

            for (int i = 0; i < 2; i++)
            {
                tabResult[i + 32] = 0;
            }
            tabResult[34] = 176;
            tabResult[35] = 4;
            for (int i = 0; i < 18; i++)
            {
                tabResult[i + 36] = 0;
            }



            int k = 0;
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {


                    tabResult[54 + k] = Convert.ToByte(this.Image[i, j].PixelR);
                    tabResult[54 + k + 1] = Convert.ToByte(this.Image[i, j].PixelG);
                    tabResult[54 + k + 2] = Convert.ToByte(this.Image[i, j].PixelB);
                    k += 3;
                }

            }
            File.WriteAllBytes(file, tabResult);


        }
        /// <summary>
        /// Convertis les tableaux de byte (little endian) en int
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int byteToInt = 0;
            for (int i = 0; i < tab.Length; i++)

            {
                byteToInt += tab[i] * (int)Math.Pow(256, i);



            }

            return byteToInt;
        }
        /// <summary>
        /// convertis les int en tableau de byte(little endian)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public byte[] Convertir_Int_To_Endian(int val)
        {

            byte[] tabByte = new byte[4];
            int n = val;
            for (int i = 0; i < 4; i++)
            {

                tabByte[i] = (byte)(n % 256);
                n /= 256;
            }


            return tabByte;
        }
        /// <summary>
        /// fais tourner l'image d'un certain angle en utilisant une matrice de rotation
        /// </summary>
        /// <param name="angle"></param>
        public void Rotation(double angle)
        {

            double angleRadians = (double)angle * Math.PI / 180;
            int h = (Convert.ToInt32(Math.Sin(angleRadians) * this.Largeur + Math.Cos(angleRadians) * this.Hauteur));
            int l = (Convert.ToInt32(Math.Cos(angleRadians) * this.Largeur + Math.Sin(angleRadians) * this.Hauteur));

            if(h < 0) h *= -1;
            if (l < 0) l *= -1;

            MyPixel[,] nouvelleImage = new MyPixel[h, l];



            int coordcentrex = (int)((this.Largeur / 2) - Math.Cos(angleRadians)* (l/2) - Math.Sin(angleRadians)*(h/2));
            int coordcentrey = (int)((this.Hauteur / 2) - Math.Cos(angleRadians) * (h / 2) + Math.Sin(angleRadians) * (l / 2));

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    int coordx = Convert.ToInt32(Math.Cos(angleRadians) * (i - coordcentrex) - (Math.Sin(angleRadians) * (j - coordcentrey)));
                    int coordy = Convert.ToInt32(Math.Sin(angleRadians) * (i - coordcentrex) + (Math.Cos(angleRadians) * (j - coordcentrey)));



                    if (coordx <= l && coordx >= 0 && coordy <= h && coordy >= 0)
                    {

                        nouvelleImage[i, j] = this.Image[coordx, coordy];
                    }
                    else
                    {

                        nouvelleImage[i, j] = new MyPixel(0, 0, 0);
                    }
                }
            }


            this.Image = new MyPixel[h, l];
            this.Hauteur = h;
            this.Largeur = l;
            this.TailleFichier = 54 + (3 * l * h);

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    this.Image[i, j] = nouvelleImage[i, j];
                }
            }
        }
        /// <summary>
        /// fais tourner l'image d'un certain angle en utilisant les coordonnees polaires
        /// </summary>
        /// <param name="angle"></param>
        public void Rotation1(int angle)
        {

            double angleRadians = (double)angle * Math.PI / 180;

            int h = (int)(Math.Sin(angleRadians) * this.Image.GetLength(1) + Math.Cos(angleRadians) * this.Image.GetLength(0));
            int l = (int)(Math.Cos(angleRadians) * this.Image.GetLength(1) + Math.Sin(angleRadians) * this.Image.GetLength(0));

            


            MyPixel[,] nouvelleImageRot = new MyPixel[h, l];

            for (int i = 0; i < h; i++)
            {

                for (int j = 0; j < l; j++)
                {

                    double coordx = j;
                    double coordy = h - i - (Math.Sin(angleRadians) * this.Image.GetLength(1));

                    double rho = Math.Sqrt((coordx * coordx) + (coordy * coordy));
                    double anglePolaire = Math.Atan2(coordy, coordx);

                    double x = rho * Math.Cos(anglePolaire);
                    double y = rho * Math.Sin(anglePolaire);

                    if ((int)(this.Image.GetLength(0) - y) >= 0 && x >= 0 && (int)(this.Image.GetLength(0) - y) < this.Image.GetLength(0) && x < this.Image.GetLength(1))
                    {
                        nouvelleImageRot[i, j] = this.Image[(int)(this.Image.GetLength(0) - y), (int)x];
                    }
                    else
                    {
                        nouvelleImageRot[i, j] = new MyPixel(0, 0, 0);
                    }

                }
            }
         
            this.Image = new MyPixel[h, l];
            this.Hauteur = h;
            this.Largeur = l;
            this.TailleFichier = 54 + (3 * l * h);

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    this.Image[i, j] = nouvelleImageRot[i, j];
                }
            }

        

    }
        /// <summary>
        /// Applique l'effet mirroir
        /// </summary>
        public void EffetMirroir()
        {
            int hauteur = this.Hauteur;
            int largeur = this.Largeur;

            MyPixel[,] nouvelleImage = new MyPixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    nouvelleImage[i, j] = this.Image[i, largeur - 1 - j];

                }
            }
            for(int i = 0; i< hauteur; i++)
            {
                for(int j = 0; j < largeur; j++)
                {
                    this.Image[i, j] = nouvelleImage[i, j];
                }
            }
            
        }

        /// <summary>
        /// Applique l'effet d'agrandissement
        /// </summary>
        public void Agrandir (int coef)
        {

            int h  = this.Hauteur * coef;

            int l = this.Largeur * coef;

            this.TailleFichier = 54 + (3 * h * l);
            
        



            MyPixel[,] nouvelleImage = new MyPixel[h,l];
           

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    nouvelleImage[i, j] = this.Image[i/coef, j/coef];
               
                


                }

            }
            this.Image = new MyPixel[h, l];
           

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    this.Image[i, j] = nouvelleImage[i, j];
                }
            }



        }


        /// <summary>
        /// Applique l'effet de nuances de gris
        /// </summary>
        public void Greyscale()
        {
            int hauteur = this.Hauteur;
            int largeur = this.Largeur;

            MyPixel[,] nouvelleImage = new MyPixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    nouvelleImage[i, j] = this.image[i,j].GreyScale();

                }
            }
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    this.Image[i, j] = nouvelleImage[i, j];
                }
            }

        }


        /// <summary>
        /// appelle la methode de convolution pour appliquer le noyau associe aux detection de contours
        /// </summary>
        public void DetectionDeContours()
        {
            int[,] noyau = { { -1, -1, -1}, { -1,8, -1 }, { -1, -1, -1 } };
            Convolution(noyau);
        }
        /// <summary>
        /// appelle la methode de convolution pour appliquer le noyau associe aux renforcement des bords
        /// </summary>
        public void RenforcementBords()
        {
            int[,] noyau = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            Convolution(noyau);
        }

        /// <summary>
        /// appelle la methode de convolution pour appliquer le noyau associe au floutage
        /// </summary>
        public void Flou()
        {
            int[,] noyau = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            Convolution(noyau);
        }
        /// <summary>
        /// appelle la methode de convolution pour appliquer le noyau associe au repoussage
        /// </summary>
        public void Repoussage()
        {
            int[,] noyau = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            Convolution(noyau);
        }

        /// <summary>
        /// applique la suite de mandelbrot en colorant les points selon le nombre d'iterations
        /// </summary>
        public void Fractale(int iterations)
        {
            int h = this.Hauteur;
            int l = this.Largeur;
            MyPixel[,] imageFractale = new MyPixel[h,l];
            for(int i = 0; i < h; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    double a = (double)(i - (l / 2)) / (double)(l / 4);
                    double b = (double)(j - (l / 2)) / (double)(l / 4);
                    Complex complex = new Complex(a, b);
                    Complex z = new Complex(0, 0);
                    int compteur = 0;


                    while (compteur < iterations)
                    {
                        z.Carre();
                        z.Addition(complex);
                        if (z.Module() > 2) break;
                        compteur++;
    
                }   if(compteur < iterations)
                    {
                        imageFractale[i, j] = new MyPixel(255, 255, 255);
                    }
                    if(compteur % 3 == 0)
                    {
                        imageFractale[i, j] = new MyPixel(150, 50, 75);
                    }
                    else if(compteur%10 == 0)
                    {
                        imageFractale[i, j] = new MyPixel(100, 100, 255);
                    }
                    else
                    {
                        imageFractale[i, j] = new MyPixel(20, 25, 255);
                    }
                    
                }
            }
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    this.Image[i, j] = imageFractale[i, j];
                }
            }

        }

        /// <summary>
        /// code l'image passee en parametre dans celle appelle comme variable d'instance, appelle RassemblerPixels
        /// </summary>
        /// <param name="image2"></param>
        public void CodageImage(MyImage image2)
        {
            int h = this.Hauteur;
            int l = this.Largeur;
            MyPixel[,] nouvelleImage = new MyPixel[h, l];

            for(int i = 0; i < Hauteur; i++)
            {
                for(int j = 0; j < Largeur; j++)
                {

                    nouvelleImage[i,j] = this.image[i,j].RassemblerPixels(image2.image[i, j]);
                   

                }
            }
            
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    this.Image[i, j] = nouvelleImage[i, j];
                }
            }



        }

        /// <summary>
        /// decode l'image passee en variable d'instance, appelle DesassemblerPixels
        /// </summary>
        /// <param name="image2"></param>
        public void DecodageImage()
        {
            int h = this.Hauteur;
            int l = this.Largeur;
            MyPixel[,] nouvelleImage = new MyPixel[h, l];

            for (int i = 0; i < Hauteur; i++)
            {
                for (int j = 0; j < Largeur; j++)
                {

                    nouvelleImage[i, j] = this.Image[i, j].DesassemblerPixels();


                }
            }
            
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    this.Image[i, j] = nouvelleImage[i, j];
                }
            }



        }

        /// <summary>
        /// Creer un tableau du nombres d'occurences de chaque nuances de RGB dans l'image 
        /// </summary>
        public void Histogram()
        {
           
            for (int k = 0; k < 3; k++)
            {
                int[] occurences = new int[256];
                for (int a = 0; a < 256; a++)
                {
                    for (int i = 0; i < Hauteur; i++)
                    {
                        for (int j = 0; j < Largeur; j++)
                        {
                            if (k == 0)
                            {
                                if (image[i, j].PixelR == a)
                                {
                                    occurences[a] += 1;
                                }
                            }
                            if (k == 1)
                            {
                                if (image[i, j].PixelG == a)
                                {
                                    occurences[a] += 1;
                                }
                            }
                            if (k == 2)
                            {
                                if (image[i, j].PixelB == a)
                                {
                                    occurences[a] += 1;
                                }
                            }
                        }
                    }
                }
                if (k == 0)
                {
                    Console.WriteLine("Rouge");
                }
                if (k == 1)
                {
                    Console.WriteLine("Vert");
                }
                if (k == 2)
                {
                    Console.WriteLine("Bleu");
                }
                for (int i = 0; i < 256; i = i + 10)
                {
                    string ligne = "";
                    for (int j = 0; j < 25; j++)
                    {
                        if (occurences[j] < 255 - i)
                        {
                            ligne += ' ';
                        }
                        else
                        {
                            ligne += '|';
                        }
                    }
                    Console.Write(ligne);
                    Console.WriteLine();
                }
            }
            Console.ReadKey();
        }
        public void Histogram1()
        { 
            for (int k = 0; k < 3; k++)
            {
                int[] occurences = new int[256];
                for (int a = 0; a < 256; a++)
                {
                    for (int i = 0; i < Hauteur; i++)
                    {
                        for (int j = 0; j < Largeur; j++)
                        {
                            if (k == 0)
                            {
                                if (image[i, j].PixelR == a)
                                {
                                    occurences[a] += 1;
                                }
                            }
                            if (k == 1)
                            {
                                if (image[i, j].PixelG == a)
                                {
                                    occurences[a] += 1;
                                }
                            }
                            if (k == 2)
                            {
                                if (image[i, j].PixelB == a)
                                {
                                    occurences[a] += 1;
                                }
                            }
                        }
                    }
                }
                if (k == 0)
                {
                    Console.WriteLine("Rouge");
                }
                if (k == 1)
                {
                    Console.WriteLine("Vert");
                }
                if (k == 2)
                {
                    Console.WriteLine("Bleu");
                }
                for (int i = 0; i < 256; i++)
                {
                    string ligne = "";
                    for (int j = 0; j < 255; j++)
                    {
                        if (occurences[j] < 255 - i)
                        {
                            ligne += ' ';
                        }
                        else
                        {
                            ligne += '|';
                        }
                    }
                    Console.Write(ligne);
                    Console.WriteLine();
                }
            }
            Console.ReadKey();
        }


        /// <summary>
        /// applique une matrice de convolution passee en parametre a une image
        /// </summary>
        /// <param name="image2"></param>
        public void Convolution(int[,] noyau)
        {
            int largeurN = noyau.GetLength(0);
            int hauteurN = noyau.GetLength(1);
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    int sommeR = 0;
                    int sommeG = 0;
                    int sommeB = 0;
                    int compteur = 1;
                    for (int k = 0; k < hauteurN; k++)
                    {
                        for (int l = 0; l < largeurN; l++)
                        {
                            int a = i + k;
                            int b = j + l;
                            if (a >= hauteur)
                            {
                                a = i + k - hauteur;
                            }
                            if (b >= largeur)
                            {
                                b = j + l - largeur;
                            }
                            if (a < 0)
                            {
                                a = i + k + hauteur; 
                            }
                            if (b < 0)
                            {
                                b = j + l + largeur;
                            }
                            
                            sommeR += noyau[k, l] * image[a, b].PixelR;
                            sommeG += noyau[k, l] * image[a, b].PixelG;
                            sommeB += noyau[k, l] * image[a, b].PixelB;
                            compteur++;
                        }
                    }
                    sommeR = sommeR / compteur;
                    sommeG = sommeG / compteur;
                    sommeB = sommeB / compteur;
                    if (sommeR > 255) sommeR = 255;
                    if (sommeG > 255) sommeG = 255;
                    if (sommeR > 255) sommeR = 255;
                    if (sommeR < 0) sommeR = 0;
                    if (sommeG < 0) sommeG = 0;
                    if (sommeB < 0) sommeB = 0;
                    this.image[i, j].PixelR = sommeR;
                    this.image[i, j].PixelG = sommeG;
                    this.image[i, j].PixelB = sommeB;
                }
            }
        }
    }

    }
    
