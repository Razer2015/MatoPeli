using static System.Console;
using MatoPeli.Luokat;
using System;

namespace MatoPeli
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                Sovellus.AjaSovellus();
            }
            catch (Exception e){
                WriteLine(e);
            }
            ReadLine();
        }
    }
}
