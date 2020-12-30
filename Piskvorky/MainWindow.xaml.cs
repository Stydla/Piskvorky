using Piskvorky.FiveInARow;
using Piskvorky.User;
using Piskvorky.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

namespace Piskvorky
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      Data data = Data.Load();
      DataContext = data;      
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if(DataContext is Data data)
      {
        data.Save();
      }
    }


    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
      try
      {
        if (DataContext is Data data)
        {
          UserListDV ul = data.UsersPanelDV.UserListDV;
          if (ul.SelectedUser != null)
          {
            if (ul.SelectedUser.Games.Count > 0)
            {
              UserDV selectedUser = ul.SelectedUser;
              GameDV game = ul.SelectedUser.SelectedGame;

              JsonPlayRequest playRequest = new JsonPlayRequest();
              playRequest.gameToken = game.GameToken;
              playRequest.userToken = selectedUser.Token;
              playRequest.positionX = int.Parse(tbX.Text);
              playRequest.positionY = int.Parse(tbY.Text);

              JsonPlayResponse response = JsonCommunication.Play(playRequest);

              //MessageBox.Show($"{response.GetJsonString()}");

            }
          }
        }
      }
      catch(Exception ex)
      {

      }
    }

  }
}
