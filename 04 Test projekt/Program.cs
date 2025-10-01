using System.ComponentModel.DataAnnotations;

string[] ParkingGarage = new string[101];

Console.Write("Ange fordonstyp: ");
string fordonsTyp = Console.ReadLine();

Console.Write("Ange registreringsnummer : ");
string regNumber = Console.ReadLine();


bool displayMenu = true;
do
{
    Console.Clear();
    MainMenu();
    Console.Write("\n\tAnge ett menyval: ");

    if (!int.TryParse(Console.ReadLine(), out int inmatning))
    {
        Console.WriteLine("\n\tOgiltigt val, vänligen ange ett menyval mellan 1-7");
        continue; //bryter loopen
    }
    UserInput(inmatning);
} while (displayMenu);


//METODER

//Meny
void MainMenu()
{
    Console.WriteLine("\t~ Parque Parking ~\n");

    Console.WriteLine("\t1) Registrera parkering");
    Console.WriteLine("\t2) Sök fordon");
    Console.WriteLine("\t3) Ändra parkering");
    Console.WriteLine("\t4) Checka ut fordon");
    Console.WriteLine("\t5) Översikt parkering");
    Console.WriteLine("\t6) Historik/Logg");
    Console.WriteLine("\t7) Avsluta");
}

//Menyval
void UserInput(int input)
{

    switch (input)
    {
        case 1:
            {
                Console.Clear();
                //RegisterParking();
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
                ChangeParking();
                break;
            }
        case 4:
            {
                Console.Clear();
                CheckoutVehicle();
                break;
            }
        case 5:
            {
                Console.Clear();
                DisplayParking();
                break;
            }
        case 6:
            {
                Console.Clear();
                DisplayLog();
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

//Registrera parkering:
void RegisterParking(string vehicle, string regNumber, int ParkingNumber)
{
    Console.Write("Ange fordonstyp: ");
    vehicle = Console.ReadLine();

    Console.Write("Ange registreringsnummer : ");
    regNumber = Console.ReadLine();

    string skiljetecken = "#";

    for (int i = 1; i < ParkingGarage.Length; i++)
    {
        if (ParkingGarage[i] == null)
        {
            ParkingGarage[ParkingNumber] = vehicle + skiljetecken + regNumber;
            Console.WriteLine($"Fordon: {vehicle} {skiljetecken} {regNumber} parkeras på plats nr: [i] ");
            return;
        }

    }
}

//Sök fordon:
void SearchVehicle()
{
    Console.WriteLine("\tSök fordon");
}

//Flytta parkering:
void ChangeParking()
{
    Console.WriteLine("\tÄndra parkering");
}

void CheckoutVehicle()
{
    Console.WriteLine("\tChecka ut fordon");
}

//Översikt parkering:
void DisplayParking()
{
    Console.WriteLine("\tÖversikt parkering");
}

//Historik/Logg:
void DisplayLog()
{
    Console.WriteLine("\tHistorik/Logg");
}


//KLASSER
class Vehicle
{
    public string Type { get; set; }
    public string RegNumber { get; set; }
}

class Parkingsession
{
    DateTime StartTime { get; set; } = DateTime.Now; //starttid
    DateTime? EndTime { get; set; }  //sluttid //är null tills den checkar ut
    Vehicle Vehicle { get; set; }
    Parking ParkingNumber { get; set; }
}

class Parking
{
    //(Car) Fordon
    //Bilplatser
    //status - ledig, halvtom, upptagen
}

class Log
{
    DateTime LogTime { get; set; } = DateTime.Now;
    string LogEntry = "";
    Vehicle Vehicle { get; set; }
}

/*
//Be användare om inmatning 
//Felhantering: Kontrollera inmatning (endast heltal, ej textsträngar)
//Om giltig inmatning : visa meny
//Använd switch för menyval
    Menyval
    1.Registrera parkering
    2. Sök fordon
    3. Flytta fordon
    4. Checka ut fordon
    5. Översikt parkering
    6. Historik/logg
    7. Avsluta programmet
*/
/* Fordon:
   RegNr    
   FordonsTyp (BIL/MC)
       BIL : 1 parkeringsplats
       MC  : 1 eller 2 parkeringsplatser

Parkering:
   Plats
   Fordon
   Starttid
   Sluttid

Parkeringsplats:
   Status: Ledig, upptagen, halvtom
   Fordon med regnummer
   0 eller 1 bilplats
   0, 1 eller 2 MC-plats
*/


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
