using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Collections;

namespace TD1
{
    class Program
    {
        static void Main(string[] args)
        {

        
           
            int compteur = 0;
            while (compteur == 0)
            {
                Console.WriteLine("Veuillez selectionner le traitement souhaite");
                Console.WriteLine("1 - Traiter une image ");
                Console.WriteLine("2 - Appliquer un filtre ");
                Console.WriteLine("3 - Creer/Extraire une image ");
                Console.WriteLine("4 - QRCODE");


                int choix = Convert.ToInt32(Console.ReadLine());

                if (choix == 1 || choix == 2 || choix == 3 || choix == 4)
                {
                    if (choix == 1)
                    {
                        Console.WriteLine("Veuillez ecrire le nom de l'image bmp: ");
                        string nomImage = Console.ReadLine();
                        MyImage image = new MyImage(nomImage);
                        Process.Start(nomImage);
                        Console.WriteLine("Veuillez selectionner:");
                        Console.WriteLine("1 - Nuance de gris");
                        Console.WriteLine("2 - Agrandir une image:");
                        Console.WriteLine("3 - Rotation");
                        Console.WriteLine("4 - Effet Mirroir");
                        int choix2 = Convert.ToInt32(Console.ReadLine());
                        if (choix2 == 1) { image.Greyscale(); image.From_Image_To_File("Greyscale.bmp"); Process.Start("Greyscale.bmp"); }
                        if (choix2 == 2) { Console.WriteLine("Veuillez selectionner coeff:"); int coeff = Convert.ToInt32(Console.ReadLine()); image.Agrandir(coeff); image.From_Image_To_File("Agrandir.bmp"); Process.Start("Agrandir.bmp"); }
                        if (choix2 == 3) { Console.WriteLine("Veuillez selectionner angle:"); int angle = Convert.ToInt32(Console.ReadLine()); image.Rotation(angle); image.From_Image_To_File("Rotation.bmp"); Process.Start("Rotation.bmp"); }
                        if (choix2 == 4) { image.EffetMirroir(); image.From_Image_To_File("EffetMirroir.bmp"); Process.Start("EffetMirroir.bmp"); }
                        Console.WriteLine("Faire une autre operation? oui = 0, non = 1");
                        int choix3 = Convert.ToInt32(Console.ReadLine());
                        if (choix3 == 1)
                        {
                            compteur = 1;
                        }
                        else
                        {
                            compteur = 0;
                        }
                    }
                    if (choix == 2)
                    {
                        Console.WriteLine("Veuillez ecrire le nom de l'image bmp: ");
                        string nomImage = Console.ReadLine();
                        MyImage image = new MyImage(nomImage);
                        Process.Start(nomImage);
                        Console.WriteLine("Veuillez selectionner:");
                        Console.WriteLine("1 - Detection de contours");
                        Console.WriteLine("2 - Renforcement des bords");
                        Console.WriteLine("3 - Flou");
                        Console.WriteLine("4 - Repoussage");
                        int choix2 = Convert.ToInt32(Console.ReadLine());
                        if (choix2 == 1) { image.DetectionDeContours(); image.From_Image_To_File("DetectionContours.bmp"); Process.Start("DetectionContours.bmp"); }
                        if (choix2 == 2) { image.RenforcementBords(); image.From_Image_To_File("RenforcementBords.bmp"); Process.Start("RenforcementBords.bmp"); }
                        if (choix2 == 3) { image.Flou(); image.From_Image_To_File("Flou.bmp"); Process.Start("Flou.bmp"); }
                        if (choix2 == 4) { image.Repoussage(); image.From_Image_To_File("Repoussage.bmp"); Process.Start("Repoussage.bmp"); }
                        Console.WriteLine("Faire une autre operation? oui = 0, non = 1");
                        int choix3 = Convert.ToInt32(Console.ReadLine());
                        if (choix3 == 1)
                        {
                            compteur = 1;
                        }
                        else
                        {
                            compteur = 0;
                        }

                    }
                    if (choix == 3)
                    {
                        Console.WriteLine("Veuillez ecrire le nom de l'image bmp: ");
                        string nomImage = Console.ReadLine();
                        MyImage image = new MyImage(nomImage);
                        Process.Start(nomImage);
                        Console.WriteLine("Veuillez selectionner:");
                        Console.WriteLine("1 - Fractale");
                        Console.WriteLine("2 - Histogramme");
                        Console.WriteLine("3 - Coder une image");
                        Console.WriteLine("4 - Decoder une image (Decodage de derniere image codee");
                        int choix2 = Convert.ToInt32(Console.ReadLine());
                        if (choix2 == 1) { Console.WriteLine("Veuillez selectionner nb d'iterations:"); int iterations = Convert.ToInt32(Console.ReadLine()); image.Fractale(iterations); image.From_Image_To_File("Fractale.bmp"); Process.Start("Fractale.bmp"); }
                        if (choix2 == 2) { image.Histogram(); Console.ReadKey(); }
                        if (choix2 == 3) { Console.WriteLine("Veuillez selectionner image a coder dans: " + nomImage +" image doit etre plus petite ou de meme taille"); string nomImageCode = Console.ReadLine(); MyImage imageCode = new MyImage(nomImageCode); image.CodageImage(imageCode); image.From_Image_To_File("ImageCodee.bmp"); Process.Start("ImageCodee.bmp"); }
                        if (choix2 == 4) { Console.WriteLine("Decodage de derniere image codee"); MyImage imageCodee2 = new MyImage("ImageCodee.bmp"); imageCodee2.DecodageImage(); imageCodee2.From_Image_To_File("ImageDecodee.bmp"); Process.Start("ImageDecodee.bmp"); }
                        Console.WriteLine("Faire une autre operation? oui = 0, non = 1");
                        int choix3 = Convert.ToInt32(Console.ReadLine());
                        if (choix3 == 1)
                        {
                            compteur = 1;
                        }
                        else
                        {
                            compteur = 0;
                        }

                    }
                    if (choix == 4)
                    {
                        Console.WriteLine("Veuillez ecrire votre phrase ");
                        string phrase = Console.ReadLine();
                        QR qr = new QR(phrase);
                        MyImage qrCode = new MyImage(qr.QRcode);
                        qrCode.From_Image_To_File("QRcode.bmp"); Process.Start("QRcode.bmp");
                        Console.WriteLine("Faire une autre operation? oui = 0, non = 1");
                        int choix3 = Convert.ToInt32(Console.ReadLine());
                        if (choix3 == 1)
                        {
                            compteur = 1;
                        }
                        else
                        {
                            compteur = 0;
                        }
                    }
                }

                else
                {
                    Console.WriteLine("Pas un choix valable");
                    Console.ReadKey();
                }
            }
            Console.WriteLine("Appuyez sur n'importe quelle touche pour sortir de la console");
            Console.ReadKey();
            
        }

       

    }
}
