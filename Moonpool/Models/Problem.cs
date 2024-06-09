using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Moonpool.Models
{
    public class Problem
    {
        public string? subjectInfo { get; set; }
        public string? chapterInfo { get; set; }
        public string? imageByte { get; set; }
        public string? imageHash { get; set; }
        public string? answer { get; set; }
        public decimal weight { get; set; }

        public uint totalSolvedNumber { get; set; }

        public uint numberOfCorrect { get; set; }

        public BitmapImage? image;


        public Problem() { }

        public Problem(string subject, string chapter, string filepath, string answer, string weight)
        {
            subjectInfo = subject;
            chapterInfo = chapter;
            SetImage(filepath);
            SetAnswer(answer);
            SetWeight(weight);
        }

        public static byte[] GetImageByte(string imagePath)
        {
            byte[] imageBytes;
            using (FileStream fileStream = new(imagePath, FileMode.Open, FileAccess.Read))
            using (BufferedStream bufferedStream = new(fileStream))
            {
                using MemoryStream memoryStream = new();
                bufferedStream.CopyTo(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            return imageBytes;
        }

        private string GetImageBase64(BitmapImage image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(ms);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        public static string GetImageHash(byte[] imageBytes)
        {
            string hashString;
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(imageBytes);
                hashString = BitConverter.ToString(hashBytes).Replace("-", "");
            }
            return hashString;
        }

        public static string GetImageHash(string imageString)
        {
            byte[] imageBytes = Convert.FromBase64String(imageString);
            return GetImageHash(imageBytes);
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
            image = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            imageByte = GetImageBase64(image);
            imageHash = GetImageHash(imageByte);
        }

        [JsonIgnore]
        public decimal CorrectRate => totalSolvedNumber == 0 ? 0 : (decimal)numberOfCorrect / totalSolvedNumber * 100;

        [JsonIgnore]
        public decimal WeightedRate => CorrectRate * weight;

        public void SetAnswer(string newAnswer)
        {
            if (answer != newAnswer)
            {
                answer = newAnswer;
                totalSolvedNumber = 0;
                numberOfCorrect = 0;
            }
        }

        public void SetWeight(string inputString)
        {
            decimal newWeight;
            bool success = Decimal.TryParse(inputString, out newWeight);
            if (success && newWeight != weight)
            {
                weight = newWeight;
            }
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
