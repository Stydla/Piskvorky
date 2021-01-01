using Piskvorky.AI;
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

      ucDesk.EmptyPointClicked += UcDesk_PointClicked;

    }


    private void UcDesk_PointClicked(int x, int y)
    {
      if (DataContext is Data data)
      {
        try
        {
          if (data.AutomaticMode) return;


          UserDV user = data.UsersPanelDV.UserListDV.SelectedUser;
          GameDV game = user.SelectedGame;


          JsonPlayRequest request = new JsonPlayRequest();
          request.gameToken = game.GameToken;
          request.userToken = user.Token;
          request.positionX = x;
          request.positionY = y;

          JsonCommunication.Play(request);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString());
        }

      }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if(DataContext is Data data)
      {
        data.Save();
      }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      if (DataContext is Data data)
      {
        try
        {

          UserDV user = data.UsersPanelDV.UserListDV.SelectedUser;
          GameDV game = user.SelectedGame;

          AIEngine.StartAIEngine(game, user, data.DeskData);

        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString());
        }

      }
    }


  }
}
