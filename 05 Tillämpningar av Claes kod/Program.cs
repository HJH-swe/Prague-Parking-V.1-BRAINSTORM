// Deklarerar variabler som behövs genom hela programmet
string? vehicleType;
string regNumber;
string delimiter = "#";       
string mcDelimiter = "|";
string[] parkingSpaces = new string[101];       // Skapar 101 element (0-100). P-plats 0 ska aldrig användas --> 100 p-platser
bool displayMenu = true;
// List<string> logs = new List<string>();     //skapar en lista för händelser - används inte för tillfället

// Lägger in en bil på parkingSpaces[0] - endast testdata. Vi tar bort det sen
parkingSpaces[0] = "CAR#ABC123";

do
{
    MainMenu();
}
while (displayMenu);

void MainMenu()
{
    Console.Clear();
    Console.WriteLine("\t~~ PRAGUE PARKING ~~\n");

    Console.WriteLine("\t1) Registrera parkering");
    Console.WriteLine("\t2) Sök fordon");
    Console.WriteLine("\t3) Flytta fordon");
    Console.WriteLine("\t4) Checka ut fordon");
    Console.WriteLine("\t5) Översikt parkering");
    Console.WriteLine("\t6) Avsluta");

    Console.Write("\n\tVälj ett alternativ (1-6): "); 

    // Använder try-catch för att säkra upp koden

    try
    {
        int menuInput = int.Parse(Console.ReadLine());

        switch (menuInput)
        {
            case 1:
                {
                    Console.Clear();
                    Console.WriteLine("\t ~~ REGISTRERA FORDON ~~");
                    vehicleType = VehicleType();
                    if (vehicleType == null)
                    {
                        Console.WriteLine("\tÅtergår till huvudmenyn...");  // HJH: Behöver vi skriva ut det här?
                                                                            // Jag tycker nog det räcker att programmet går till huvudmenyn.
                        Console.ReadLine();
                        //Thread.Sleep(1000);                               // Ett alternativ till console.readline
                                                                            // -> går vidare automatiskt efter 1 sekund
                        break;
                    }
                    RegisterParking(vehicleType);
                    Console.ReadLine();
                    break;
                }
            case 2:
                {
                    // HJH: Uppdaterade sökfunktionen, la till att man måste skriva in ett regnummer att söka på.
                    // Men koden är lite stökig. Är uppdateringen onödig? Ska vi ta bort?
                    bool validSearch = false;
                    Console.Clear();
                    Console.WriteLine("\t ~~ SÖK EFTER FORDON ~~");
                    do 
                    {
                        Console.WriteLine("\nSkriv in fordonets registreringsnummer:");
                        string searchRegNumber = Console.ReadLine().ToUpper();
                        if (searchRegNumber.Length == 0 || searchRegNumber.Length > 10)
                        {
                            Console.WriteLine("\n\nOgiltigt registreringsnummer. \nKontrollera registreringsnumret och sök igen.");
                        }
                        else
                        {
                            validSearch = true;
                            SearchVehicle(searchRegNumber);
                        }
                    }
                    while(!validSearch);
                    break;
                }
            case 3:
                {
                    Console.Clear();
                    Console.WriteLine("\t ~~ FLYTTA FORDON ~~");
                    // HJH: Behöver lägga till lite kod här för att få fram int fromSpot och int toSpot (metodens parametrar)
                    MoveVehicle();
                    break;
                }
            case 4:
                {
                    Console.Clear();
                    Console.WriteLine("\t ~~ CHECKA UT FORDON ~~");
                    //CheckoutVehicle();
                    break;
                }
            case 5:
                {
                    Console.Clear();
                    Console.WriteLine("\t~~ ÖVERSIKT PARKERING ~~");
                    DisplayParking(); //La till denna tillfälligt för att kunna testköra programmet 
                    break;
                }
            case 6:
                {
                    Console.WriteLine("\nProgrammet avslutas...\n\n");
                    Thread.Sleep(1000); // HJH: La in en liten paus innan programmet stängs
                    displayMenu = false;
                    break;
                }
            default:
                {
                    Console.Clear();
                    Console.WriteLine("Ogiltigt val, vänligen ange ett menyval mellan 1-6");
                    Console.ReadKey();
                    break;
                }
        }
    }
    catch
    {
        Console.WriteLine("Ogiltigt menyval. Välj i menyn genom att trycka på siffertangenterna.\n\n");
        Console.ReadLine(); //Lägger till denna kod för att pausa progrmmet innan den återgår till menyvalen
    }

}

