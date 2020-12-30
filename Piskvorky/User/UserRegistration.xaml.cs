using Piskvorky.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Piskvorky.User
{
  /// <summary>
  /// Interaction logic for UserRegistration.xaml
  /// </summary>
  public partial class UserRegistration : UserControl
  {
    public UserRegistration()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

      if(string.IsNullOrWhiteSpace(tbNickname.Text))
      {
        MessageBox.Show("Vypln nickname!");
      }
      if(string.IsNullOrWhiteSpace(tbEmail.Text))
      {
        MessageBox.Show("Vypln email!");
      }

      bool isEmail = Regex.IsMatch(tbEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
      if(!isEmail)
      {
        MessageBox.Show("Email neni spravne!");
      }

      JsonNewUserRequest user = new JsonNewUserRequest();
      user.email = tbEmail.Text;
      user.nickname = tbNickname.Text;
      

      var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://piskvorky.jobs.cz/api/v1/user");
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Method = "POST";

      using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
      {
        string json = user.GetJsonString();

        streamWriter.Write(json);
      }

      string resultJson;
      try
      {
        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
          resultJson = streamReader.ReadToEnd();

          JsonNewUserResponse responseObj = JsonNewUserResponse.Parse(resultJson);
          Save(responseObj, user.nickname, user.email);

        }
      } catch(WebException ex)
      {
        var httpResponse = ex.Response;
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
          resultJson = streamReader.ReadToEnd();
        }
        
        MessageBox.Show(resultJson);
      }
      
    }

    private void Save(JsonNewUserResponse jsonUser, string nickname, string email)
    {
      if(DataContext is Data data)
      {
        UserDV user = new UserDV();
        user.Id = jsonUser.userId;
        user.Token = jsonUser.userToken;
        user.Email = email;
        user.Nickname = nickname;
        data.UsersPanelDV.UserListDV.Users.Add(user);
      } else
      {
        MessageBox.Show("Invalid Data Context - user registration");
      }
    }

  }
}
