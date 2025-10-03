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
List<string> logs = new List<string>();     //skapar en lista för händelser

for (int i = 0; i < parkingSpaces.Length; i++)
{
    parkingSpaces[i] = "x";
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
        if (parkingSpaces[i].Contains("MC") && !(parkingSpaces[i].Contains("|")))     
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

//Kan komma på ett bättre namn sen
void VisualRepresentationAllParkingSpaces()
{
    //Ska klura lite på en matris som visar hela parkeringen

    // en 10 x 10 matris
    int[,] parkingMatrix = new int[10, 10]
    {
        {1, 2, 3, 4, 5, 6, 7 ,8, 9, 10 },
        {11, 12, 13, 14, 15, 16, 17, 18, 19, 20 },
        {21, 22, 23, 24, 25, 26, 27, 28, 29 , 30},
        {31, 32, 33, 34, 35, 36, 37, 38, 39, 40 },
        {41, 42, 43, 44, 45, 46, 47, 48, 49, 50 },
        {51, 52, 53, 54, 55, 56, 57, 58, 59, 60 },
        {61, 62, 63, 64, 65, 66, 67, 68, 69, 70 },
        {71, 72, 73, 74, 75, 76, 77, 78, 79, 80 },
        {81, 82, 83, 84, 85, 86, 87, 88, 89, 90 },
        {91, 92, 93, 94, 95, 96, 97, 98, 99, 100 }
    };

    // Läste om nästlade for-loopar i matriser här: https://www.bytehide.com/blog/2d-arrays-csharp
    // och lånade den mallen
    for (int i = 0; i < parkingMatrix.GetLength(0); i++)
    {
        for (int j = 0; j < parkingMatrix.GetLength(1); j++)
        {
            // access matrix[i, j] here
            Console.Write(parkingMatrix[i, j].ToString().PadLeft(4));
        }
        // Fick hjälp av co-pilot att placera CW rätt och PadLeft
        Console.WriteLine();
    }
}

void MatrixAllParkingSpaces()
{
    string[,] parkingMatrix = new string[10, 10];
    int counter = 1; 

    // Ska försöka lägga in strängarna från parkingSpaces i
    // ska ju börja på 1 egentligen
    for (int i = 0; i < parkingMatrix.GetLength(0); i++)
    {
        for (int j = 0; j < parkingMatrix.GetLength(1); j++)
        {
            // access matrix[i, j] here
            parkingMatrix[i, j] = parkingSpaces[counter];
            counter++;
        }
    }

    counter = 0;
    for (int i = 0; i < parkingMatrix.GetLength(0); i++)
    {
        for (int j = 0; j < parkingMatrix.GetLength(1); j++)
        {
            counter++;
            // access matrix[i, j] here
            if (parkingMatrix[i, j].Contains("CAR") || parkingMatrix[i, j].Contains("|"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(counter.ToString().PadLeft(4));
            }
            else if (parkingMatrix[i, j].Contains("MC") && !(parkingMatrix[i, j].Contains("|")))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(counter.ToString().PadLeft(4));
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(counter.ToString().PadLeft(4));
            }
        }
        // Fick hjälp av co-pilot att placera CW .PadLeft
        Console.WriteLine();
    }
    Console.ForegroundColor = ConsoleColor.Gray;
}