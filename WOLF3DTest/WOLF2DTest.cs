using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WOLF3DTest
{
    [TestClass]
    public class WOLF2DTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            uint color = 0x1919;
            Console.WriteLine("Red: " + (byte)(color >> 24));
            Console.WriteLine("Green: " + (byte)(color >> 16));
            Console.WriteLine("Blue: " + (byte)(color >> 8));
            Console.WriteLine("Alpha: " + (byte)(color));
        }
    }
}
