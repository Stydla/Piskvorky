using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Piskvorky.WebApi
{
  public class JsonCommunication
  {

    private static string SendRequest(string url, string contentJson)
    {
      var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Method = "POST";

      using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
      {
        streamWriter.Write(contentJson);
      }

      string resultJson;
      try
      {
        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
          resultJson = streamReader.ReadToEnd();
        }
      }
      catch (WebException ex)
      {
        var httpResponse = ex.Response;
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
          resultJson = streamReader.ReadToEnd();
        }

        MessageBox.Show(resultJson);
        throw new Exception(resultJson);
      }
      return resultJson;
    }

    public static JsonNewUserResponse CreateUser(JsonNewUserRequest user)
    {
      string url = "https://piskvorky.jobs.cz/api/v1/user";
      string response = SendRequest(url, user.GetJsonString());
      return JsonNewUserResponse.Parse(response);
    }

    public static JsonConnectResponse Connect(JsonConnectRequest request)
    {
      string url = "https://piskvorky.jobs.cz/api/v1/connect";
      string response = SendRequest(url, request.GetJsonString());
      return JsonConnectResponse.Parse(response);
    }

    public static JsonPlayResponse Play(JsonPlayRequest request)
    {
      string url = "https://piskvorky.jobs.cz/api/v1/play";
      string response = SendRequest(url, request.GetJsonString());
      return JsonPlayResponse.Parse(response);
    }

    internal static JsonCheckStatusResponse CheckStatus(JsonCheckStatusRequest request)
    {
      string url = "https://piskvorky.jobs.cz/api/v1/checkStatus";
      string response = SendRequest(url, request.GetJsonString());
      return JsonCheckStatusResponse.Parse(response);
    }
  }
}
