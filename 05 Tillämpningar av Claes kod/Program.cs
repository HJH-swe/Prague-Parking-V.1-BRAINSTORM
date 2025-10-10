// Deklarerar variabler som behövs genom hela programmet
string? vehicleType;
string regNumber;
string delimiter = "#";
string mcDelimiter = "|";
string[] parkingSpaces = new string[101];       // Skapar 101 element (0-100). P-plats 0 ska aldrig användas --> 100 p-platser
bool displayMenu = true;
int menuInput;
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
        menuInput = int.Parse(Console.ReadLine());

        switch (menuInput)
        {
            case 1:
                {
                    //Console.WriteLine("\t ~~ REGISTRERA FORDON ~~"); //flyttade in denna rad i VehicleType(); då konsolen tidigare rensades och tog bort denna rad
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
                    //kan vi ändra gränssnittet så att tidigare text rensas och man 
                    // HJH: Uppdaterade sökfunktionen, la till att man måste skriva in ett regnummer att söka på.
                    // Men koden är lite stökig. Är uppdateringen onödig? Ska vi ta bort?
                    bool validSearch = false;

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
                            Thread.Sleep(2000);
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
                    // La in "bilden" över hela parkeringen -> lättare för användaren att välja p-platser att flytta från och till
                    VisualAllParkingSpaces(parkingSpaces);

                    Console.Write("\nAnge p-plats att flytta fordonet från: ");
                    int.TryParse(Console.ReadLine(), out int fromSpot);
                    Console.Write("\nAnge p-plats att flytta fordonet till: ");
                    int.TryParse(Console.ReadLine(), out int toSpot);
                    MoveVehicle(fromSpot, toSpot);
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
                    // Claes - detta är en VG-funktion
                    Console.Clear();
                    Console.WriteLine("\t\t~~ ÖVERSIKT ÖVER PARKERINGEN ~~");
                    VisualAllParkingSpaces(parkingSpaces);
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
        Console.WriteLine("\tOgiltigt menyval. Välj i menyn genom att trycka på siffertangenterna.\n\n");
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
    int parkingIndex = -1; // initierar en ny int variabel med värdet -1 för att hålla reda på vilken p-plats fordonet tilldelas. -1 betyder att ingen plats har hittats än
                           //om det inte finns en ledig plats förblir värdet -1 och ett felmeddelande visas, används även för att visa info om var fordonet parkerats

    // Claes - det här är en VG-uppgift:
    // En inbyggd optimeringsrutin som ser till att 2 MC alltid parkeras ihop
    // Personalen behöver inte gå och flytta MC senare
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

//Metod med menyval vid registrering av fordon
static string? VehicleType()
{
    Console.Clear();
    Console.WriteLine("\t ~~ REGISTRERA FORDON ~~"); //lägger in denna här istället för i MainMenu(); då konsolen rensas och denna rad försvinner då.
    Console.WriteLine("\n\t[1] Bil\n\t[2] MC");
    Console.Write("\n\tVälj fordonstyp eller tryck [ENTER] för att återgå: ");

    //denna kod kommer att låta användaren återgå till huvudmenyn om inmatningen är tom
    string input = Console.ReadLine();
    if (string.IsNullOrEmpty(input))
    {
        return null;
    }

    if (int.TryParse(input, out int menuSelect)) //om input är giltig siffra - deklareras en int menuSelect
                                                 //try
    {
        //    int menuSelect = int.Parse(Console.ReadLine());
        switch (menuSelect)
        {
            case 1:
                return "CAR"; //bör vi ändra till BIL ?
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

//Metod som skriver ut info om p-plats (t.ex. om det står 2 mc eller ett fordon på platsen)
string PrintParkingSpaceInfo(int index)
{
    //Om det står 2 MC på platsen kommer det finnas | i strängen
    if (parkingSpaces[index].Contains('|'))
    {
        string[] splitMC = parkingSpaces[index].Split('|');
        string[] temp0 = splitMC[0].Split("#");
        string[] temp1 = splitMC[1].Split("#");
        return String.Format($"\n\tPlats {index}: {temp0[0]}#{temp0[1]} {mcDelimiter} {temp1[0]}#{temp1[1]}"); //la till mcDelimiter
                                                                                                               // HJH: ändrade till temp1[] för att skriva ut andra mc:n
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
    // HJH: La in felmeddelande om parkeringen är tom (SR förslag). 
    // Man måste ändå skriva in ett regnummer att söka på (görs innan metoden). Inte jättesnyggt, men kanske duger?
    if (emptyParking)
    {
        Console.WriteLine("\n\n\tDet finns inga parkerade fordon.");
    }

    // Om fordonet fortfarande inte hittats efter for-loopen och parkeringen inte är tom
    if (!vehicleFound && !emptyParking)
    {
        Console.WriteLine("\n\n\tFordonet hittades inte. \nKontrollera registreringsnumret och sök igen.");
    }
    Console.ReadLine();
}


//Metod för att flytta fordon från en plats till en annan.
void MoveVehicle(int fromSpot, int toSpot)
{

    if (!IsValidIndex(fromSpot) || !IsValidIndex(toSpot))
    {
        Console.WriteLine("\nOgiltigt platsnummer!");
        Thread.Sleep(1500);
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
        // Målruta tom → flytta direkt
        parkingSpaces[toSpot] = vehicleToMove;
        Console.WriteLine($"\n\nFordon {vehicleToMove} flyttades från plats {fromSpot} till {toSpot}.");
    }
    else
    {
        string[] vehiclesAtTo = parkingSpaces[toSpot].Split('|');

        // Fall: Bil finns på målrutan → inte tillåtet
        if (parkingSpaces[toSpot].Contains("CAR"))
        {
            Console.WriteLine($"\n\nKan inte flytta till plats {toSpot}, bil upptar platsen.");
            parkingSpaces[fromSpot] = vehicleToMove;
            Console.ReadKey();
            return;
        }

        // Fall: MC finns på målrutan: Om bara en MC → flytta dit MC.
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




/* För att checka ut ett fordon behöver jag först anropa att fordonet för att sedan konvertera det till att checka ut det, så första steget antar jag är att hitta fordonet tex via regnummer för att avgöra om det är en MC/BIL */
// String: MC#ABC123|MC#CDE456      -->  splitMC[0] = MC#ABC123  splitMC[1] = MC#CDE456
// --> string1 = MC#ABC123  string2 = MC#CDE456
// OM platsen innehåller "|" (då står 2 mc på platsen, men vi vill ta bort 1)
// Kod som bara tar bort rätt fordon
// Splitta strängen och ta bort rätt del

// ANNARS (då står det bara ett fordon på platsen)
// kod som nollställer platsen

string CheckaOut(string regNumber, string[] parkingSpaces)
{
    for (int i = 1; i < parkingSpaces.Length; i++)
    {
        if (parkingSpaces[i].Contains(regNumber))       //Hittar vi fordonet med regNumber så går vi vidare
        {
            if (parkingSpaces[i].Contains('|'))         //Hittar vi fordon med "|", då är det 2 MC på platsen

            {
                string[] splitMC = parkingSpaces[i].Split('|');     // Delar upp strängen i två delar för att checka korrekt MC
                                                                    // splitMC[0] = MC#ABC123  splitMC[1] = MC#CDE456

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

            if (parkingSpaces[i].StartsWith("MC#"))     //Tar bort en MC från platsen
            {
                parkingSpaces[i] = null; // tar bort fordonet från platsen
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
        for (int i = 1; i < parkingSpaces.Length; i++) // Börja på 1, plats 0 är testdata
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
                // Räknaren används som index på parkingSpaces[] - för att få strängarna på rätt plats [i, j]
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
                    if (parkingMatrix[i, j].Contains("CAR") || parkingMatrix[i, j].Contains('|'))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\t" + counter.ToString().PadLeft(4));         // Hittade .PadLeft() på nätet för snygg formatering
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


        // HJH: Jag klipper ut och sparar delar metoden MoveVehicle här .
        // Har krånglat till metoden hemskt mycket, och ska försöka reda ut det.
        // Men sparar kod vi redan hade i fall man måste stoppa in det igen.

        /* Från rad 430 - precis efter //Flytta MC och kombinera - parkingSpaces[toSpot] = parkingSpaces[toSpot] + "|" + vehicleToMove;

                    // Borttagning av MC från fromSpot
                    string[] fromVehicles = parkingSpaces[fromSpot].Split('|');

                    // Om det stod 2 MC på fromSpot
                    if (fromVehicles.Length == 2)
                    {
                        // Ta bort en MC, lämna kvar den andra
                        if (fromVehicles[0] == vehicleToMove)
                        {
                            parkingSpaces[fromSpot] = fromVehicles[1];
                        }
                        else if (fromVehicles[1] == vehicleToMove)
                        {
                            parkingSpaces[fromSpot] = fromVehicles[0];
                        }
                    }
                    else
                    {
                        // Endast en MC på platsen, ta bort den
                        parkingSpaces[fromSpot] = null;
                    }*/

/* HJH: Den här metoden används inte - lägger den här 
void PrintGarage()
{
    Console.WriteLine("\n--- Parkeringsstatus ---");
    for (int i = 0; i < parkingSpaces.Length; i++)
    {
        if (!string.IsNullOrEmpty(parkingSpaces[i]))
            Console.WriteLine($"Plats {i + 1}: {parkingSpaces[i]}");
    }
    Console.WriteLine("------------------------\n");
}*/