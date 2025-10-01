
// Vektorn tar emot fordon, men representerar p-platser
// Skapar 101 element (0-100), och fyller vehicles[0] med testdata. Ska aldrig användas av personalen
Vehicles[] vehicles = new Vehicles[101];

vehicles[0].type = "Car";
vehicles[0].regNumber = "ABC123";
vehicles[0].dateTimeArrival = "";
vehicles[0].dateTimeDeparture = "";

Console.WriteLine(vehicles);
Console.WriteLine("Test. Lägg till ett nytt fordon. ");

// Försöker lösa så att informationen hamnar på en viss plats i vektorn
FindFreeSpace();

Console.WriteLine("Test. Sök efter fordon.");
SearchForVehicle(vehicles);


Console.WriteLine("\n\nTryck på valfri tangent för att avsluta...");
Console.ReadKey();

void FindFreeSpace()
{
    for (int i = 0; i < vehicles.Length; i++)
    {
        if (vehicles[i].type != null)     // Här kanske man får ändra sen om type = MC
        {
            continue;
        }
        else
        {
            VehicleRegistration(vehicles, i);   // Kanske borde returnera ett värde istället? 
            break;
        }

    }
}

void VehicleRegistration(Vehicles[] vehicles, int index)
{
    // Ska anropa andra metoder här för att fylla i .type, .regNumber etc.
    vehicles[index].type = VehicleType();
    vehicles[index].regNumber = VehicleRegNumber();
    Console.WriteLine($"\nParkera fordonet på plats: {index}");
}

static string VehicleType()
{
    Console.WriteLine("\nVälj fordonstyp med siffertangenterna");
    Console.WriteLine("\t[1] Bil");
    Console.WriteLine("\t[2] MC");
    int menuSelect = int.Parse(Console.ReadLine());
    //TODO: Lägg till funktion, fånga upp om användaren inte skriver en siffra

    switch (menuSelect)
    {
        case 1:
            return "Car";

        case 2:
            return "MC";

        default:
            Console.WriteLine("\n\nOgiltigt menyval. Tryck [1] för bil, eller [2] för MC.\n");
            return VehicleType();
    }
}


static string VehicleRegNumber()
{
    Console.Write("\nFyll i fordonets registreringsnummer (avsluta med [Enter]): ");
    string regNumber = Console.ReadLine().ToUpper();        // ToUpper gör alla bokstäver till stora bokstäver --> regnumren formateras likadant 

    if (regNumber == "") //TODO: Finns det fler ogiltiga villkor?
    {
        Console.WriteLine("Ogitligt registreringsnummer. Vänligen försök igen");
        return VehicleRegNumber();
    }
    else
    {
        return regNumber;   
    }
}

void PrintVehicleInfo(Vehicles[] vehicles, int index)       
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine(vehicles[index].type);
    Console.WriteLine(vehicles[index].regNumber);
    Console.WriteLine($"Finns på plats: {index}");
}

void PrintAllVehicles(Vehicles[] vehicles)
{
    Console.Clear();
    Console.WriteLine("\nLista över alla parkerade fordon:\n");

    //TODO: Innan inlämning: Ändra till i = 1 för att inte ta med test-fordonet
    for (int i = 0; i < vehicles.Length; i++)
    {
        // Lägger till if-sats för att inte skriva ut alla tomma p-platser
        if (vehicles[i].type != null)
        {
            Console.WriteLine("Fordonstyp: {0}", vehicles[i].type);
            Console.WriteLine("Registreringsnummer: {0}", vehicles[i].regNumber);
            Console.WriteLine("Parkeringsplats: {0}", i);
            Console.WriteLine();
        }
    }
}

void SearchForVehicle(Vehicles[] vehicles)
{
    Console.WriteLine("\t -- SÖK EFTER FORDON --");
    Console.WriteLine("Skriv in fordonets registreringsnummer");
    string searchRegNumber = Console.ReadLine().ToUpper();          // ToUpper igen
    bool vehicleFound = false;

    // Ändra till i = 1 för att inte få med test-fordonet
    for (int i = 0; i < vehicles.Length; i++)
    {
        if (vehicles[i].regNumber != null)
        {
            if (vehicles[i].regNumber == searchRegNumber)
            {
                PrintVehicleInfo(vehicles, i);
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
    if (vehicleFound == false)
    {
        Console.WriteLine("Fordonet hittades inte. \nKontrollera registreringsnumret och sök igen.");
    }

}

internal struct Vehicles
{
    internal string type;
    internal string regNumber;
    internal string dateTimeArrival;
    internal string dateTimeDeparture;
}