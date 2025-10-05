// Deklarerar variabler som behövs genom hela programmet
string vehicleType;
string regNumber;
string typeDivider = "#";
string mcDivider = "|";
string[] parkingSpaces = new string[101];       // Skapar 101 element (0-100). P-plats 0 ska aldrig användas --> 100 p-platser
bool displayMenu = true;
List<string> logs = new List<string>();     //skapar en lista för händelser

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
    Console.WriteLine("\t3) Flytta fordon");
    Console.WriteLine("\t4) Checka ut fordon");
    Console.WriteLine("\t5) Översikt parkering");
    Console.WriteLine("\t6) Logg");//lägger in DisplayLog(); för att kunna testa om registreras korrekt - kan ta bort detta menyval sen
    Console.WriteLine("\t7) Avsluta");

    Console.Write("\n\tVälj ett alternativ (1-7): "); // la till denna för gränssnittet

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
                    MoveVehicle();
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
                    Console.ReadKey();
                    break;
                }
        }
    }
    catch
    {
        Console.WriteLine("Ogiltigt menyval. Välj i menyn genom att trycka på siffertangenterna.\n\n");
        Console.ReadLine(); //Lägger till denna kod för att pausa progrmmet innan den återgår till menyvalen
        //Tar bort MainMenu(); då felmeddelandet inte visas pga att Console.Clear(); tar bort allt i consolen 
    }

}

//Registrera parkering:
void RegisterParking()
{
    Console.WriteLine("\t ~~ REGISTRERA FORDON ~~");
    // Gjorde om till knappalternativ, så användaren inte skriver "bil", "car", eller nåt annat själv
    vehicleType = VehicleType();

    // HJH: Ska vi lägga in kod som förhindrar att regnumret blir för långt?
    // Eller inte består av bokstäver och siffror?
    // "Registreringsnummer är alltid strängar med maxlängd 10 tecken." (pdf:en med uppiften)
    //Deklarerar en sträng variabel för att kontroller om inmatningen är ett tomt värde, eller fler än 10 tecken.
    string input;
    do
    {
        Console.Write("\nAnge registreringsnummer: ");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input) || input.Length > 10)
        {
            Console.WriteLine("Regisreringsnumret måste vara 1-10 tecken. Vänligen försök igen");
        }
    }
    while (string.IsNullOrEmpty(input) || input.Length > 10);

    regNumber = input.ToUpper();

    int parkingIndex = -1; // initierar en ny int variabel med värdet -1 för att hålla reda på vilken p-plats fordonet tilldelas,
                           //om det inte finns en ledig plats förblir värdet -1 och ett felmeddelande visas, används även för att visa info om var fordonet parkerats


    if (vehicleType == "MC") //Om MC registreras: kolla först om det finns en ledig plats bredvid en MC 
    {
        for (int i = 1; i < parkingSpaces.Length; i++) //börjar söka på plats 1 i arrayen
                                                       //loopar igenom parkingSpace-arrayen och kollar om det finns en plats som inte är tom, och som börjar på "MC" och inte innehåller tecknet '|'
        {
            if (!string.IsNullOrEmpty(parkingSpaces[i]) &&
                parkingSpaces[i].StartsWith("MC#")
                && !parkingSpaces[i].Contains("|"))
            {
                parkingIndex = i;
                break;
            }
        }
        if (parkingIndex == -1)

            for (int i = 1; i < parkingSpaces.Length; i++) //om det inte finns en halvtom-plats så kontrolleras om det finns en ledig plats
            {
                if (string.IsNullOrEmpty(parkingSpaces[i]))
                {
                    parkingIndex = i;
                    break;
                }
            }
    }
    else //Om en bil regisreras
    {
        for (int i = 1; i < parkingSpaces.Length; i++)
        {

            if (string.IsNullOrEmpty(parkingSpaces[i]))
            {
                parkingIndex = i;
                break;
            }
        }
    }
    if (parkingIndex != -1)
    {
        AssignVehicleToParking(vehicleType, regNumber, parkingIndex);
        Console.WriteLine(PrintParkingSpaceInfo(parkingIndex));
    }
    else
    {
        Console.WriteLine("Ingen ledig plats hittades");
    }
    Console.ReadLine();
}


