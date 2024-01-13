namespace MainLogic;

public class ShipsParser
{
    public List<Ship> Execute(List<ShipDataJson> shipsDataJson)
    {
        List<Ship> ships = new List<Ship>();

        foreach (var shipDataJson in shipsDataJson)
        {
            List<int> xCoordinates = ParseCoordinates(shipDataJson.X);
            List<int> yCoordinates = ParseCoordinates(shipDataJson.Y);

            var cells = new List<ShipCell>();
            for (int i = 0; i < xCoordinates.Count; i++)
            {
                ShipCell cell = new ShipCell(
                    xCoordinates[i],
                    yCoordinates[i]);
                cells.Add(cell);
            }
            ships.Add(new Ship(cells));
        }

        return ships;
    }
    
    private List<int> ParseCoordinates(string coordinates)
    {
        string[] coordinatesArray = coordinates.Split(' ');
        var intCoordinates = new List<int>();
        foreach (string s in coordinatesArray)
        {
            intCoordinates.Add(Int32.Parse(s));
        }

        return intCoordinates;
    }
}