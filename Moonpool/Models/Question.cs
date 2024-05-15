using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Moonpool.Models
{
    public class Question
    {
        public Image? image;
        public string? answer;
        private uint totalSolvedNumber;
        private uint numberOfCorrect;
        public decimal weight;

        public decimal getCorrectRate()
        {
            return ((decimal)numberOfCorrect/totalSolvedNumber) * 100;
        }

        public decimal getWeightedRate()
        {
            return getCorrectRate() * weight;
        }

        public void receiveSolution(string solution)
        {
            totalSolvedNumber++;
            if (solution == answer)
            {
                numberOfCorrect++;
            }
        }
    }
}
