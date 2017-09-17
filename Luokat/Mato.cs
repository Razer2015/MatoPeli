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
        private bool _kasva;
        public List<Pala> Palat { get; set; }
        public Pala Paa { get { return (this.Palat?[/*0*/ this.Palat.Count - 1] ?? null); } }

        public int Pituus { get { return (this.Palat.Count); } }

        public Suunta Suunta { get; set; }

        public Mato(int alkuX, int alkuY, Suunta alkusuunta) {
            _kasva = false;
            Suunta = alkusuunta;
            this.Palat = new List<Pala>();
            this.Palat.Add(new Pala(alkuX, alkuY));
        }

        private void LisaaPala() {
            if (Suunta == Suunta.OIKEA) {
                Palat.Add(new Pala(Paa.X + 1, Paa.Y));
            }
            if (Suunta == Suunta.ALAS) {
                Palat.Add(new Pala(Paa.X, Paa.Y + 1));
            }
            if (Suunta == Suunta.YLOS) {
                Palat.Add(new Pala(Paa.X, Paa.Y - 1));
            }
            if (Suunta == Suunta.VASEN) {
                Palat.Add(new Pala(Paa.X - 1, Paa.Y));
            }
        }

        public void Liiku() {
            LisaaPala();
            if (Palat.Count >= 4 && !_kasva) {
                Palat.RemoveAt(0);
            }
            else {
                _kasva = false;
            }
        }
    }
}
