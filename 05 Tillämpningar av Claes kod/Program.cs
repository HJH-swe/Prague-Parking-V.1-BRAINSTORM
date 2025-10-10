// Deklarerar variabler som behövs genom hela programmet
using System;

string? vehicleType;
string regNumber;
string delimiter = "#";
string mcDelimiter = "|";
string[] parkingSpaces = new string[101];       // Skapar 101 element (0-100). P-plats 0 ska aldrig användas --> 100 p-platser
bool displayMenu = true;
int menuInput;

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
        //kontrollera om inmatningen är tom [ENTER] eller skriver något som inte är en siffra.
        string input = Console.ReadLine();
        if (!int.TryParse(input, out menuInput))
        {
            Console.WriteLine("\tOgiltigt menyval. Välj i menyn genom att trycka på siffertangenterna.\n\n");
            Console.ReadLine();
            return;
        }

        switch (menuInput)
        {
            case 1:
                {
                    vehicleType = VehicleType(); //lägger till denna så att man kommer tillbaka till menyn för att registrera fordon, inte huvudmeny.
                    if (vehicleType == null)
                    {
                        break;
                    }
                    RegisterParking(vehicleType);
                    Console.ReadLine();
                    break;
                }
            case 2:
                {
                    bool validSearch = false;
                    //Använder do-while så att detta kommer upp minst en gång
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("\t ~~ SÖK EFTER FORDON ~~");
                        Console.Write("\n\tAnge registreringsnummer (eller tryck [Enter] för huvudmenyn): ");

                        string searchRegNumber = Console.ReadLine().ToUpper();
                        if (string.IsNullOrEmpty(searchRegNumber))
                        {
                            // Användaren trycker Enter för att gå tillbaka till huvudmenyn
                            break;
                        }
                        else if (searchRegNumber.Length > 10)
                        {
                            Console.WriteLine("\n\tOgiltigt registreringsnummer. \n\tKontrollera registreringsnumret och sök igen.");
                            Thread.Sleep(2000);// En paus innan programmet stängs
                        }
                        else
                        {
                            validSearch = true;
                            SearchVehicle(searchRegNumber);
                        }
                    }
                    while (!validSearch);
                    break;
                }
            case 3:
                {
                    Console.Clear();
                    Console.WriteLine("\t ~~ FLYTTA FORDON ~~");
                    // Bild över hela parkeringen -> lättare för användaren att välja p-platser att flytta från och till
                    VisualAllParkingSpaces(parkingSpaces);

                    Console.Write("\nAnge p-plats att flytta fordonet från (eller tryck på [ENTER] för huvudmenyn): ");
                    input = Console.ReadLine();

                    if (String.IsNullOrEmpty(input))
                    {
                        break;
                    }
                    else
                    {
                        int.TryParse(input, out int fromSpot);
                        Console.Write("\nAnge p-plats att flytta fordonet till: ");
                        int.TryParse(Console.ReadLine(), out int toSpot);
                        MoveVehicle(fromSpot, toSpot);
                        break;
                    }
                }
            case 4:
                {
                    Console.Clear();
                    Console.WriteLine("\t ~~ CHECKA UT FORDON ~~");
                    Console.Write("\nAnge regnummer för utcheckning (eller tryck [ENTER] för huvudmenyn): ");

                    regNumber = Console.ReadLine().ToUpper();
                    if (string.IsNullOrEmpty(regNumber))
                    {
                        break;
                    }
                    string result = CheckOutVehicle(regNumber);
                    Console.WriteLine(result);
                    Console.ReadLine();
                    break;
                }

            case 5:
                {
                    // Claes - detta är en VG-funktion
                    Console.Clear();
                    Console.WriteLine("\t\t~~ ÖVERSIKT ÖVER PARKERINGEN ~~");
                    VisualAllParkingSpaces(parkingSpaces);
                    DisplayParking();
                    break;
                }
            case 6:
                {
                    Console.WriteLine("\nProgrammet avslutas...\n\n");
                    Thread.Sleep(1000);
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
        Console.WriteLine("\tEtt oväntat fel inträffade.\n\n");
        Console.ReadLine();
    }

}

