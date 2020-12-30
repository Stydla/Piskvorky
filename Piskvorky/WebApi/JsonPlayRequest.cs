using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Piskvorky.WebApi
{
  public class JsonPlayRequest
  {
    /*
{
  "userToken": "59f1e6b1-4241-465a-9ff3-985c9c1365d6",
  "gameToken": "507ff295-5007-4967-9d42-9d1c9c1137f0",
  "positionX": 28,
  "positionY": 0
}
     */

    public string userToken { get; set; }
    public string gameToken { get; set; }
    public int positionX { get; set; }
    public int positionY { get; set; }

    public string GetJsonString()
    {
      string jsonString = JsonSerializer.Serialize(this);
      return jsonString;
    }
  }

  public class JsonPlayResponse
  {
    /*
{
  "statusCode": 0,
  "playerCrossId": "string",
  "playerCircleId": "string",
  "actualPlayerId": "string",
  "winnerId": "string",
  "coordinates": [
    [
      {
        "playerId": "string",
        "x": 0,
        "y": 0
      }
    ]
  ]
} 
     */
    public int statusCode { get; set; }
    public string playerCrossId { get; set; }
    public string playerCircleId { get; set; }
    public string actualPlayerId { get; set; }
    public string winnerId { get; set; }
    public List<JsonCoordinates> coordinates { get; set;}

    public static JsonPlayResponse Parse(string input)
    {
      return (JsonPlayResponse)JsonSerializer.Deserialize(input, typeof(JsonPlayResponse));
    }

    public string GetJsonString()
    {
      string jsonString = JsonSerializer.Serialize(this);
      return jsonString;
    }

  }


}