static string VehicleType()
{
    Console.WriteLine("\n\t[1] Bil\n\t[2] MC");
    Console.Write("\n\tVälj fordonstyp: ");

    //Console.Write("\nVälj fordonstyp:");
    //Console.WriteLine("\t[1] Bil");
    //Console.WriteLine("\t[2] MC");
    try
    {
        int menuSelect = int.Parse(Console.ReadLine());
        switch (menuSelect)
        {
            case 1:
                return "CAR";

            case 2:
                return "MC";

            default:
                Console.Clear();
                Console.WriteLine("\n\nOgiltigt val. Tryck [1] för bil, eller [2] för MC.\n");
                return VehicleType();
        }
    }
    catch
    {
        Console.Clear();
        Console.WriteLine("\n\nOgiltigt val. Tryck [1] för bil, eller [2] för MC.\n");
        return VehicleType();
    }
    Console.ReadLine();
}


void SearchVehicle()
{
    Console.WriteLine("\t ~~ SÖK EFTER FORDON ~~");
    Console.WriteLine("Skriv in fordonets registreringsnummer:");
    string searchRegNumber = Console.ReadLine().ToUpper();          // ToUpper igen
    bool vehicleFound = false;

    // TODO: Ändra till i = 1 för att inte få med test-fordonet
    //bör vi ha ett felmeddelande där det står att det inte finns några parkerade fordon om det är 0 parkerade fordon? /SR
    for (int i = 0; i < parkingSpaces.Length; i++)
    {
        if (parkingSpaces[i] != null && parkingSpaces[i] != "")     // Om värdet inte är null eller "" står det ett fordon på p-platsen
        {
            if (parkingSpaces[i].Contains(searchRegNumber))
            {
                // Om fordonet hittades ska info skrivas ut
                Console.WriteLine(PrintVehicleInfo(i));
                vehicleFound = true;
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

// HJH: Man borde kunna använda SearchVehicle
void MoveVehicle()
{

}

// En metod som skriver ut info om fordon. Kan användas i andra metoder:
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
        return String.Format($"Det står två MC på plats {index}: \n{temp0[0]}#{temp0[1]} {mcDivider} {temp0[0]}#{temp0[1]}"); //la till mcDivider
    }
    // Om det inte står 2 MC på platsen, står det ett fordon på platsen
    else
    {
        string[] temp = parkingSpaces[index].Split('#');
        return String.Format("{0}#{1} står på plats: {2}", temp[0], temp[1], index);
    }
}



//metod för att lägga till en parkering


   void AssignVehicleToParking(string vehicleType, string regNumber, int parkingIndex)
    {
        if (vehicleType == "MC")
        {
            if (string.IsNullOrEmpty(parkingSpaces[parkingIndex]))
            {
                parkingSpaces[parkingIndex] = vehicleType + typeDivider + regNumber;
            }
            else if (parkingSpaces[parkingIndex].StartsWith("MC#") && !parkingSpaces[parkingIndex].Contains(mcDivider))
            {
                parkingSpaces[parkingIndex] += mcDivider + vehicleType + typeDivider + regNumber;
            }
            // Annars: platsen är full för MC
        }
        else // CAR
        {
            parkingSpaces[parkingIndex] = vehicleType + typeDivider + regNumber;
        }
        PrintParkingSpaceInfo(parkingIndex);
    }


//kommenterar på denna tillsvidare då den ej behövs
//string SaveLog(string vehicleType, string typeDivider, string regNumber, int parkingPlace, DateTime startTime)
//{
//    string log = ($"Parkeringplats {parkingPlace}: {vehicleType}{typeDivider}{regNumber}\tIncheckad: {startTime: yyyy-MM-dd HH:mm:ss}");

//    logs.Add(log);
//    return log;
//}


//void DisplayLog() //kanske ska använda denna metod för själva parkeringsöversikten? 
//{
//    if (logs.Count == 0)
//    {
//        Console.WriteLine("Ingen historik finns ännu");
//        Console.ReadKey();
//        return;
//    }
//    foreach (var log in logs)
//    {
//        Console.WriteLine(log);
//    }
//    Console.ReadKey();
//}

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