using System;
using System.Collections.Generic;
using System.IO;

namespace Компилятор
{
    struct TextPosition
    {
        public uint lineNumber; // номер строки
        public byte charNumber; // номер позиции в строке

        public TextPosition(uint ln = 0, byte c = 0)
        {
            lineNumber = ln;
            charNumber = c;
        }
    }

    struct Err
    {
        public TextPosition errorPosition;
        public byte errorCode;

        public Err(TextPosition errorPosition, byte errorCode)
        {
            this.errorPosition = errorPosition;
            this.errorCode = errorCode;
        }
    }


    class InputOutput
    {
        const byte ERRMAX = 9; // не больше 9,т.к. больше ошибок могут быть порождены первой, в реальности их может и не быть
        public static char Ch { get; set; }   //считываемый символ из текста
        public static TextPosition positionNow = new TextPosition();
        static string line;
        static byte lastInLine = 0;
        public static List<Err> err; //список ошибок
        static StreamReader File { get;  set; }
        public bool IsFinish { get; set; } = false;
        static uint errCount = 0;  //счетчик ошибок
        private StreamReader file;
        /*
        public static SemanticAnalyzer.Scope localScope = new SemanticAnalyzer.Scope();
        public static SemanticAnalyzer.TypeRec charType, intType, realType, boolType, textType;
        public static SyntaxisAnalyzer.ListRec varList;
        public static SyntaxisAnalyzer.WithStack localWith = null;
        */
        
        public InputOutput(StreamReader file)
        {
            this.file = file;
            IsFinish = false;
        }
        public static void NextCh()  //позволяет прочитать очередной символ
        {
            if (positionNow.charNumber == lastInLine)
            {
                ListThisLine();
                if (err.Count > 0)
                        ListErrors();
                ReadNextLine();
                positionNow.lineNumber = positionNow.lineNumber + 1;
                positionNow.charNumber = 0;
            }
            else ++positionNow.charNumber;
            Ch = line[positionNow.charNumber];
        }

        private static void ListThisLine()
        {
            Console.WriteLine(line);
        }

        private static void ReadNextLine()
        {
            File = new StreamReader("O:/comp.txt");
            if (!File.EndOfStream)
            {
                line = File.ReadLine();
                err = new List<Err>();
            }
            else
            {
                End();
            }
            File.Close();
        }

        static void End()
        {
            Console.WriteLine($"Компиляция завершена: : ошибок — {errCount}!");
        }

        static void ListErrors()
        {
            int pos = 6 - $"{positionNow.lineNumber} ".Length;
            string s;
            foreach (Err item in err)
            {
                ++errCount;
                s = "**";
                if (errCount < 10) s += "0";
                s += $"{errCount}**";
                while (s.Length - 1 < pos + item.errorPosition.charNumber) s += " ";
                s += $"^ ошибка код {item.errorCode}";
                Console.WriteLine(s);
            }
        }

        static public void Error(byte errorCode, TextPosition position)
        {
            Err e;
            if (err.Count <= ERRMAX)
            {
                e = new Err(position, errorCode);
                err.Add(e);
            }
        }

        static public void Error(byte errorCode)
        {
            Err e;
            if (err.Count <= ERRMAX)
            {
                e = new Err(LexicalAnalyzer.token, errorCode);
                err.Add(e);
            }
        }
    }
}