//Registrera parkering:
void RegisterParking(string? vehicleType)
{
    //Kontrollerar att registreringsnumret inte är ett tomt värde och max 10 tecken. Visar felmeddelande vid felaktig inmatning.
    string? input;
    do
    {
        Console.Write("\nAnge registreringsnummer: ");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input) || input.Length > 10)
        {
            Console.WriteLine("Registreringsnumret måste vara 1-10 tecken. Vänligen försök igen");
        }
    }
    while (string.IsNullOrEmpty(input) || input.Length > 10);

    regNumber = input.ToUpper();
    int parkingIndex = -1; // initierar en ny int variabel med värdet -1 för att hålla reda på vilken p-plats fordonet tilldelas. -1 betyder att ingen plats har hittats än
                           //om det inte finns en ledig plats förblir värdet -1 och ett felmeddelande visas, används även för att visa info om var fordonet parkerats

    if (vehicleType == "MC") //Om en MC registreras
    {
        for (int i = 1; i < parkingSpaces.Length; i++) //börjar söka på plats 1 i arrayen
                                                       //loopar igenom parkingSpace-arrayen och kollar om det finns en plats som inte är tom, och som börjar på "MC" och inte innehåller tecknet '|'
        {
            //kontrollerar först om det finns en ledig plats bredvid en MC 
            if (!string.IsNullOrEmpty(parkingSpaces[i]) &&
                parkingSpaces[i].StartsWith("MC#")
                && !parkingSpaces[i].Contains('|'))
            {
                parkingIndex = i;
                break;
            }
            if (parkingIndex == -1) //om det inte finns en halvtom-plats så kontrolleras om det finns en ledig plats

                for (int j = 1; i < parkingSpaces.Length; j++)
                {
                    if (string.IsNullOrEmpty(parkingSpaces[j]))
                    {
                        parkingIndex = j;
                        break;
                    }
                }
        }
    }
    else //Om en bil registreras
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
    //Console.ReadLine();
}

//Metod för att menyval vid registrering av fordon
static string? VehicleType()
{
    Console.WriteLine("\n\t[1] Bil\n\t[2] MC \n\t[3] Återgå till huvudmenyn");
    Console.Write("\n\tVälj fordonstyp: ");

    try
    {
        int menuSelect = int.Parse(Console.ReadLine());
        switch (menuSelect)
        {
            case 1:
                return "CAR";

            case 2:
                return "MC";

            case 3: //la till detta för att kunna gå tillbaka till huvudmenyn

                return null;
                
            default:
                {
                    Console.Clear();
                    Console.WriteLine("\n\nOgiltigt val. Tryck [1] för bil, [2] för MC eller [3] för huvudmenyn.\n");
                    return VehicleType();
                }
        }
    }
    catch
    {
        Console.Clear();
        Console.WriteLine("\n\nOgiltigt val. Tryck [1] för bil, [2] för MC eller [3] för huvudmenyn.\n");
        return VehicleType();
    }
}

//Metod för att söka efter fordon
void SearchVehicle(string searchNumber)
{
    bool vehicleFound = false;
    bool emptyParking = true;

    for (int i = 1; i < parkingSpaces.Length; i++)
    {
        // Om värdet inte är null eller "" står det ett fordon på p-platsen (och parkeringen är inte tom)
        if (parkingSpaces[i] != null && parkingSpaces[i] != "")     
        {
            emptyParking = false;
            if (parkingSpaces[i].Contains(searchNumber))
            {
                // Om fordonet hittades ska info skrivas ut
                Console.WriteLine(PrintParkingSpaceInfo(i));
                vehicleFound = true;
            }
        }
    }
    // HJH: La in felmeddelande om parkeringen är tom (SR förslag). 
    // Man måste ändå skriva in ett regnummer att söka på (görs innan metoden). Inte jättesnyggt, men duger?
    if (emptyParking)
    {
        Console.WriteLine("\n\nDet finns inga parkerade fordon.");
    }

    // Om fordonet fortfarande inte hittats efter for-loopen och parkeringen inte är tom
    if (!vehicleFound && !emptyParking)
    {
        Console.WriteLine("\n\nFordonet hittades inte. \nKontrollera registreringsnumret och sök igen.");
    }
    Console.ReadKey();
}


