using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Piskvorky.WebApi
{
  public class JsonNewUserRequest
  {
    /*
{
  "nickname": "string",
  "email": "string"
}
    */
    public string nickname { get; set; }
    public string email { get; set; }

    public string GetJsonString()
    {
      string jsonString = JsonSerializer.Serialize(this);
      return jsonString;
    }
  }

  public class JsonNewUserResponse
  {
    /*
{
  "statusCode": 0,
  "gameToken": "string",
  "gameId": "string"
}*/

    public int statusCode { get; set; }
    public string userId { get; set; }
    public string userToken { get; set; }

    public static JsonNewUserResponse Parse(string input)
    {
      return (JsonNewUserResponse)JsonSerializer.Deserialize(input, typeof(JsonNewUserResponse));
    }

  }
}
