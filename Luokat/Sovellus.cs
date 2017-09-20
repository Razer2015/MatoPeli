using MatoPeli.Luettelot;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using static System.Console;

namespace MatoPeli.Luokat
{
    static class Sovellus
    {
        static int VASENYLAX;
        static int VASENYLAY;
        static int PELINLEVEYS;
        static int PELINKORKEUS;
        static int LIIKKUMISNOPEUS;
        static ConsoleColor PohjaVari;
        static ConsoleColor MatoVari;

        static Mato Mato;
        static Omena Omena;
        static int Pisteet;
        static bool Exit;

        public static void AjaSovellus() {
            AlustaMuuttujat();
            //Jos ei käytetä ConfigurationManager-luokkaa, voidaan config-tiedosto
            //lukea myös tiedostokäsittelynä.
            //AlustaMuuttujat2("MatoPeli.exe.config");

            AlustaConsole();
            SoitaTunnari();

            Pelaa();

            //WriteLine(VASENYLAX);
            //WriteLine(VASENYLAY);
            //WriteLine(PELINLEVEYS);
            //WriteLine(PELINKORKEUS);
            //WriteLine(LIIKKUMISNOPEUS); 
        }

        static void Pelaa() {
            int kuluneetMillisekunnit = 0;

            do {
                Thread.Sleep(LIIKKUMISNOPEUS);
                kuluneetMillisekunnit += LIIKKUMISNOPEUS;
                PuhdistaMato();
                Mato.Liiku();
                YritaSyodaOmena();
                LopetaJosOsuu();
                KasitteleSyote();
                TulostaAika(kuluneetMillisekunnit);
                TulostaPisteet();
                PiirraMato();
                PiirraOmena();
            } while (!Exit);
        }

        private static void TulostaPisteet() {
            string teksti = string.Format("Pisteet {0:0000}", Pisteet);

            int vasen = CursorLeft, yla = CursorTop;

            SetCursorPosition(VASENYLAX + 20, VASENYLAY + PELINKORKEUS + 1);
            ForegroundColor = ConsoleColor.DarkYellow;
            BackgroundColor = ConsoleColor.Black;
            Write(teksti);

            ResetColor();
            SetCursorPosition(vasen, yla);
        }

        private static void TulostaAika(int aika) {
            double sekunteja = (double)aika / 1000;
            string teksti = string.Format("{0:00} sek", sekunteja);

            // Otetaan talteen kohdistimen sijainti
            int vasen = CursorLeft, yla = CursorTop;

            SetCursorPosition(VASENYLAX, VASENYLAY + PELINKORKEUS + 1);
            ForegroundColor = ConsoleColor.Magenta;
            BackgroundColor = ConsoleColor.Black;
            Write(teksti);

            ResetColor();
            SetCursorPosition(vasen, yla);
        }

        private static void YritaSyodaOmena() {
            if (Mato.Paa.Osuu(Omena)) {
                Beep(5000, 300);
                if(Omena.Vari == ConsoleColor.Yellow) {
                    Pisteet += 50;
                }
                else if (Omena.Vari == ConsoleColor.Red) {
                    Pisteet += 100;
                }
                else if (Omena.Vari == ConsoleColor.Green) {
                    Pisteet += 150;
                }
                Mato.Syo(Omena, PELINLEVEYS, PELINKORKEUS);
                LIIKKUMISNOPEUS = (int)(LIIKKUMISNOPEUS * 0.75);
            }
        }

        static void PiirraOmena() {
            char c1 = HaeMerkki(15);

            ForegroundColor = Omena.Vari;
            BackgroundColor = PohjaVari;
            SetCursorPosition(VASENYLAX + 1 + Omena.X, VASENYLAY + 1 + Omena.Y);
            Write(c1);
            ResetColor();
        }

        private static void LopetaJosOsuu() {
            if (Mato.OsuuItseensa()) {
                Beep();
                Exit = true;
            }

            // Osuuko reunoihin
            if (Mato.Paa.X == -1 || Mato.Paa.X == (PELINLEVEYS - 1) ||
                Mato.Paa.Y == -1 || Mato.Paa.Y == (PELINKORKEUS - 1)) {
                Beep();
                Exit = true;
            }
        }

