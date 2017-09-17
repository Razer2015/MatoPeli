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
        public static void AjaSovellus() {
            AlustaMuuttujat();
            //Jos ei käytetä ConfigurationManager-luokkaa, voidaan config-tiedosto
            //lukea myös tiedostokäsittelynä.
            //AlustaMuuttujat2("MatoPeli.exe.config");

            AlustaConsole();
            SoitaTunnari();

            //WriteLine(VASENYLAX);
            //WriteLine(VASENYLAY);
            //WriteLine(PELINLEVEYS);
            //WriteLine(PELINKORKEUS);
            //WriteLine(LIIKKUMISNOPEUS); 
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
                    BackgroundColor = ConsoleColor.Gray;
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

            VASENYLAX = int.Parse(appsettings["VASENYLAX"]);
            VASENYLAY = int.Parse(appsettings["VASENYLAY"]);
            PELINLEVEYS = int.Parse(appsettings["PELINLEVEYS"]);
            PELINKORKEUS = int.Parse(appsettings["PELINKORKEUS"]);
            LIIKKUMISNOPEUS = int.Parse(appsettings["LIIKKUMISNOPEUS"]);
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
            } catch (Exception ex) {
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
