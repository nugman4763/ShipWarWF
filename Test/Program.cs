// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Threading.Channels;
using MainLogic;

var settings = new ShipSettings(
    0,
    2,
    0,
    0
    );
    
var shipsValidator =
    new ShipsValidator(settings);
var shipsParser =
    new ShipsParser();
    
List<ShipDataJson> shipData = new List<ShipDataJson>();
// shipData.Add(new ShipDataJson
// {
//     X = "1",
//     Y = "4"
// });
shipData.Add(new ShipDataJson
{
    X = "3 4",
    Y = "5 5"
});
shipData.Add(new ShipDataJson
{
    X = "4 4",
    Y = "7 8"
});
// shipData.Add(new ShipDataJson
// {
//     X = "1 2 3 5",
//     Y = "4 4 4 4"
// });

List<Ship> ships = shipsParser.Execute(shipData);

shipsValidator.Execute(ships);

var shipDetectorForPlayer1 = new ShipDetector(ships);

Console.WriteLine(shipDetectorForPlayer1.IsDetected(3,5));

int[,] map = new int[3,3]
{
    {1,0,0},
    {1,0,0},
    {1,0,0}
};

var cellsToShipsConverter = 
    new CellsToShipsConverter();
    
List<ShipDataJson> shipDataJsons = cellsToShipsConverter.Execute(map);

foreach (var shipDataJson in shipDataJsons)
{
    Console.WriteLine(shipDataJson.X);
    Console.WriteLine(shipDataJson.Y);
}