        static void KasitteleSyote() {
            Thread.Sleep(50);
            if (KeyAvailable) {
                ConsoleKeyInfo keyInfo = ReadKey(true);
                switch (keyInfo.Key) {
                    case ConsoleKey.UpArrow:
                        Mato.Suunta = Suunta.YLOS;
                        break;
                    case ConsoleKey.DownArrow:
                        Mato.Suunta = Suunta.ALAS;
                        break;
                    case ConsoleKey.LeftArrow:
                        Mato.Suunta = Suunta.VASEN;
                        break;
                    case ConsoleKey.RightArrow:
                        Mato.Suunta = Suunta.OIKEA;
                        break;
                }
            }
        }

        static void PuhdistaMato() {
            BackgroundColor = PohjaVari;
            foreach (var p in Mato.Palat) {
                SetCursorPosition(VASENYLAX + 1 + p.X, VASENYLAY + 1 + p.Y);
                Write(' ');
            }
        }

        static void PiirraMato() {
            ForegroundColor = MatoVari;
            BackgroundColor = PohjaVari;
            char c1 = HaeMerkki(177),
                c2 = HaeMerkki(16),
                c3 = HaeMerkki(17),
                c4 = HaeMerkki(30),
                c5 = HaeMerkki(31);

            foreach (var p in Mato.Palat) {
                SetCursorPosition(VASENYLAX + 1 + p.X, VASENYLAY + 1 + p.Y);
                if (p.Equals(Mato.Paa)) {
                    switch (Mato.Suunta) {
                        case Suunta.ALAS:
                            Write(c2);
                            break;
                        case Suunta.YLOS:
                            Write(c4);
                            break;
                        case Suunta.VASEN:
                            Write(c3);
                            break;
                        case Suunta.OIKEA:
                            Write(c5);
                            break;
                    }
                }
                else {
                    Write(c1);
                }
            }
        }

        static void PiirraKehys(int x, int y) {
            Clear();
            char c1 = HaeMerkki(201),
                c2 = HaeMerkki(205),
                c3 = HaeMerkki(183),
                c4 = HaeMerkki(186),
                c5 = HaeMerkki(200),
                c6 = HaeMerkki(188);

            SetCursorPosition(x, y);

            Write(c1);
            for (int i = 0; i < PELINLEVEYS; i++) {
                Write(c2);
            }
            Write(c3);

            for (int i = 1; i <= PELINKORKEUS; i++) {
                SetCursorPosition(x, y + i);
                Write(c4);
                for (int j = 0; j < PELINLEVEYS; j++) {
                    BackgroundColor = PohjaVari;
                    Write(" ");
                }
                ResetColor();
                Write(c4);
            }

            SetCursorPosition(x, y + PELINKORKEUS);
            Write(c5);
            for (int i = 0; i < PELINLEVEYS; i++) {
                Write(c2);
            }
            Write(c6);
        }

        static void AlustaMuuttujat() {
            var appsettings = ConfigurationManager.AppSettings;

            try {
                VASENYLAX = int.Parse(appsettings["VASENYLAX"]);
                VASENYLAY = int.Parse(appsettings["VASENYLAY"]);
                PELINLEVEYS = int.Parse(appsettings["PELINLEVEYS"]);
                PELINKORKEUS = int.Parse(appsettings["PELINKORKEUS"]);
                LIIKKUMISNOPEUS = int.Parse(appsettings["LIIKKUMISNOPEUS"]);
                PohjaVari = ConsoleColor.Gray;
                MatoVari = ConsoleColor.DarkGreen;

                Mato = new Mato(PELINLEVEYS / 2, PELINKORKEUS / 2, Suunta.ALAS);
                Omena = new Omena();
                Omena.PositioOmena(PELINLEVEYS, PELINKORKEUS);
                Pisteet = 0;
                Exit = false;
            }
            catch (Exception) {
                throw new ApplicationException("Konfiguroinnin lukemisessa virhe. ");
            }
        }



        static void AlustaConsole() {
            WindowWidth = PELINLEVEYS + VASENYLAX + 4;
            WindowHeight = PELINKORKEUS + VASENYLAY + 3;
            BufferWidth = WindowWidth;
            BufferHeight = WindowHeight;
            Title = $"Matopeli (c) Hans Nieminen, {DateTime.Today.Year}";

            CursorVisible = false;
            PiirraKehys(2, 2);
        }

