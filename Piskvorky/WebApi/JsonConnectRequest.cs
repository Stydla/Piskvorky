using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Piskvorky.WebApi
{
  public class JsonConnectRequest
  {
    /*
{
  "userToken": "59f1e6b1-4241-465a-9ff3-985c9c1365d6"
}
    */

    public string userToken { get; set; }

    public string GetJsonString()
    {
      string jsonString = JsonSerializer.Serialize(this);
      return jsonString;
    }

  }

  public class JsonConnectResponse
  {
    /*
{
  "statusCode": 201,
  "gameToken": "507ff295-5007-4967-9d42-9d1c9c1137f0",
  "gameId": "e64cca46-46bc-4d80-b25f-3021deb42eb9",
  "headers": {}
}
     */

    public int statusCode { get; set; }
    public string gameToken { get; set; }
    public string gameId { get; set; }


    public static JsonConnectResponse Parse(string input)
    {
      return (JsonConnectResponse)JsonSerializer.Deserialize(input, typeof(JsonConnectResponse));
    }
  }
}
