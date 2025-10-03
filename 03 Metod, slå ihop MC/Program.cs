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

// Lägger in en bil på parkingSpaces[0] - endast testdata. Vi tar bort det sen
parkingSpaces[0] = "CAR#ABC123";
parkingSpaces[1] = "MC#BBB222";
parkingSpaces[2] = "MC#CCC333";
parkingSpaces[3] = "MC#DDD444|MCEEE555";

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