//kontrollerar om regnummer redan finns registrerad
bool IsRegNumberRegistered(string regNumber)
{
    for (int i = 1; i < parkingSpaces.Length; i++)
    {
        if (parkingSpaces[i] != null && parkingSpaces[i].Contains(regNumber))
        {
            return true;
        }
    }
    return false;
}

//Metod för att registrera parkering
void RegisterParking(string? vehicleType)
{
    // Claes - det här är en VG-uppgift: felhantering
    //Kontrollerar att registreringsnumret inte är ett tomt värde och max 10 tecken. Visar felmeddelande vid felaktig inmatning.
    string? input;
    do
    {
        Console.Write("\n\tAnge registreringsnummer: ");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input) || input.Length > 10)
        {
            Console.WriteLine("\n\tRegistreringsnumret måste innehålla 1-10 tecken. Vänligen försök igen");
            Thread.Sleep(2000);
            vehicleType = VehicleType();
            if (vehicleType == null)
            {
                return;
            }
        }
    }
    while (string.IsNullOrEmpty(input) || input.Length > 10);

    regNumber = input.ToUpper();

    if (IsRegNumberRegistered(regNumber))

    {
        Console.WriteLine($"\n\tFordon med registreringsnummer {regNumber.ToUpper()} är redan registrerat.");
        Console.ReadLine();
        return;
    }
    int parkingIndex = -1; // initierar en hjälpvariabel som håller reda på vilken p-plats fordonet tilldelas. -1 betyder att ingen plats har hittats än

    // Claes - det här är en VG-uppgift:
    // En inbyggd optimeringsrutin som ser till att 2 MC alltid parkeras ihop
    // Personalen behöver inte gå och flytta MC senare
    if (vehicleType == "MC") //Om en MC registreras
    {
        for (int i = 1; i < parkingSpaces.Length; i++)
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
            {
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
}

//Metod med menyval vid registrering av fordon
static string? VehicleType()
{
    Console.Clear();
    Console.WriteLine("\t ~~ REGISTRERA FORDON ~~");
    Console.WriteLine("\n\t[1] Bil\n\t[2] MC");
    Console.Write("\n\tVälj fordonstyp (eller tryck [ENTER] för huvudmenyn): ");

    //återgår till huvudmenyn om användaren trycker [ENTER]
    string input = Console.ReadLine();
    if (string.IsNullOrEmpty(input))
    {
        return null;
    }

    if (int.TryParse(input, out int menuSelect))
    {
        switch (menuSelect)
        {
            case 1:
                return "BIL";
            case 2:
                return "MC";
            default:
                {
                    Console.Write("\n\n\tOgiltigt val. Tryck [1] för bil, [2] för MC eller [3] för huvudmenyn...\n");
                    Thread.Sleep(1500);
                    return VehicleType();
                }
        }
    }
    else
    {
        Console.Write("\n\n\tOgiltigt val. Tryck [1] för bil, [2] för MC eller [3] för huvudmenyn...\n");
        Thread.Sleep(1500);
        return VehicleType();
    }
}

//Metod för att lägga till ett fordon på en parkeringsplats
//Hör delvis till VG-delen - MC (kan dela plats med en annan MC), och en bil per plats.
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
    }
    else
    {
        parkingSpaces[parkingIndex] = vehicleType + delimiter + regNumber;
    }

}

