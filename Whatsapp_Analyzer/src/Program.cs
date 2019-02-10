using System;

namespace whatsapp_analytic_tool
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Analyzer(@"");
            a.WriteAnalyze(@"result.txt");
            Console.WriteLine(a.ToString());
        }
    }
}