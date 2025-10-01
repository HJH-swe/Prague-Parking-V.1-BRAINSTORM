// Förslag på metod: När ett nytt fodon registrerats, ha en metod som kollar om det är en MC.
// Gå igenom parkerade fordon och kolla om det finns en plats med 1 MC. '
// Säg till på en gång att parkera den nya MC på samma plats.
// Eventuella svårigheter: Se till att det bara blir 2 MC på samma plats,
// alltså att programmet inte försöker parkera alla MC på samma plats

void MergeMC()
{
    for (int i = 0; i < vehicles.Length; i++)
    {
        if (vehicles[i].type != null)     // Här kanske man får ändra sen om type = MC
        {
            if (vehicles[i].type == "MC") // Får nog ska en ny egenskap, tex. bool .parkedtogether
            {
                if (vehicles[i].parkedtogher = false)
                {
                    // Här kan vi kanske använda metoderna Claes tipsade om i inlämningsuppgiften: 
                    // String.Join String.Split
                }
            }
            continue;
        }
        else
        {
            VehicleRegistration(vehicles, i);   // Kanske borde returnera ett värde istället? 
            break;
        }

    }
}