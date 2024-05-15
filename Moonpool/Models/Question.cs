using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;

namespace Moonpool.Models
{
    public class Question
    {
        public BitmapImage? image;
        public string? answer;
        private uint totalSolvedNumber;
        private uint numberOfCorrect;
        public string? imageHash;
        public decimal weight;

        private static byte[] GetImageByte(string imagePath)
        {
            byte[] imageBytes;
            using (FileStream fileStream = new(imagePath, FileMode.Open, FileAccess.Read))
            using (BufferedStream bufferedStream = new(fileStream))
            {
                using (MemoryStream memoryStream = new())
                {
                    bufferedStream.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }
            return imageBytes;
        }

        private static string GetImageHash(byte[] imageBytes)
        {
            string hashString;
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(imageBytes);
                hashString = BitConverter.ToString(hashBytes).Replace("-", "");
            }
            return hashString;
        }

        private static BitmapImage GetImage(byte[] imageBytes)
        {
            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public void SetImage(string imagePath)
        {
            var imageBytes = GetImageByte(imagePath);
            imageHash = GetImageHash(imageBytes);
            image = GetImage(imageBytes);
        }

        public decimal GetCorrectRate()
        {
            return ((decimal)numberOfCorrect/totalSolvedNumber) * 100;
        }

        public decimal GetWeightedRate()
        {
            return GetCorrectRate() * weight;
        }

        public void ReceiveSolution(string solution)
        {
            totalSolvedNumber++;
            if (solution == answer)
            {
                numberOfCorrect++;
            }
        }
    }
}
