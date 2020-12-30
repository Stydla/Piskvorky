using Piskvorky.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
  /// Interaction logic for UserList.xaml
  /// </summary>
  public partial class UserList : UserControl
  {
    public UserList()
    {
      InitializeComponent();


    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      if(DataContext is Data data)
      {
        UserDV selectedUser = data.UsersPanelDV.UserListDV.SelectedUser;
        if (selectedUser != null)
        {
          JsonConnectRequest request = new JsonConnectRequest();
          request.userToken = selectedUser.Token;
          JsonConnectResponse response = JsonCommunication.Connect(request);

          GameDV game = new GameDV();
          game.GameId = response.gameId;
          game.GameToken = response.gameToken;
          if(!selectedUser.Games.Any(x=>x.GameId == game.GameId))
          {
            selectedUser.Games.Add(game);
          }
        }
      }
    }
  }
}
