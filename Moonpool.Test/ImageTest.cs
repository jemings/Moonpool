using Moonpool.Models;

namespace Moonpool.Test
{
    public class ImageTest
    {
        [Fact]
        public void Image_loading()
        {
            Problem q = new();

            q.SetImage("..\\..\\..\\bananas.jpg");

            Console.WriteLine(q.imageHash);

            Assert.Equal("17B7229FC6FCB92963368D9EFB3571CB7A4EB84274713BB90A7851BF8BA0F6B8", q.imageHash);
            Assert.Equal(390, q.image?.PixelHeight);
            Assert.Equal(504, q.image?.PixelWidth);
        }
    }
}
