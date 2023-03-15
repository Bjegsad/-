using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Компилятор
{
    class Program
    {
        static void Main()
        {
            var lexan = new LexicalAnalyzer("O:\\comp.txt");
            lexan.LexAn();
        }
    }
}
