using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawImageTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GDI+绘图程序已启动");

            DrawImage draw = new DrawImage();

            draw.Draw();

            Console.WriteLine("GDI+绘图已经完成");
            Console.ReadKey();
        }
    }
}
