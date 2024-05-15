using Moonpool.Models;

namespace Moonpool.Test
{
    public class ImageTest
    {
        [Fact]
        public void Image_loading()
        {
            Question q = new();

            q.SetImage("..\\..\\..\\bananas.jpg");

            Assert.Equal("476D8B572EFACD8CD4F8BFDADB0C91F0E724C6E29BAA00F4F3C991698E92B7EB", q.imageHash);
            Assert.Equal(390, q.image?.PixelHeight);
            Assert.Equal(504, q.image?.PixelWidth);
        }
    }
}
