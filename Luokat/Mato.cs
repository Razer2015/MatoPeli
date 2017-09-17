using MatoPeli.Luettelot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoPeli.Luokat
{
    class Mato
    {
        public List<Pala> Palat { get; set; }
        public Pala Paa { get { return (this.Palat?[/*0*/ this.Palat.Count - 1] ?? null); } }

        public int Pituus { get { return (this.Palat.Count); } }

        public Suunta Suunta { get; set; }

        public Mato(int alkuX, int alkuY) {
            this.Palat = new List<Pala>();
            this.Palat.Add(new Pala(alkuX, alkuY));
        }
    }
}
