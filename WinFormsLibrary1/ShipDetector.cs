namespace MainLogic;

public class ShipDetector
{
    private readonly List<Ship> _ships;
    private List<ShipCell> _chekedCells;

    public ShipDetector(List<Ship> ships)
    {
        _ships = ships;
        _chekedCells = new List<ShipCell>();
    }

    public bool IsDetected(int x, int y)
    {
        if (IsAlreadyChecked(x, y))
        {
            throw new Exception("Клетка уже была проверена");
        }
        _chekedCells.Add(new ShipCell(x, y));
        foreach (var ship in _ships)
        {
            foreach (var cell in ship.Cells)
            {
                if (cell.X == x && cell.Y == y)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsAlreadyChecked(int x, int y)
    {
        foreach (var cell in _chekedCells)
        {
            if (cell.X == x && cell.Y == y)
            {
                return true;
            }
        }

        return false;
    }
}