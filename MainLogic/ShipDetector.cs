using System.Diagnostics;

namespace MainLogic;

public class ShipDetector
{
    private readonly List<Ship> _ships;
    private List<ShipCell> _chekedCells;
    private int _countWinCells;
    private int _winCellsHitted;

    public ShipDetector(List<Ship> ships, ShipSettings settings)
    {
        _ships = ships;
        _chekedCells = new List<ShipCell>();
        _countWinCells = settings.CountOfSize4Ships*4 +
                settings.CountOfSize3Ships*3 +
                settings.CountOfSize2Ships*2 +
                settings.CountOfSize1Ships;
        _winCellsHitted = 0;

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
                    _winCellsHitted++;
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

    public bool CheckWin()
    {
        return _winCellsHitted == _countWinCells;
    }
}