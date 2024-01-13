namespace MainLogic;

public class ShipSettings
{
    public int CountOfSize4Ships { get; }
    public int CountOfSize3Ships { get; }
    public int CountOfSize2Ships { get; }
    public int CountOfSize1Ships { get; }

    public ShipSettings(
        int countOfSize1Ships,
        int countOfSize2Ships,
        int countOfSize3Ships,
        int countOfSize4Ships)
    {
        CountOfSize1Ships = countOfSize1Ships;
        CountOfSize2Ships = countOfSize2Ships;
        CountOfSize3Ships = countOfSize3Ships;
        CountOfSize4Ships = countOfSize4Ships;
    }
}