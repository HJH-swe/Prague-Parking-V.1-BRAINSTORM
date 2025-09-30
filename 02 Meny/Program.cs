// Börjar spåna på en meny och metod vi kan lägga in den i -
// men vi kan skrota den om nån annan redan har börjat eller kommer på en bättre design!

MainMenu();


Console.ReadKey();

static void MainMenu()
{
    Console.WriteLine("\t\t-- MENY --");
    Console.WriteLine("\tVälj alterativ med siffertangenterna");  // Siffror eller  bokstäver? Är "siffertangenter" rätt/tydligt?
    Console.WriteLine("\t[1]  Parkera nytt fordon");
    Console.WriteLine("\t[2]  Flytta ett fordon");
    Console.WriteLine("\t[3]  Söka efter ett fordon");
    Console.WriteLine("\t[4]  Hämta ut fordon");       // Fordonet ska tas bort från systemet
    Console.WriteLine("\t[5]  Avsluta programmet");
    // Kan fylla på med fler val

    // Testar try-catch för att säkra upp koden lite
    try
    {
        int menuSelect = int.Parse(Console.ReadLine());

        switch (menuSelect)
        {
            case 1:
                {
                    // Fyll på med kod/metod - parkera fordon
                    Console.WriteLine("Test. meny 1");
                    break;
                }
            case 2:
                {
                    // Fyll på med kod/metod - flytta ett fordon.
                    // t.ex. flytta ihop MC
                    Console.WriteLine("Test. meny 2");
                    break;
                }
            case 3:
                {
                    // Fyll på med kod/metod - söka efter fordon.
                    // t.ex efter regnummer
                    Console.WriteLine("Test. meny 3");
                    break;
                }
            case 4:
                {
                    // Fyll på med kod/metod - hämta ut fordon.
                    // Fordonet ska även tas bort från vektorn
                    Console.WriteLine("Test. meny 4");
                    break;
                }
            case 5:
                {
                    // Fyll på med kod/metod - avsluta programmet
                    Console.WriteLine("Test. meny 5");
                    break;
                }

            default:
                {
                    Console.WriteLine("Ogiltigt menyval. Välj genom att trycka 1-5\n\n"); //TODO: ändra 1-X beroende på antal menyval
                    MainMenu();
                    break;
                }
        }
    }
    catch
    {

        Console.WriteLine("Ogiltigt menyval. Vänligen välj i menyn genom att trycka på siffertangenterna.\n\n");
        MainMenu();
    }
}