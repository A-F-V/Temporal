using System;
using Pastel;

namespace Temporal
{
    internal class PastelConsole
    {
        private readonly ColourPalette palette;

        public PastelConsole(ColourPalette palette)
        {
            this.palette = palette;
        }

        public string Format(string literal, params object[] bindings)
        {
            string output = "";
            string[] temp = literal.Split('{', '}');
            for (int pos = 0; pos < temp.Length; pos++)
                if (pos % 2 == 0)
                    output += temp[pos].Pastel(palette.Body);
                else
                    output += bindings[(pos - 1) / 2].ToString().Pastel(palette[int.Parse(temp[pos])]);

            return output;
        }

        public void FormatWriteLine(string literal, params object[] bindings)
        {
            Console.WriteLine(Format(literal, bindings));
        }

        public void FormatWrite(string literal, params object[] bindings)
        {
            Console.Write(Format(literal, bindings));
        }

        public void WriteLine(string literal, int ID = -2)
        {
            Console.WriteLine(literal.Pastel(palette[ID]));
        }

        public string ReadAnswer()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string output = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            return output;
        }

        public string AskListQuestion(string[] options)
        {
            for (int i = 0; i < options.Length; i++) WriteLine($"{i + 1}. {options[i]}", i % 2 == 0 ? 0 : -2);

            return ReadAnswer();
        }

        public void WriteError(Exception e)
        {
            Console.WriteLine();
            FormatWriteLine("Error... {-3}", e.Message);
            Console.WriteLine();
        }

        public int AskIntQuestion(string question)
        {
            WriteLine(question);
            return int.Parse(ReadAnswer());
        }

        public string AskQuestion(string question)
        {
            WriteLine(question);
            return ReadAnswer();
        }

        public float AskFloatQuestion(string question)
        {
            WriteLine(question);
            return float.Parse(ReadAnswer());
        }
    }
}