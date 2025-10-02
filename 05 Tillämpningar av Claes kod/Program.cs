// Deklarerar variabler som behövs genom hela programmet

string vehicleType;
string regNumber;
string typeDivider = "#";
string mcDivider = "|";
string[] parkingSpaces = new string[101];       // Skapar 101 element (0-100). P-plats 0 ska aldrig användas --> 100 p-platser
bool displayMenu = true;

// Lägger in en bil på parkingSpaces[0] - endast testdata. Vi tar bort det sen
parkingSpaces[0] = "CAR#ABC123";

do
{
    MainMenu();
}
while (displayMenu);



//Meny - koperiat in Susannes meny
void MainMenu()
{
    Console.Clear();
    Console.WriteLine("\t~ Prague Parking ~\n");

    Console.WriteLine("\t1) Registrera parkering");
    Console.WriteLine("\t2) Sök fordon");
    Console.WriteLine("\t3) Ändra parkering");
    Console.WriteLine("\t4) Checka ut fordon");
    Console.WriteLine("\t5) Översikt parkering");
    Console.WriteLine("\t6) Historik/Logg");
    Console.WriteLine("\t7) Avsluta");

    try
    {
        int menuInput = int.Parse(Console.ReadLine());

        switch (menuInput)
        {
            case 1:
                {
                    Console.Clear();
                    RegisterParking();
                    Console.ReadKey();
                    break;
                }
            case 2:
                {
                    Console.Clear();
                    //SearchVehicle();
                    break;
                }
            case 3:
                {
                    Console.Clear();
                    //ChangeParking();
                    break;
                }
            case 4:
                {
                    Console.Clear();
                    //CheckoutVehicle();
                    break;
                }
            case 5:
                {
                    Console.Clear();
                    //DisplayParking();
                    break;
                }
            case 6:
                {
                    Console.Clear();
                    //DisplayLog();
                    break;
                }
            case 7:
                {
                    Console.WriteLine("\nProgrammet avslutas...\n\n");
                    displayMenu = false;
                    break;
                }
            default:
                {
                    Console.Clear();
                    Console.WriteLine("Ogiltigt val, vänligen ange ett menyval mellan 1-7");
                    break;
                }
        }
    }
    catch
    {
        Console.WriteLine("Ogiltigt menyval. Välj i menyn genom att trycka på siffertangenterna.\n\n");
        MainMenu();
    }
}

    //Registrera parkering:
    void RegisterParking()
    {
        Console.Write("Ange fordonstyp: ");             // Ska vi göra knappalternativ? Så användaren inte skriver "bil" ibland och "car" ibland
        vehicleType = Console.ReadLine().ToUpper();     // ToUpper gör alla bokstäver till stora bokstäver

        Console.Write("\nAnge registreringsnummer: ");
        regNumber = Console.ReadLine().ToUpper();

        for (int i = 1; i < parkingSpaces.Length; i++)
        {
            if (parkingSpaces[i] == null || parkingSpaces[i] == "")     // la till == "". När vi tar bort bilar blir nog värdet ""
            {
                parkingSpaces[i] = vehicleType + typeDivider + regNumber;
                Console.WriteLine($"Fordon: {vehicleType}{typeDivider}{regNumber} parkeras på plats: {i} ");
                return;
            }

        }
    }






    /*
    // Här börjar Claes kod
    string fordonsTyp = "CAR";

    string skiljetecken = "#";

    string regNummer = "ABC123";

    int platsNummer = 3;

    string[] PHus = new string[101];

    ParkeraFordon(PHus, skiljetecken, fordonsTyp, regNummer, platsNummer);
    Console.WriteLine($"Plats nummer {platsNummer}: {PHus[platsNummer]}");
    Console.WriteLine(HämtaPRuta(PHus, platsNummer));


    void ParkeraFordon(string[] PHus, string skiljetecken, string fordonsTyp, string regNummer, int platsNummer)
    {
        //Stoppa in fordonet på angiven plats
        PHus[platsNummer] = fordonsTyp + skiljetecken + regNummer;
    }
    {

        //Stoppa in fordonet på angiven plats

        PHus[platsNummer] = fordonsTyp + skiljetecken + regNummer;

    }


    string HämtaPRuta(string[] PHus, int platsNummer)
    {
        //Hämta fordonet på angiven plats
        string[] temp = PHus[platsNummer].Split('#');
        return String.Format("Plats nummer: {0} innehåller fordonstyp: {1} med registreringsnummer: {2}", platsNummer, temp[0], temp[1]);
    }
    */