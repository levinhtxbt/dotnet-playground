using System;

namespace M1GetListBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            new HtmlAgilityPackService().FromDms();

            Console.ReadKey();
        }
    }
}
