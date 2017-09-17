using MatoPeli.Rajapinnat;

namespace MatoPeli.Luokat
{
    class Pala : IPositio
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Pala(int x, int y) {
            this.X = x;
            this.Y = y;
        }

        public bool Osuu(Pala pala) {
            return (this.X == pala.X && this.Y == pala.Y);
        }

        public override string ToString() {
            return $"({this.X},{this.Y})";
        }
    }
}
