namespace MainLogic;

public class CellsToShipsConverter
{
    public List<ShipDataJson> Execute(int[,] map)
    {
        var ship = new List<ShipDataJson>();

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 1)
                {
                    var shipDataJson = new ShipDataJson();

                    shipDataJson.X = j.ToString();
                    shipDataJson.Y = i.ToString();

                    if ((i + 1) < map.GetLength(0) && map[i + 1, j] == 1)
                    {
                        shipDataJson.X += " " + j.ToString();
                        shipDataJson.Y += " " + (i + 1).ToString();


                        for (int k = 2; (i + k) < map.GetLength(0) && map[i + k, j] == 1; k++)
                        {
                            shipDataJson.X += " " + j.ToString();
                            shipDataJson.Y += " " + (i + k).ToString();

                            map[i + k, j] = 0;
                        }

                        map[i + 1, j] = 0;
                    }
                    else if ((j + 1) < map.GetLength(1) && map[i, j + 1] == 1)
                    {
                        shipDataJson.X += " " + (j + 1).ToString();
                        shipDataJson.Y += " " + i.ToString();

                        for (int k = 2; (j + k) < map.GetLength(1) && map[i, j + k] == 1; k++)
                        {
                            shipDataJson.X += " " + (j + k).ToString();
                            shipDataJson.Y += " " + i.ToString();

                            map[i, j + k] = 0;
                        }

                        map[i, j + 1] = 0;
                    }
                    map[i, j] = 0;
                    ship.Add(shipDataJson);

                }
            }
        }

        return ship;
    }


}