using Moonpool.Models;

namespace Moonpool.Test
{
    public class ModelsTest
    {
        [Fact]
        public void Problem_never_solved()
        {
            Problem q = new() { answer = "3" };
            Assert.Equal(0, q.CorrectRate);
        }

        [Fact]
        public void Problem_all_correct()
        {
            Problem q = new() { answer = "3" };
            q.ReceiveSolution("3");
            Assert.Equal(100, q.CorrectRate);
        }

        [Fact]
        public void Problem_answer_changed()
        {
            Problem q = new() { answer = "3" };
            q.ReceiveSolution("3");
            q.ReceiveSolution("3");
            q.SetAnswer("2");
            Assert.Equal(0, q.CorrectRate);
        }

        [Fact]
        public void Problem_half_correct()
        {
            Problem q = new() { answer = "3" };
            q.ReceiveSolution("2");
            q.ReceiveSolution("3");
            var rate = q.CorrectRate;
            Assert.Equal(50, rate);
        }

        [Fact]
        public void Problem_one_third_correct()
        {
            Problem q = new() { answer = "3" };
            q.ReceiveSolution("1");
            q.ReceiveSolution("2");
            q.ReceiveSolution("3");
            Assert.Equal(((decimal)1 / 3) * 100, q.CorrectRate);
        }

        [Fact]
        public void Weight_1_with_all_correct()
        {
            Problem q = new() { answer = "3", weight = 1 };
            q.ReceiveSolution("3");
            q.ReceiveSolution("3");
            q.ReceiveSolution("3");
            Assert.Equal(100, q.WeightedRate);
        }

        [Fact]
        public void Weight_dot_3_with_all_correct()
        {
            Problem q = new() { answer = "3", weight = (decimal)0.3 };
            q.ReceiveSolution("3");
            q.ReceiveSolution("3");
            q.ReceiveSolution("3");
            Assert.Equal(30, q.WeightedRate);
        }
    }
}