/* För att checka ut ett fordon behöver jag först anropa att fordonet för att sedan konvertera det till att checka ut det, så första steget antar jag är att hitta fordonet tex via regnummer för att avgöra om det är en MC/BIL */

string CheckaOut(string regNumber, string[] parkingSpaces)
{
    for (int i = 1; i < parkingSpaces.Length; i++)
    {
        if (parkingSpaces[i].Contains(regNumber))

            if (parkingSpaces[i].StartsWith("MC#") && !parkingSpaces[i].Contains('|'))
            {
            parkingSpaces[i] = null; // tar bort fordonet från platsen
                return $"Fordon {regNumber} har checkats ut från plats {i}.";
        }
    }
    return $"Inget fordon med regnr {regNumber} hittades.";
}


//Metod för att flytta fordon

/* HJH: Kommenterar bort lite kod som inte behövs i början
 * 
using System;

class Program
{
    static string[] parkingGarage = new string[100];

    static void Main(string[] args)
    {
        // Exempeldata
        parkingGarage[0] = "CAR#ABC123";       // Plats 1
        parkingGarage[5] = "MC#XYZ789";        // Plats 6
        parkingGarage[10] = "MC#LMN456";       // Plats 11

        PrintGarage();

        MoveVehicle(6, 11);   // Flytta MC från plats 6 → 11 (blir 2 MC på plats 11)
        MoveVehicle(1, 20);   // Flytta bil från plats 1 → 20

        PrintGarage();
    }
    */
/// <summary>
/// Flyttar ett fordon från en plats till en annan.
/// Hanterar regler för bil/MC/2 MC.
/// </summary>
/*static*/
void MoveVehicle(int fromSpot, int toSpot) // HJH: Om man tar bort static kan man använda samma variabler som finns i hela programmet,
                                                          // t. ex. parkingSpaces[]
    {
        // HJH: Behöver nog inte göra - 1.
        // Vår vektor går från 0-100, men vi använder inte plats 0 (i loopar osv). Så alla index borde representera rätt p-plats
        int fromIndex = fromSpot - 1;
        int toIndex = toSpot - 1;

        if (!IsValidIndex(fromIndex) || !IsValidIndex(toIndex))
        {
            Console.WriteLine(" Ogiltigt platsnummer!");
            return;
        }

        if (string.IsNullOrEmpty(parkingSpaces[fromIndex])) // HJH: Bytte namn från parkingGarage[] till parkingSpaces[] (även nedanför)
        {
            Console.WriteLine($" Ingen bil eller MC finns på plats {fromSpot}.");
            return;
        }

        string vehicleToMove = parkingSpaces[fromIndex];
        string[] vehiclesAtFrom = vehicleToMove.Split('|');

        if (vehiclesAtFrom.Length > 1)
        {
            Console.WriteLine($" Plats {fromSpot} innehåller flera fordon. Specificera vilket du vill flytta.");
            return;
        }

        if (string.IsNullOrEmpty(parkingSpaces[toIndex]))
        {
            // Målruta tom → flytta direkt
            parkingSpaces[toIndex] = vehicleToMove;
            parkingSpaces[fromIndex] = null;
            Console.WriteLine($" Fordon flyttades från plats {fromSpot} till {toSpot}."); // HJH: Måste lägga in kod (console.readline eller console.readkey)
                                                                                          // för att hinna läsa såna här meddelanden
        }
        else
        {
            string[] vehiclesAtTo = parkingSpaces[toIndex].Split('|');

            // Fall: Bil finns på målrutan → inte tillåtet
            if (parkingSpaces[toIndex].Contains("CAR"))
            {
                Console.WriteLine($" Kan inte flytta till plats {toSpot}, bil upptar platsen.");
                return;
            }

            // Fall: MC finns på målrutan → kolla om det redan är två MC
            if (vehiclesAtTo.Length == 1 && vehicleToMove.StartsWith("MC"))
            {
                // Flytta MC och kombinera
                parkingSpaces[toIndex] = parkingSpaces[toIndex] + "|" + vehicleToMove;
                parkingSpaces[fromIndex] = null;
                Console.WriteLine($" MC flyttades från plats {fromSpot} till {toSpot} (nu 2 MC på plats {toSpot}).");
            }
            else
            {
                Console.WriteLine($" Plats {toSpot} är redan full.");
            }
        }
    }

   /* static*/ bool IsValidIndex(int index)
    {
        return index >= 0 && index < parkingSpaces.Length;
    }

    /*static*/ void PrintGarage()
    {
        Console.WriteLine("\n--- Parkeringsstatus ---");
        for (int i = 0; i < parkingSpaces.Length; i++)
        {
            if (!string.IsNullOrEmpty(parkingSpaces[i]))
                Console.WriteLine($"Plats {i + 1}: {parkingSpaces[i]}");
        }
        Console.WriteLine("------------------------\n");
    }
