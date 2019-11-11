using System;

namespace snakegame
{
    class Program
    {
        public static void Main()
    {
        //alkujärestelyt:
        //Konsoli-ikkunan korkeuden asetus
        //merkkiasetusten pakotus UTF-8:aan ja kursorin poisto
        //Console.WindowHeight = Console.LargestWindowHeight;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;


        #region Muuttujat

        //mato -taulukon luonti. Taulukon koko on madon maksimipituus ja samalla maksimipistemäärä. Erilliset taulukot hännän X- ja Y- pisteille
        //Asetetaan madon pään (taulukon 0.-paikka) sijainnin arvot
        //Asetetaan ensimmäisen syötin sijainnin arvot
        //muiden muuttujien luonti
        int[] matoX = new int[1000];
        matoX[0] =  40;
        int[] matoY = new int[1000];
        matoY[0] = 25;
        int syottiX = 10;
        int syottiY =0; 
        int syodytSyotit = 0;

        string kayttajanValinta = "";

        decimal peliNopeus = 150m;

        bool pelataan = true;
        bool osuikoSeinaan = false;
        bool syottiSyoty = false;
        bool onkoValikossa = true;

        Random satunnainen = new Random();


        #endregion


        NaytaValikko(out kayttajanValinta);


        do
        {

            
            switch (kayttajanValinta)
            {
                #region Ohjeet

                case "1":
                case "o":
                case "ohjeet":
                    Console.Clear();
                    RakennaReunat();
                    Console.SetCursorPosition(5, 5);
                    Console.WriteLine("1. Liikuta matoa nuolinäppäimillä");
                    Console.SetCursorPosition(5, 6);
                    Console.WriteLine("2. Mato kuolee, jos se osuu seinään tai itseensä");
                    Console.SetCursorPosition(5, 7);
                    Console.WriteLine("3. Saat pisteitä syömällä syöttejä,");
                    Console.SetCursorPosition(5, 8);
                    Console.WriteLine("mutta samalla mato pitenee ja sen nopeus kasvaa.");
                    Console.SetCursorPosition(5, 9);
                    Console.WriteLine("4. Muista varoa! Käärme liikku nopeampaa vertikaalisesti");
                    Console.SetCursorPosition(5, 10);
                    Console.WriteLine( "kuin horisontaalisesti");
                    Console.SetCursorPosition(5, 11);
                    Console.WriteLine();
                    Console.SetCursorPosition(5, 12);
                    Console.WriteLine("Paina enter-näppäintä palataksesi valikkoon");
                    Console.ReadLine();
                    Console.Clear();
                    NaytaValikko(out kayttajanValinta);
                    break;
                #endregion


                #region Pelaaminen

                case "2":
                case "p":
                case "pelaa":
                    #region Alkuasetukset

                    //Siirrytään pois valikosta ja mato näytölle (luodaan "ensimmäinen" pää)
                    Console.Clear();
                    Console.SetCursorPosition(matoX[0], matoY[0]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    char mato = '\u263A';
                    Console.WriteLine(mato);


                    //ensimmäisen syötin asetus
                    SyotinAsetus(satunnainen, out syottiX, out syottiY);
                    SyotinVarjays(syottiX, syottiY);


                    //asetetaan rajat kentälle
                    RakennaReunat();


                    ConsoleKey liike = Console.ReadKey().Key;

                    #endregion


                    do
                    {
                        #region Ohjaimet

                        //mato liikkeelle ja ohjainten asetus
                        switch (liike)
                        {
                            case ConsoleKey.LeftArrow:
                                Console.SetCursorPosition(matoX[0], matoY[0]);
                                Console.Write(" ");
                                matoX[0]--;
                                break;

                            case ConsoleKey.RightArrow:
                                Console.SetCursorPosition(matoX[0], matoY[0]);
                                Console.Write(" ");
                                matoX[0]++;
                                break;

                            case ConsoleKey.UpArrow:
                                Console.SetCursorPosition(matoX[0], matoY[0]);
                                Console.Write(" ");
                                matoY[0]--;
                                break;

                            case ConsoleKey.DownArrow:
                                Console.SetCursorPosition(matoX[0], matoY[0]);
                                Console.Write(" ");
                                matoY[0]++;
                                break;
                        }

                        #endregion


                        #region Pelimekaniikat

                        //kasvatetaan matoa
                        KasvataKaarmetta(syodytSyotit, matoX, matoY, out matoX, out matoY);


                        osuikoSeinaan = SeinaOsuma(matoX[0], matoY[0]);


                        //törmäysmekaniikat
                        if (osuikoSeinaan == true)
                        {

                            pelataan = false;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.SetCursorPosition(15, 20);
                            Console.WriteLine("Mato osui seinään ja kuoli!! Yritähän uudelleen :)");
                            Console.SetCursorPosition(15, 21);
                            Console.Write("Pisteesi: " + 100 * syodytSyotit + "!");
                            Console.SetCursorPosition(15, 22);
                            Console.WriteLine("Paina enteriä jatkaksesi");
                            syodytSyotit = 0;
                            Console.ReadLine();
                            Console.Clear();

                            //palataan valikkoon
                            NaytaValikko(out kayttajanValinta);

                            //Console.ReadKey(); //////////////////////////////////////////////// TÄÄLLÄ READKEY!!
                        }


                        //onko syotti syöty?
                        syottiSyoty = SyotiinkoSyotti(matoX[0], matoY[0], syottiX, syottiY);


                        //seuraavien syöttien asetus satunnaisesti
                        if (syottiSyoty == true)
                        {
                            SyotinAsetus(satunnainen, out syottiX, out syottiY);
                            SyotinVarjays(syottiX, syottiY);
                            syodytSyotit++;
                            peliNopeus = peliNopeus * 0.950m;
                        }


                        if (Console.KeyAvailable) liike = Console.ReadKey().Key;

                        //madon liikkeen "hidastus"
                        System.Threading.Thread.Sleep(Convert.ToInt32(peliNopeus));

                        #endregion

                    } while (pelataan == true);
                    break;
                #endregion


                #region Lopetus ja väärä syöte
            
                case "3":
                case "l":
                case "lopeta":
                onkoValikossa = false;
                Console.Clear();
                break;


                default:
                    Console.WriteLine("Syöttettäsi ei ymmärretty. Paina enteriä yrittääksesi uudelleen.");
                    Console.ReadLine();
                    Console.Clear();
                    NaytaValikko(out kayttajanValinta);
                    break;
                    #endregion

            }


        } while (onkoValikossa == true);


    }


    #region Aliohjelmat


    public static void NaytaValikko(out string kayttajanValinta)
    {
        string valikko1 = "Tervetuloa Matopeliin!\nValinta: numero + enter-näppäin\n\n1. Ohje\n2. Pelaa peliä\n3. Lopeta \n\n\n";
        Console.WriteLine(valikko1);
        kayttajanValinta = Console.ReadLine().ToLower();
        
    }


    public static void KasvataKaarmetta(int syodytSyotit, int[] matoX1, int[] matoY1, out int[] matoX2, out int[] matoY2)
    {
        //luodaan uusi pää
        Console.SetCursorPosition(matoX1[0], matoY1[0]);
        Console.ForegroundColor = ConsoleColor.Green;
        char uusiMato = '\u263A';
        Console.WriteLine(uusiMato);


        //luodaan häntä
        for (int i =1; i<syodytSyotit + 1; i++)
        {
            Console.SetCursorPosition(matoX1[i], matoY1[i]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("o");
        }


        //"pyyhitään" hännänpää pois
        Console.SetCursorPosition(matoX1[syodytSyotit + 1], matoY1[syodytSyotit + 1]);
        Console.WriteLine(" ");


        //jokaisen ruumiinosan sijainti
        for (int i = syodytSyotit+1; i> 0; i--)
        {
            matoX1[i] = matoX1[i - 1];
            matoY1[i] = matoY1[i - 1];
        }


        //palautetaan uusi taulukko
        matoX2 = matoX1;
        matoY2 = matoY1;
    }


    public static void RakennaReunat()
    {

        //kentän vasen ja oikea reuna
        for (int i = 1; i < 41; i++)
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(1, i);
            Console.Write("8");
            Console.SetCursorPosition(70, i);
            Console.Write("8");

        }


        //kentän ala- ja yläreuna
        for (int i = 1; i < 71; i++)
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(i, 1);
            Console.Write("8");
            Console.SetCursorPosition(i, 40);
            Console.Write("8");

        }


    }


    public static bool SeinaOsuma(int matoX, int matoY)
    {

        if (matoX == 1 || matoX == 70 || matoY == 1 || matoY == 40)
        return true;
        else return false;

    }


    public static void SyotinAsetus(Random satunnainen, out int syottiX, out int syottiY)
    {

        syottiX = satunnainen.Next(0 + 2, 70 - 2);
        syottiY = satunnainen.Next(0 + 2, 40 - 2);

    }


    private static void SyotinVarjays(int syottiX, int syottiY)
    {

        Console.SetCursorPosition(syottiX, syottiY);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("@");

    }


    private static bool SyotiinkoSyotti(int matoX, int matoY, int syottiX, int syottiY)
    {

        if (matoX == syottiX && matoY == syottiY) return true;
        else return false;

    }
    

    #endregion
    }
}
