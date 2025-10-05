// Förslag på metod: När ett nytt fodon registrerats, ha en metod som kollar om det är en MC.
// Gå igenom parkerade fordon och kolla om det finns en plats med 1 MC. '
// Säg till på en gång att parkera den nya MC på samma plats.
// Eventuella svårigheter: Se till att det bara blir 2 MC på samma plats,
// alltså att programmet inte försöker parkera alla MC på samma plats


string vehicleType;
string regNumber;
string typeDivider = "#";
string mcDivider = "|";
string[] parkingSpaces = new string[101];       // Skapar 101 element (0-100). P-plats 0 ska aldrig användas --> 100 p-platser
bool displayMenu = true;
//List<string> logs = new List<string>();     //skapar en lista för händelser

for (int i = 0; i < parkingSpaces.Length; i++)
{
    parkingSpaces[i] = "";
}

// Lägger in en bil på parkingSpaces[0] - endast testdata. Vi tar bort det sen
parkingSpaces[1] = "CAR#ABC123";
parkingSpaces[2] = "MC#BBB222";
parkingSpaces[3] = "MC#CCC333";
parkingSpaces[4] = "MC#DDD444|MCEEE555";

MatrixAllParkingSpaces();

Console.ReadKey();

void MergeMC()
{
    for (int i = 0; i < parkingSpaces.Length; i++)
    {
        // OM det står en mc på platsen, men ingen mcDivider
        // --> då står det 1 mc på platsen, inte 2
        if (parkingSpaces[i].Contains("MC") && !(parkingSpaces[i].Contains('|')))     
        {
            Console.WriteLine(parkingSpaces[i]); //Skriver ut alla mc som står själva
            // Här borde vara kod för att slå ihop dem. Ska fundera på det sen.
            continue;
        }
        else
        {
            //ANNARS: hoppa vidare och kolla nästa p-plats
            //(om det står 1 bil eller 2 mc på platsen kan vi strunta i p-platsen)
            continue;
        }

    }
}


void MatrixAllParkingSpaces()
{
    string[,] parkingMatrix = new string[10, 10];
    // Använder en räknare som börjar på 1 (för att p-plats 0 inte ska användas)
    int counter = 1; 

    // Lägger in strängarna från vektorn parkingSpaces i matrisen
    for (int i = 0; i < parkingMatrix.GetLength(0); i++)
    {
        for (int j = 0; j < parkingMatrix.GetLength(1); j++)
        {
            // Räknaren används som index på parkingSpaces - för att få strängarna på rätt plats [i, j]
            parkingMatrix[i, j] = parkingSpaces[counter];
            counter++;
        }
    }

    // Räknaren börjar på 1 (för att p-platserna börjar på 1)
    counter = 1;
    for (int i = 0; i < parkingMatrix.GetLength(0); i++)
    {
        for (int j = 0; j < parkingMatrix.GetLength(1); j++)
        {
            // OM det står en bil eller två mc på platsen --> upptagen
            if (parkingMatrix[i, j].Contains("CAR") || parkingMatrix[i, j].Contains('|'))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(counter.ToString().PadLeft(4));         // Co-pilot föreslog .PadLeft för snygg formatering
            }
            // ANNARS OM det står en mc på platsen --> halvt upptagen
            else if (parkingMatrix[i, j].Contains("MC") && !(parkingMatrix[i, j].Contains('|')))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(counter.ToString().PadLeft(4));
            }
            // ANNARS: ingen bil eller mc på platsen --> tom
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(counter.ToString().PadLeft(4));
            }
            counter++;
        }
        Console.WriteLine();
    }
    Console.ForegroundColor = ConsoleColor.Gray;
}