using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoPeli.Luokat
{
    class Omena : Pala
    {
        static private Random r = new Random();

        public ConsoleColor Vari { get; set; }

        public Omena() : base(0, 0) {

        }

        public void PositioOmena(int x, int y) {
            int varinro = r.Next(0, 100) % 3; // Jokin luvuista 0, 1 tai 2

            X = r.Next(0, x - 1);
            Y = r.Next(0, y - 1);

            if (varinro == 0) {
                Vari = ConsoleColor.Red;
            }
            if (varinro == 1) {
                Vari = ConsoleColor.Yellow;
            }
            if (varinro == 2) {
                Vari = ConsoleColor.Green;
            }
        }
    }
}