//Metod som skriver ut info om p-plats (t.ex. om det står 2 mc eller ett fordon på platsen)
string PrintParkingSpaceInfo(int index)
{
    //Om det står 2 MC på platsen kommer det finnas | i strängen
    if (parkingSpaces[index].Contains('|'))
    {
        string[] splitMC = parkingSpaces[index].Split('|');
        string[] temp0 = splitMC[0].Split("#");
        string[] temp1 = splitMC[1].Split("#");
        return String.Format($"\n\tPlats {index}: {temp0[0]}#{temp0[1]} {mcDelimiter} {temp1[0]}#{temp1[1]}");
    }
    // Om det inte står 2 MC på platsen --> ett fordon på p-platsen
    else
    {
        string[] temp = parkingSpaces[index].Split('#');
        return String.Format("\n\tPlats {2}: {0}#{1}", temp[0], temp[1], index);
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
    if (emptyParking)
    {
        Console.WriteLine("\n\n\tDet finns inga parkerade fordon.");
    }
    // Om fordonet fortfarande inte hittats efter for-loopen och parkeringen inte är tom
    if (!vehicleFound && !emptyParking)
    {
        Console.WriteLine("\n\n\tFordonet hittades inte. \n\tKontrollera registreringsnumret och sök igen.");
    }
    Console.ReadLine();
}


//Metod för att flytta fordon från en plats till en annan.
void MoveVehicle(int fromSpot, int toSpot)
{
    if (!IsValidIndex(fromSpot) || !IsValidIndex(toSpot))
    {
        Console.WriteLine("\nOgiltigt platsnummer!");
        Thread.Sleep(2000);
        return;
    }
    if (string.IsNullOrEmpty(parkingSpaces[fromSpot]))
    {
        Console.WriteLine($"\nIngen bil eller MC finns på plats {fromSpot}.");
        Thread.Sleep(2000);
        return;
    }

    Console.Clear();
    bool splitRequired = false;
    string vehicleToMove = parkingSpaces[fromSpot];
    string[] vehiclesAtFrom = vehicleToMove.Split('|');

    //Om det står mer än ett fordon på platsen
    if (vehiclesAtFrom.Length > 1)
    {
        splitRequired = true;
        Console.WriteLine($"\n\nPlats {fromSpot} innehåller flera fordon.");
        Console.WriteLine(PrintParkingSpaceInfo(fromSpot));

        bool validRegNumber = false;
        do
        {
            Console.Write("\nAnge registreringsnumret på fordonet du vill flytta: ");
            string regNumber = Console.ReadLine().ToUpper();
            if (vehiclesAtFrom[0].Contains(regNumber))
            {
                vehicleToMove = vehiclesAtFrom[0];
                parkingSpaces[fromSpot] = vehiclesAtFrom[1];
                validRegNumber = true;

            }
            else if (vehiclesAtFrom[1].Contains(regNumber))
            {
                vehicleToMove = vehiclesAtFrom[1];
                parkingSpaces[fromSpot] = vehiclesAtFrom[0];
                validRegNumber = true;
            }
            else
            {
                Console.WriteLine("\n\tOgiltigt registreringsnummer. \n\tVänligen skriv in det igen.");
                validRegNumber = false;
            }
        } while (!validRegNumber);
    }
    // Bara ett fordon på platsen fromSpot -> tar bort från fromSpot
    else
    {
        vehicleToMove = vehiclesAtFrom[0];
        parkingSpaces[fromSpot] = null;
    }

    if (string.IsNullOrEmpty(parkingSpaces[toSpot]))
    {
        // Parkeringsplats tom → flytta direkt
        parkingSpaces[toSpot] = vehicleToMove;
        parkingSpaces[fromSpot] = splitRequired ? parkingSpaces[fromSpot] : null; // ser till att platsen blir tom om ett fordon flyttas, eller att rätt MC blir kvar om två MC delar plats
        Console.WriteLine($"\n\nFordon {vehicleToMove} flyttades från plats {fromSpot} till {toSpot}.");
    }
    else
    {
        string[] vehiclesAtTo = parkingSpaces[toSpot].Split('|');

        // Om bil finns på parkeringsplatsen → inte tillåtet
        if (parkingSpaces[toSpot].Contains("BIL"))
        {
            Console.WriteLine($"\n\nKan inte flytta till plats {toSpot}, bil upptar platsen.");
            if (splitRequired) //om det stod två MC på platsen och flytt misslyckas, sätt tillbaka fordonet till ursprungsplatsen
            {
                parkingSpaces[fromSpot] = vehiclesAtFrom[0] + "|" + vehiclesAtFrom[1];
            }
            else
            {

                parkingSpaces[fromSpot] = vehicleToMove;
            }
            Console.ReadKey();
            return;
        }

        // Om MC finns på parkeringsplatsen: Om bara en MC → flytta dit MC.
        if (vehiclesAtTo.Length == 1 && vehicleToMove.StartsWith("MC"))
        {
            // Flytta MC och kombinera
            parkingSpaces[toSpot] = parkingSpaces[toSpot] + "|" + vehicleToMove;

            Console.WriteLine($"\n\nMC {vehicleToMove} flyttades från plats {fromSpot} till {toSpot} (nu 2 MC på plats {toSpot}).");
        }
        // Annars är platsen upptagen → flytten misslyckas
        else
        {
            Console.WriteLine($"\n\nPlats {toSpot} är redan full. Kunde inte flytta fordon.");
            parkingSpaces[fromSpot] = vehicleToMove;
        }
    }
    Console.ReadLine();
}

bool IsValidIndex(int index)
{
    return index > 0 && index <= 100;
}

string CheckOutVehicle(string regNumber)
{
    for (int i = 1; i < parkingSpaces.Length; i++)
    {
        if (parkingSpaces[i] != null && parkingSpaces[i].Contains(regNumber))      // Hittar vi fordonet med regNumber så går vi vidare
        {
            if (parkingSpaces[i].Contains('|'))         //Hittar vi fordon med "|", då är det 2 MC på platsen
            {
                string[] splitMC = parkingSpaces[i].Split('|');     // Delar upp strängen i två delar för att checka korrekt MC

                if (splitMC[0].Contains(regNumber))
                {

                    parkingSpaces[i] = splitMC[1];  //Denna MC ska checkas ut
                }
                else if (splitMC[1].Contains(regNumber))
                {

                    parkingSpaces[i] = splitMC[0];  //Denna MC ska checkas ut
                }
                return $"Fordon {regNumber} har checkats ut från plats {i}.";
            }
            else
            {
                parkingSpaces[i] = null; // tar bort fordonet från platsen
                return $"Fordon {regNumber} har checkats ut från plats {i}.";
            }
        }
    }
    return $"Fordon med registreringsnummer {regNumber} hittades inte.";  
}

void DisplayParking()
{
    bool isParked = false;

    Console.WriteLine("\n\n\t~ Incheckade fordon ~");
    for (int i = 1; i < parkingSpaces.Length; i++) 
    {
        if (!string.IsNullOrEmpty(parkingSpaces[i]))
        {
            Console.WriteLine(PrintParkingSpaceInfo(i));
            isParked = true;
        }
    }
    if (!isParked)
    {
        Console.WriteLine("\n\tDet finns inga parkerade fordon");
    }
    Console.Write("\n\tTryck på en tangent för att återgå till huvudmenyn...");
    Console.ReadKey();
}

void VisualAllParkingSpaces(string[] parkingSpaces)
{
    Console.Write("\n\tLediga p-platser är ");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("gröna ");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write("\tHalvfulla p-platser (med 1 MC) är ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("gula ");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write("\tFyllda p-platser (med 1 bil eller 2 MC) är ");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("röda \n\n");


    string[,] parkingMatrix = new string[10, 10];
    // Använder en räknare som börjar på 1 (för att p-plats 0 inte ska användas)
    int counter = 1;

    // Lägger in strängarna från vektorn parkingSpaces i matrisen
    for (int i = 0; i < parkingMatrix.GetLength(0); i++)
    {
        for (int j = 0; j < parkingMatrix.GetLength(1); j++)
        {
            // Räknaren används som index på parkingSpaces[] 
            parkingMatrix[i, j] = parkingSpaces[counter];
            counter++;
        }
    }
    
    // Räknaren börjar om på 1 (för att p-platserna börjar på 1)
    counter = 1;
    for (int i = 0; i < parkingMatrix.GetLength(0); i++)
    {
        for (int j = 0; j < parkingMatrix.GetLength(1); j++)
        {
            if (parkingMatrix[i, j] != null)
            {
                // OM det står en bil eller två mc på platsen --> upptagen
                if (parkingMatrix[i, j].Contains("BIL") || parkingMatrix[i, j].Contains('|'))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\t" + counter.ToString().PadLeft(4));         
                }
                // ANNARS OM det står en mc på platsen --> halvfylld
                else if (parkingMatrix[i, j].Contains("MC") && !(parkingMatrix[i, j].Contains('|')))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\t" + counter.ToString().PadLeft(4));
                }
            }
            // ANNARS: ingen bil eller mc på platsen --> tom
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\t" + counter.ToString().PadLeft(4));
            }
            counter++;
        }
        Console.WriteLine();
    }
    Console.ForegroundColor = ConsoleColor.Gray;
}