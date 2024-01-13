using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace ClientWF
{
    public class BytesConverter
    {
        public byte[] ConvertShipToBytes(List<ShipDataJson> shipDataJson)
        {
            string jsonString = JsonConvert.SerializeObject(shipDataJson);

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
            

            return byteArray;
        }

        public List<ShipDataJson> ConvertBytesToShip(byte[] bytes)
        {
            string jsonString = Encoding.UTF8.GetString(bytes);

            List<ShipDataJson> shipDataJson = JsonConvert.DeserializeObject<List<ShipDataJson>>(jsonString);

            return shipDataJson;
        }

        public byte[] ConvertCellToBytes(CellInfoJson cell)
        {
            string jsonString = JsonConvert.SerializeObject(cell);

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);

            return byteArray;
        }

        public CellInfoJson ConvertBytesToCell(byte[] bytes)
        {
            string jsonString = Encoding.UTF8.GetString(bytes);

            CellInfoJson cell = JsonConvert.DeserializeObject<CellInfoJson>(jsonString);

            return cell;
        }
    }
}
