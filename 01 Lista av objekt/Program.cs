
// Vektorn tar emot fordon, men representerar p-platser
// Skapar 101 element (0-100), och fyller vehicles[0] med testdata. Ska aldrig användas av personalen
Vehicles[] vehicles = new Vehicles[101];

vehicles[0].type = "Car";
vehicles[0].regNumber = "ABC123";
vehicles[0].dateTimeArrival = "";
vehicles[0].dateTimeDeparture = "";

Console.WriteLine("Test. Lägg till ett nytt fordon. ");

// Försöker lösa så att informationen hamnar på en viss plats i vektorn
for (int i = 0; i < vehicles.Length; i++)
{
    if (vehicles[i].type != null)     // Här kanske man får ändra sen om type = MC
    {
        continue;
    }
    else
    {
        VehicleRegistration(vehicles, i);
        break;
    }

}


/*
Console.WriteLine("Test. Skriv ut alla fordon.");
PrintAllVehicles(vehicles);*/


Console.WriteLine("\n\nTryck på valfri tangent för att avsluta...");
Console.ReadKey();

void VehicleRegistration(Vehicles[] vehicles, int index)
{
    // Ska anropa andra metoder här för att fylla i .type, .regNumber etc.
    vehicles[index].type = VehicleType();
    vehicles[index].regNumber = VehicleRegNumber();
    Console.WriteLine($"Parkera fordonet på plats: {index}");
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
    Console.Write("\n\nFyll i fordonets registreringsnummer (avsluta med [Enter]): ");
    string regNumber = Console.ReadLine();

    if (regNumber == "")
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
    Console.WriteLine();
    Console.WriteLine(vehicles[index].type);
    Console.WriteLine(vehicles[index].regNumber);
    Console.WriteLine($"Parkera på plats: {index}");
}

void PrintAllVehicles(Vehicles[] vehicles)
{
    Console.Clear();
    Console.WriteLine("\nLista över alla parkerade fordon:\n");

    //TODO: Innan inlämning: Ändra till i = 1 för att inte ta med test-fordonet
    for (int i = 0; i < vehicles.Length; i++)
    {
        // Lägger till if-sats för att inte skriva ut alla tomma p-platser
        if (vehicles[i].type != "")
        {
            Console.WriteLine("Fordonstyp: {0}", vehicles[i].type);
            Console.WriteLine("Registreringsnummer: {0}", vehicles[i].regNumber);
            Console.WriteLine("Parkeringsplats: {0}", i);
            Console.WriteLine();
        }
    }
}

internal struct Vehicles
{
    internal string type;
    internal string regNumber;
    internal string dateTimeArrival;
    internal string dateTimeDeparture;
}