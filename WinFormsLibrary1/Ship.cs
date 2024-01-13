namespace MainLogic;

public class Ship
{
    public List<ShipCell> Cells { get; }
    
    public Ship(List<ShipCell> cells)
    {
        Cells = cells;
    }
}

public class ShipCell
{
    public int X { get; }
    public int Y { get; }
    public ShipCell(int x, int y)
    {
        X = x;
        Y = y;
    }
}