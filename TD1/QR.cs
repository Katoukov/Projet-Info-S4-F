using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD1
{
    class QR
    {
        private string phrase;
        private MyPixel[,] image;
        private int[] bin;
        private int[] mode;
        private int[] taillePhrase;
        private int[] masque;
        private Dictionary<char, int> alphabet;

        public QR (string phrase)
        {
            this.phrase = phrase.ToUpper();
            this.taillePhrase = Convert_int_to_binary(phrase.Length, 9);
            if(Convert_binary_to_int(this.taillePhrase)<=25)
            {
                this.mode = Convert_int_to_binary(1,4);
            }
            else
            {
                this.mode = Convert_int_to_binary(2,4);
            }
            this.alphabet = new Dictionary<char, int>()
            {
                { '1', 1 },{ '2', 2 },{ '3', 3 },{ '4', 4 },{ '5', 5 },{ '6', 6 },{ '7', 7 },{ '8', 8 },{ '9', 9 },{ 'A', 10 },{ 'B', 11 },{ 'C', 12 },{ 'D', 13 },{ 'E', 14 },{ 'F', 15 },{ 'G', 16 },{ 'H', 17 },{ 'I', 18 },{ 'J', 19 },{ 'K', 20 },{ 'L', 21 },{ 'M', 22 },{ 'N', 23 },{ 'O', 24 },{ 'P', 25 },{ 'Q', 26 },{ 'R', 27 },{ 'S', 28 },{ 'T', 29 },{ 'U', 30 },{ 'V', 31 },{ 'W', 32 },{ 'X', 33 },{'Y', 34 },{ 'Z', 35 },{ ' ', 36 },{ '$', 37 },{ '%', 38 },{ '*', 39 },{ '+', 40 },{ '-', 41 },{ '.', 42 },{ '/', 43 },{ ':', 44 }
            };
            this.masque = Convert_int_to_binary(30660,15);
            this.bin = Terminaison_octale(Terminaison(Convert_Pairs_to_TotalBinary(Split_in_Pairs(phrase))));
            ReedSolomon_Codage();
            Initialistion_mat();
            Remplissage_mat();
        }
        public MyPixel[,] QRcode
        {
            get { return image; }
        }
        public void ReedSolomon_Codage()
        {
            Encoding u8 = Encoding.UTF8;
            int iBC = u8.GetByteCount(phrase);
            byte[] bytesa = u8.GetBytes(phrase);
            byte[] result = ReedSolomonAlgorithm.Encode(bytesa, 7, ErrorCorrectionCodeType.QRCode);
            int[] codeErreurBin = new int[56];
            for (int i = 0; i < 7; i++)
            {
                int erreur = Convert.ToInt32(result[i]);
                int[] erreurBin = Convert_int_to_binary(erreur, 8);
                for(int j =0;i<8; i++)
                {
                    codeErreurBin[i * 8 + j] = erreurBin[j];
                }
                     
            }
            int[] binBIS = new int[bin.Length + codeErreurBin.Length];
            for (int i = 0; i < bin.Length; i++)
            {
                binBIS[i] = bin[i];
            }
            for (int i = 0; i < codeErreurBin.Length; i++)
            {
                binBIS[bin.Length + i] = codeErreurBin[i]; 
            }
            bin = new int[binBIS.Length];
            for (int i = 0; i < binBIS.Length; i++)
            {
                bin[i] = binBIS[i];
            }
        }
        public void Initialistion_mat()
        {
            if(mode==Convert_int_to_binary(1,4))
            {
                image = new MyPixel[21, 21];
            }
            else
            {
                image = new MyPixel[25, 25];
            }
            for (int i=0; i<image.GetLength(0);i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j].PixelR = 255;
                    image[i, j].PixelG = 255;
                    image[i, j].PixelB = 255;
                }
            }
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    image[i, j].PixelR = 0;
                    image[i, j].PixelG = 0;
                    image[i, j].PixelB = 0;
                    image[i, image.GetLength(1) - j].PixelR = 0;
                    image[i, image.GetLength(1) - j].PixelG = 0;
                    image[i, image.GetLength(1) - j].PixelB = 0;
                    image[image.GetLength(0) - i, j].PixelR = 0;
                    image[image.GetLength(0) - i, j].PixelG = 0;
                    image[image.GetLength(0) - i, j].PixelB = 0;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    image[i+1, j+1].PixelR = 255;
                    image[i+1, j+1].PixelG = 255;
                    image[i+1, j+1].PixelB = 255;
                    image[i+1, image.GetLength(1) - j-1].PixelR = 255;
                    image[i+1, image.GetLength(1) - j-1].PixelG = 255;
                    image[i+1, image.GetLength(1) - j-1].PixelB = 255;
                    image[image.GetLength(0) - i-1, j+1].PixelR = 255;
                    image[image.GetLength(0) - i-1, j+1].PixelG = 255;
                    image[image.GetLength(0) - i-1, j+1].PixelB = 255;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    image[i + 2, j + 2].PixelR = 0;
                    image[i + 2, j + 2].PixelG = 0;
                    image[i + 2, j + 2].PixelB = 0;
                    image[i + 2, image.GetLength(1) - j - 2].PixelR = 0;
                    image[i + 2, image.GetLength(1) - j - 2].PixelG = 0;
                    image[i + 2, image.GetLength(1) - j - 2].PixelB = 0;
                    image[image.GetLength(0) - i - 2, j + 2].PixelR = 0;
                    image[image.GetLength(0) - i - 2, j + 2].PixelG = 0;
                    image[image.GetLength(0) - i - 2, j + 2].PixelB = 0;
                }
            }
            image[image.GetLength(0) - 9, 8].PixelR = 0;
            image[image.GetLength(0) - 9, 8].PixelG = 0;
            image[image.GetLength(0) - 9, 8].PixelB = 0;
            if(mode == Convert_int_to_binary(1, 4))
            {
                for (int i = 0; i < 5; i+=2)
                {
                    image[i,8].PixelR = 0;
                    image[i,8].PixelG = 0;
                    image[i,8].PixelB = 0;
                    image[8, i].PixelR = 0;
                    image[8, i].PixelG = 0;
                    image[8, i].PixelB = 0;
                }
            }
            else
            {
                for (int i = 0; i < 9; i+=2)
                {
                    image[i, 8].PixelR = 0;
                    image[i, 8].PixelG = 0;
                    image[i, 8].PixelB = 0;
                    image[8, i].PixelR = 0;
                    image[8, i].PixelG = 0;
                    image[8, i].PixelB = 0;
                }
            }
        }
        public void Remplissage_mat()
        {
            if (mode == Convert_int_to_binary(1, 4))
            { 
                int compteur = 0;
                for(int i = 0; i<8; i+=2)
                {
                    if(i%4==0)
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            if (bin[compteur] == 1)
                            {
                                image[image.GetLength(0) - j, image.GetLength(1) - i].PixelR = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i].PixelG = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i].PixelB = 0;
                            }
                            compteur++;
                            if (bin[compteur] == 1)
                            {
                                image[image.GetLength(0) - j, image.GetLength(1) - i-1].PixelR = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i-1].PixelG = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i-1].PixelB = 0;
                            }
                            compteur++;
                        }
                    }
                    else
                    {
                        for (int j = 9; j < 21; j++)
                        {
                            if (bin[compteur] == 1)
                            {
                                image[j, image.GetLength(1) - i].PixelR = 0;
                                image[j, image.GetLength(1) - i].PixelG = 0;
                                image[j, image.GetLength(1) - i].PixelB = 0;
                            }
                            compteur++;
                            if (bin[compteur] == 1)
                            {
                                image[j, image.GetLength(1) - i - 1].PixelR = 0;
                                image[j, image.GetLength(1) - i - 1].PixelG = 0;
                                image[j, image.GetLength(1) - i - 1].PixelB = 0;
                            }
                            compteur++;
                        }
                    }
                }
                for (int i = 8; i < 12; i += 2)
                {
                    if (i % 4 == 0)
                    {
                        for (int j = 0; j < 21; j++)
                        {
                            if(j==6)
                            {
                                j++;
                            }
                            if (bin[compteur] == 1)
                            {
                                image[image.GetLength(0) - j, image.GetLength(1) - i].PixelR = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i].PixelG = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i].PixelB = 0;
                            }
                            compteur++;
                            if (bin[compteur] == 1)
                            {
                                image[image.GetLength(0) - j, image.GetLength(1) - i - 1].PixelR = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i - 1].PixelG = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i - 1].PixelB = 0;
                            }
                            compteur++;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 21; j++)
                        {
                            if (j == 6)
                            {
                                j++;
                            }
                            if (bin[compteur] == 1)
                            {
                                image[j, image.GetLength(1) - i].PixelR = 0;
                                image[j, image.GetLength(1) - i].PixelG = 0;
                                image[j, image.GetLength(1) - i].PixelB = 0;
                            }
                            compteur++;
                            if (bin[compteur] == 1)
                            {
                                image[j, image.GetLength(1) - i - 1].PixelR = 0;
                                image[j, image.GetLength(1) - i - 1].PixelG = 0;
                                image[j, image.GetLength(1) - i - 1].PixelB = 0;
                            }
                            compteur++;
                        }
                    }
                }
                for (int i = 12; i < 21; i += 2)
                {
                    if (i % 4 == 0||i%4==3)
                    {
                        for (int j = 9; j < 13; j++)
                        {
                            if (i == 6)
                            {
                                i++;
                            }
                            if (bin[compteur] == 1)
                            {
                                image[image.GetLength(0) - j, image.GetLength(1) - i].PixelR = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i].PixelG = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i].PixelB = 0;
                            }
                            compteur++;
                            if (bin[compteur] == 1)
                            {
                                image[image.GetLength(0) - j, image.GetLength(1) - i - 1].PixelR = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i - 1].PixelG = 0;
                                image[image.GetLength(0) - j, image.GetLength(1) - i - 1].PixelB = 0;
                            }
                            compteur++;
                        }
                    }
                    else
                    {
                        for (int j = 9; j < 13; j++)
                        {
                            if (i== 6)
                            {
                                i++;
                            }
                            if (bin[compteur] == 1)
                            {
                                image[j, image.GetLength(1) - i].PixelR = 0;
                                image[j, image.GetLength(1) - i].PixelG = 0;
                                image[j, image.GetLength(1) - i].PixelB = 0;
                            }
                            compteur++;
                            if (bin[compteur] == 1)
                            {
                                image[j, image.GetLength(1) - i - 1].PixelR = 0;
                                image[j, image.GetLength(1) - i - 1].PixelG = 0;
                                image[j, image.GetLength(1) - i - 1].PixelB = 0;
                            }
                            compteur++;
                        }
                    }
                }
            }
        }
        public List<string> Split_in_Pairs(string phrase)
        {
            List<string> result = new List<string>();

            while (phrase.Length > 0)
            {
                result.Add(new String(phrase.Take(2).ToArray()));
                phrase = new String(phrase.Skip(2).ToArray());
            }

            return result;
        }
        public int[]Convert_Pairs_to_TotalBinary(List<string> pairs)
        {
            int capacityList = pairs.Count();
            int[] result = new int [capacityList*11];
            for(int i = 0; i< capacityList; i++)
            {
                int deci = 0;
                if(pairs[i].Length<2)
                {
                    deci = 45 * alphabet[Convert.ToChar(pairs[i])];
                }
                else
                {
                    char[] charac = pairs[i].ToCharArray();
                    deci = 45 * alphabet[charac[0]] + alphabet[charac[1]];
                }
                int[] bin = Convert_int_to_binary(deci, 11);
                for(int j =0; j<11; j++)
                {
                    result[i * 11 + j] = bin[j];
                }

            }
            return result;   
        }
        public int[] Terminaison(int []bin)
        {
            if(mode == Convert_int_to_binary(1,4))
            {
                if (bin.Length > 149)
                {
                    int[] result = new int[152];
                    for(int i = 0; i<bin.Length; i++)
                    {
                        result[i] = bin[i];
                    }
                    for (int i = bin.Length; i < 152; i++)
                    {
                        result[i] = 0;
                    }
                    return result;
                }
                else
                {
                    int[] result = new int[bin.Length+4];
                    for (int i = 0; i < bin.Length; i++)
                    {
                        result[i] = bin[i];
                    }
                    for (int i = bin.Length; i < bin.Length+4; i++)
                    {
                        result[i] = 0;
                    }
                    if (result.Length % 8 != 0)
                    {
                        int[] resultbis = new int[result.Length + result.Length % 8];
                        for (int i = 0; i < result.Length; i++)
                        {
                            resultbis[i] = result[i];
                        }
                        for (int i = result.Length; i < result.Length + result.Length % 8; i++)
                        {
                            resultbis[i] = 0;
                        }
                        return resultbis;
                    }
                    return result;
                }
                
            }
            else
            {
                if (bin.Length > 269)
                {
                    int[] result = new int[272];
                    for (int i = 0; i < bin.Length; i++)
                    {
                        result[i] = bin[i];
                    }
                    for (int i = bin.Length; i < 272; i++)
                    {
                        result[i] = 0;
                    }
                    return result;
                }
                else
                {
                    int[] result = new int[bin.Length + 4];
                    for (int i = 0; i < bin.Length; i++)
                    {
                        result[i] = bin[i];
                    }
                    for (int i = bin.Length; i < bin.Length + 4; i++)
                    {
                        result[i] = 0;
                    }
                    if(result.Length % 8!= 0)
                    {
                        int[] resultbis = new int[result.Length + result.Length % 8];
                        for (int i = 0; i < result.Length; i++)
                        {
                            resultbis[i] = result[i];
                        }
                        for (int i = result.Length; i < result.Length + result.Length % 8; i++)
                        {
                            resultbis[i] = 0;
                        }
                        return resultbis;
                    }
                    return result;
                }
            }
            
        }
        public int[] Terminaison_octale(int[]bin)
        {
            int[] oct236 = Convert_int_to_binary(236,8);
            int[] oct17 = Convert_int_to_binary(17,8);
            if (mode == Convert_int_to_binary(1,4))
            {
                if(bin.Length!=152)
                {
                    int[] result = new int[152];
                    int limite = (152 - bin.Length) / 8;
                    for(int i = 0; i<limite;i++)
                    {
                        if(i%2==0)
                        {
                            for(int j=0; j<8; j++)
                            {
                                result[bin.Length + i * 8 + j] = oct236[j];
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                result[bin.Length + i * 8 + j] = oct17[j];
                            }
                        }
                    }
                    return result;
                }
                else 
                {
                    return bin;
                }
            }
            else
            {
                if (bin.Length != 272)
                {
                    int[] result = new int[272];
                    int limite = (272 - bin.Length) / 8;
                    for (int i = 0; i < limite; i++)
                    {
                        if (i % 2 == 0)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                result[bin.Length + i * 8 + j] = oct236[j];
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                result[bin.Length + i * 8 + j] = oct17[j];
                            }
                        }
                    }
                    return result;
                }
                else
                {
                    return bin;
                }
            }
        }
        public int[] Convert_int_to_binary(int nombre, int bits)
        {
            int[] a = new int[bits];
            for (int i = 0; nombre > 0; i++)
            {
                a[i] = nombre % 2;
                nombre = nombre / 2;
            }
            int[] result = new int[bits];

            for (int i = 0; i<bits; i++)
            {
                result[i] = a[bits - i - 1];
            }
            return result;
        }
        public int Convert_binary_to_int(int[] binaire)
        {
            int a = 0;  
            int bits = binaire.Length;
            for(int i = 0 ; i< bits ; i++)
            {
                a += binaire[i] * 2 ^ (bits - i - 1);
            }
            return a;
        }
    }
}
