namespace MainLogic;

public class ShipsValidator
{
    private readonly ShipSettings _settings;
    
    private int _currentCountOfSize1Ships;
    private int _currentCountOfSize2Ships;
    private int _currentCountOfSize3Ships;
    private int _currentCountOfSize4Ships;
    
    public ShipsValidator(ShipSettings settings)
    {
        _settings = settings;
        _currentCountOfSize1Ships = 0;
        _currentCountOfSize2Ships = 0;
        _currentCountOfSize3Ships = 0;
        _currentCountOfSize4Ships = 0;
    }
    
    public void Execute(List<Ship> ships)
    {
        ValidateCount(ships);
        ValidateIntersection(ships);
        //Граница поля - контр читы
        //Раздробленный кораблик - контр читы
    }

    private void ValidateCount(List<Ship> ships)
    {
        foreach (var ship in ships)
        {
            switch (ship.Cells.Count)
            {
                case 1:
                    _currentCountOfSize1Ships++;
                    break;
                case 2:
                    _currentCountOfSize2Ships++;
                    break;
                case 3:
                    _currentCountOfSize3Ships++;
                    break;
                case 4:
                    _currentCountOfSize4Ships++;
                    break;
                default:
                    throw new Exception("Недопустимый размер корабля");
            }
        }
        if (_currentCountOfSize1Ships != _settings.CountOfSize1Ships)
            throw new Exception("Недопустимый число единичных короблей");
        if (_currentCountOfSize2Ships != _settings.CountOfSize2Ships)
            throw new Exception("Недопустимый число двойных короблей");
        if (_currentCountOfSize3Ships != _settings.CountOfSize3Ships)
            throw new Exception("Недопустимый число тройных короблей");
        if (_currentCountOfSize4Ships != _settings.CountOfSize4Ships)
            throw new Exception("Недопустимый число четверных короблей");
    }

    private void ValidateIntersection(List<Ship> ships)
    {
        for (int i = 0; i < ships.Count; i++)
        {
            int minX = int.MaxValue;
            int maxX = 0;
            int minY = int.MaxValue;
            int maxY = 0;

            foreach (var cell in ships[i].Cells)
            {
                minX = Math.Min(minX, cell.X);
                maxX = Math.Max(maxX, cell.X);
                minY = Math.Min(minY, cell.Y);
                maxY = Math.Max(maxY, cell.Y);
            }

            for (int j = 0; j < ships.Count; j++)
            {
                if (i != j)
                {
                    foreach (var cell in ships[j].Cells)
                    {
                        if (cell.X >= minX - 1 && cell.X <= maxX + 1 &&
                            cell.Y >= minY - 1 && cell.Y <= maxY + 1)
                        {
                            throw new Exception($"Корабли {i} и {j} пересекаются");
                        }
                    }
                }
            }
        }
    }
}