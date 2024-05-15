using Moonpool.Models;

namespace Moonpool.Test
{
    public class ModelsTest
    {
        [Fact]
        public void Question_all_correct()
        {
            Question q = new Question { answer = "3" };
            q.ReceiveSolution("3");
            Assert.Equal(100, q.GetCorrectRate());
        }

        [Fact]
        public void Question_half_correct()
        {
            Question q = new Question { answer = "3" };
            q.ReceiveSolution("2");
            q.ReceiveSolution("3");
            var rate = q.GetCorrectRate();
            Assert.Equal(50, rate);
        }

        [Fact]
        public void Question_one_third_correct()
        {
            Question q = new Question { answer = "3" };
            q.ReceiveSolution("1");
            q.ReceiveSolution("2");
            q.ReceiveSolution("3");
            Assert.Equal(((decimal)1 / 3) * 100, q.GetCorrectRate());
        }

        [Fact]
        public void Weight_1_with_all_correct()
        {
            Question q = new Question { answer = "3", weight = 1 };
            q.ReceiveSolution("3");
            q.ReceiveSolution("3");
            q.ReceiveSolution("3");
            Assert.Equal(100, q.GetWeightedRate());
        }

        [Fact]
        public void Weight_dot_3_with_all_correct()
        {
            Question q = new() { answer = "3", weight = (decimal)0.3 };
            q.ReceiveSolution("3");
            q.ReceiveSolution("3");
            q.ReceiveSolution("3");
            Assert.Equal(30, q.GetWeightedRate());
        }
    }
}