        static char HaeMerkki(int koodi) {
            // Hakee merkistön sivulta 437 merkin, jonka järjestysnumero saadaan parametrista koodi
            return Encoding.GetEncoding(437).GetChars(new byte[] { (byte)koodi })[0];
        }

        static void SoitaTunnari() {
            Beep();
            Thread.Sleep(200);
            Beep();
            Thread.Sleep(200);
            Beep();
        }

        static void AlustaMuuttujat2(string tiedosto) {
            string rivi;
            try {
                using (var sr = new StreamReader(File.Open(tiedosto, FileMode.Open))) {
                    while (!sr.EndOfStream) {
                        rivi = sr.ReadLine();
                        if (rivi.Contains("\"VASENYLAX\"")) {
                            rivi = rivi.Substring(rivi.IndexOf("value=") + 7);
                            rivi = rivi.Substring(0, rivi.IndexOf("\""));
                            VASENYLAX = int.Parse(rivi);
                        }
                        if (rivi.Contains("\"VASENYLAY\"")) {
                            rivi = rivi.Substring(rivi.IndexOf("value=") + 7);
                            rivi = rivi.Substring(0, rivi.IndexOf("\""));
                            VASENYLAY = int.Parse(rivi);
                        }
                        if (rivi.Contains("\"PELINLEVEYS\"")) {
                            rivi = rivi.Substring(rivi.IndexOf("value=") + 7);
                            rivi = rivi.Substring(0, rivi.IndexOf("\""));
                            PELINLEVEYS = int.Parse(rivi);
                        }
                        if (rivi.Contains("\"PELINKORKEUS\"")) {
                            rivi = rivi.Substring(rivi.IndexOf("value=") + 7);
                            rivi = rivi.Substring(0, rivi.IndexOf("\""));
                            PELINKORKEUS = int.Parse(rivi);
                        }
                        if (rivi.Contains("\"LIIKKUMISNOPEUS\"")) {
                            rivi = rivi.Substring(rivi.IndexOf("value=") + 7);
                            rivi = rivi.Substring(0, rivi.IndexOf("\""));
                            LIIKKUMISNOPEUS = int.Parse(rivi);
                        }
                    }
                }
            }
            catch (Exception ex) {
                WriteLine($"Tiedoston {tiedosto} lukeminen ei onnistu {ex.Message}");
            }
            //StreamReader sr = null;
            //string rivi;
            //try
            //{
            //    sr = new StreamReader(File.Open(tiedosto, FileMode.Open));
            //    while (!sr.EndOfStream)
            //    {
            //        rivi = sr.ReadLine();
            //        if (rivi.Contains("\"VASENYLAX\""))
            //        {
            //            rivi = rivi.Substring(rivi.IndexOf("value=") + 7);
            //            rivi = rivi.Substring(0, rivi.IndexOf("\""));
            //            VASENYLAX = int.Parse(rivi);
            //        }
            //        if (rivi.Contains("\"VASENYLAY\""))
            //        {
            //            rivi = rivi.Substring(rivi.IndexOf("value=") + 7);
            //            rivi = rivi.Substring(0, rivi.IndexOf("\""));
            //            VASENYLAY = int.Parse(rivi);
            //        }
            //        if (rivi.Contains("\"PELINLEVEYS\""))
            //        {
            //            rivi = rivi.Substring(rivi.IndexOf("value=") + 7);
            //            rivi = rivi.Substring(0, rivi.IndexOf("\""));
            //            PELINLEVEYS = int.Parse(rivi);
            //        }
            //        if (rivi.Contains("\"PELINKORKEUS\""))
            //        {
            //            rivi = rivi.Substring(rivi.IndexOf("value=") + 7);
            //            rivi = rivi.Substring(0, rivi.IndexOf("\""));
            //            PELINKORKEUS = int.Parse(rivi);
            //        }
            //        if (rivi.Contains("\"LIIKKUMISNOPEUS\""))
            //        {
            //            rivi = rivi.Substring(rivi.IndexOf("value=") + 7);
            //            rivi = rivi.Substring(0, rivi.IndexOf("\""));
            //            LIIKKUMISNOPEUS = int.Parse(rivi);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    WriteLine($"Tiedoston {tiedosto} lukeminen ei onnistu {ex.Message}");
            //}
            //finally
            //{
            //    sr?.Close();
            //}
        }
    }
}
