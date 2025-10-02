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

    // La till try-catch för att säkra upp koden
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
                    SearchVehicle();
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
    Console.WriteLine("\t ~~ REGISTRERA FORDON ~~");
    // Gjorde om till knappalternativ, så användaren inte skriver "bil", "car", eller nåt annat själv
    vehicleType = VehicleType();

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

static string VehicleType()
{
    Console.WriteLine("\nVälj fordonstyp:");
    Console.WriteLine("\t[1] Bil");
    Console.WriteLine("\t[2] MC");
    try
    {
        int menuSelect = int.Parse(Console.ReadLine());
        //TODO: Lägg till funktion, fånga upp om användaren inte skriver en siffra

        switch (menuSelect)
        {
            case 1:
                return "CAR";

            case 2:
                return "MC";

            default:
                Console.WriteLine("\n\nOgiltigt val. Tryck [1] för bil, eller [2] för MC.\n");     
                return VehicleType();
        }
    }
    catch
    {

        Console.WriteLine("\n\nOgiltigt val. Tryck [1] för bil, eller [2] för MC.\n");
        return VehicleType();
    }
    
}


void SearchVehicle()
{
    Console.WriteLine("\t ~~ SÖK EFTER FORDON ~~");
    Console.WriteLine("Skriv in fordonets registreringsnummer");
    string searchRegNumber = Console.ReadLine().ToUpper();          // ToUpper igen
    bool vehicleFound = false;

    // TODO: Ändra till i = 1 för att inte få med test-fordonet
    for (int i = 0; i < parkingSpaces.Length; i++)
    {
        if (parkingSpaces[i] != null && parkingSpaces[i] != "")     // Om värdet inte är null eller "" står det ett fordon på p-platsen
        {
            if (parkingSpaces[i].Contains(searchRegNumber))
            {
                // Om fordonet hittades ska info skrivas ut
                Console.WriteLine(PrintVehicleInfo(i));
                vehicleFound = true;
                break;
            }

        }
        // Är else överflödigt?
        else
        {
            continue;
        }
    }

    // Om fordonet fortfarande inte hittats efter for-loopen
    if (vehicleFound == false)
    {
        Console.WriteLine("\n\nFordonet hittades inte. \nKontrollera registreringsnumret och sök igen.");
    }
    Console.ReadKey();
}

// En metod som skriver ut info om fordon:
string PrintVehicleInfo(int index)
{
    string[] temp = parkingSpaces[index].Split('#');
    return String.Format("{0} {1} står på plats: {2}", temp[0], temp[1], index);
}

//En metod som skriver info om p-plats (t.ex. om det står 2 mc eller ett fordon på platsen)
// Behövs den här metoden?
string PrintParkingSpaceInfo(int index)
{
    //Om det står 2 MC på platsen kommer det finnas | i strängen
    if (parkingSpaces[index].Contains("|"))
    {
        string[] splitMC = parkingSpaces[index].Split('|');
        string[] temp0 = splitMC[0].Split("#");
        string[] temp1 = splitMC[1].Split("#");
        return String.Format($"Det står två MC på plats {index}: \n{temp0[1]} \n{temp1[1]}");
    }
    // Om det inte står 2 MC på platsen, står det ett fordon på platsen
    else
    {
        string[] temp = parkingSpaces[index].Split('#');
        return String.Format("{0} {1} står på plats: {2}", temp[0], temp[1], index);
    }
}






/*
// Här börjar Claes kod:

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

//TODO: bestäm och utforma enhetligt användargränssnitt