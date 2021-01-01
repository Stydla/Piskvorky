using Piskvorky.AI;
using Piskvorky.FiveInARow;
using Piskvorky.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
  /// Interaction logic for Games.xaml
  /// </summary>
  public partial class GamesList : UserControl
  {

    

    public GamesList()
    {
      InitializeComponent();

      Timer timer = new Timer();
      timer.Elapsed += Timer_Elapsed;
      timer.Interval = 500;
      timer.Start();

    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      Dispatcher.Invoke(new Action(()=> ShowSelectedGame()));
      
    }


    private void ShowSelectedGame()
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

              if (game == null) return;

              JsonCheckStatusRequest checkStatusRequest = new JsonCheckStatusRequest();
              checkStatusRequest.userToken = selectedUser.Token;
              checkStatusRequest.gameToken = game.GameToken;

              JsonCheckStatusResponse response = JsonCommunication.CheckStatus(checkStatusRequest);

              if(response.winnerId != null)
              {
                ul.SelectedUser.SelectedGame = null;
                ul.SelectedUser.Games.Remove(game);
              } else
              {
                DeskData dd = new DeskData();
                bool isMyTurn = selectedUser.Id == response.actualPlayerId;
                ESymbol mySymbol;
                if (isMyTurn)
                {
                  mySymbol = response.actualPlayerId == response.playerCircleId ? ESymbol.Circle : ESymbol.Cross;
                } else
                {
                  mySymbol = response.actualPlayerId == response.playerCircleId ? ESymbol.Cross : ESymbol.Circle;
                }
                
                dd.Fill(response, mySymbol, isMyTurn);
                data.DeskData = dd;

                if(isMyTurn && data.AutomaticMode)
                {
                  AIEngine.StartAIEngine(game, selectedUser, dd);
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {

      }
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ShowSelectedGame();
    }
  }
}