/*}*/

//Metod som skriver ut info om p-plats (t.ex. om det står 2 mc eller ett fordon på platsen)
string PrintParkingSpaceInfo(int index)
{
    //Om det står 2 MC på platsen kommer det finnas | i strängen
    if (parkingSpaces[index].Contains('|'))
    {
        string[] splitMC = parkingSpaces[index].Split('|');
        string[] temp0 = splitMC[0].Split("#");
        string[] temp1 = splitMC[1].Split("#");
        return String.Format($"Plats {index}: {temp0[0]}#{temp0[1]} {mcDelimiter} {temp1[0]}#{temp1[1]}"); //la till mcDelimiter
                                                                                                           // HJH: ändrade till temp1[] för att skriva ut andra mc:n
    }
    // Om det inte står 2 MC på platsen --> ett fordon på p-platsen
    else
    {
        string[] temp = parkingSpaces[index].Split('#');
        return String.Format("Plats {2}: {0}#{1}", temp[0], temp[1], index);
    }
}

//metod för att lägga till ett fordon på en parkeringsplats
//Hanterar både MC (kan dela plats med en annan MC), och en bil per plats.
void AssignVehicleToParking(string vehicleType, string regNumber, int parkingIndex)
{
    if (vehicleType == "MC")
    {
        if (string.IsNullOrEmpty(parkingSpaces[parkingIndex]))
        {
            parkingSpaces[parkingIndex] = vehicleType + delimiter + regNumber;
        }
        else if (parkingSpaces[parkingIndex].StartsWith("MC#") && !parkingSpaces[parkingIndex].Contains(mcDelimiter))
        {
            parkingSpaces[parkingIndex] += mcDelimiter + vehicleType + delimiter + regNumber;
        }
        // Annars: platsen är full för MC
    }
    else // CAR
    {
        parkingSpaces[parkingIndex] = vehicleType + delimiter + regNumber;
    }
}
void DisplayParking()
{

    for (int i = 1; i < parkingSpaces.Length; i++) // Börja på 1, plats 0 är testdata
    {
        if (!string.IsNullOrEmpty(parkingSpaces[i]))
        {
            Console.WriteLine(PrintParkingSpaceInfo(i));
        }
        else
        {
            Console.WriteLine($"Plats {i}: \t(ledig)");
        }
    }
    Console.ReadKey();
}

//Flyttar ner denna då den inte används för tillfället /SR
// En metod som skriver ut info om fordon. Kan användas i andra metoder:
//string PrintVehicleInfo(int index)
//{
//    string[] temp = parkingSpaces[index].Split('#');
//    return String.Format("{0} {1} står på plats: {2}", temp[0], temp[1], index);
//}

/*void DisplayLog(int parkingIndex) //kanske ska använda denna metod för själva parkeringsöversikten?
                  //HJH: Låter smart! Kanske kan visa loggen unden den visuella representationen av hela parkeringen? 
                  //behöver troligtvis modifieras en aning då den är kopplad till SaveLog(); vilket jag tog bort från RegistreraFordon()
{
    if (logs.Count == 0)
    {
        if (vehicleType == "MC")
        {
            if (string.IsNullOrEmpty(parkingSpaces[parkingIndex]))
            {
                parkingSpaces[parkingIndex] = vehicleType + delimiter + regNumber;
            }
            else if (parkingSpaces[parkingIndex].StartsWith("MC#") && !parkingSpaces[parkingIndex].Contains(mcDelimiter))
            {
                parkingSpaces[parkingIndex] += mcDelimiter + vehicleType + delimiter + regNumber;
            }
            // Annars: platsen är full för MC
        }
        else // CAR
        {
            parkingSpaces[parkingIndex] = vehicleType + delimiter + regNumber;
        }
        PrintParkingSpaceInfo(parkingIndex);
    }
}*/


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

// la in denna för att kunna testa programmet och se om fordonen registreras korrekt
