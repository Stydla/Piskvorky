using Piskvorky.AI;
using Piskvorky.FiveInARow;
using Piskvorky.User;
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

namespace Piskvorky.BulkSolve
{
  /// <summary>
  /// Interaction logic for BulkSolveUI.xaml
  /// </summary>
  public partial class BulkSolveUI : UserControl
  {
    public BulkSolveUI()
    {
      InitializeComponent();
    }

    private void CreateGames_Click(object sender, RoutedEventArgs e)
    {
      if (DataContext is Data data)
      {
        for(int i = 0; i < 24; i++)
        {
          BulkSolveDataItem bsdi = CreateGame();
          data.BulkSolveData.Data.Add(bsdi);
        }
      }
    }


    private BulkSolveDataItem CreateGame()
    {
      if (DataContext is Data data)
      {

        UserDV selectedUser = data.UsersPanelDV.UserListDV.SelectedUser;
        if (selectedUser == null) return null;

        JsonConnectRequest request = new JsonConnectRequest();
        request.userToken = selectedUser.Token;
        JsonConnectResponse response = JsonCommunication.Connect(request);

        GameDV game = new GameDV();
        game.GameId = response.gameId;
        game.GameToken = response.gameToken;
        if (!selectedUser.Games.Any(x => x.GameId == game.GameId))
        {
          selectedUser.Games.Add(game);
        }


        BulkSolveDataItem bsdi = new BulkSolveDataItem();
        bsdi.DeskData = new DeskData();
        bsdi.DeskData.SquareSize = 5;
        bsdi.Game = game;
        bsdi.User = selectedUser;

        return bsdi;
      }
      return null;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

      Timer timer = new Timer();
      timer.Elapsed += Timer_Elapsed;
      timer.Interval = 1000;
      timer.Start();
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      Dispatcher.Invoke(new Action(() => ShowSelectedGame()));
    }

    private void ShowSelectedGame()
    {
      try
      {
        if (DataContext is Data data)
        {

          foreach (var bsdi in data.BulkSolveData.Data)
          {

            Task.Run(() =>
            {
              UserDV user = bsdi.User;
              GameDV game = bsdi.Game;

              if (game == null) return;

              JsonCheckStatusRequest checkStatusRequest = new JsonCheckStatusRequest();
              checkStatusRequest.userToken = user.Token;
              checkStatusRequest.gameToken = game.GameToken;

              JsonCheckStatusResponse response = JsonCommunication.CheckStatus(checkStatusRequest);

              if (response.winnerId != null)
              {
                DeskData dd = bsdi.DeskData.Copy();
                if(response.winnerId == user.Id)
                {
                  dd.Winner = user.Nickname;
                } else
                {
                  dd.Winner = "Opponent";
                }
                bsdi.DeskData = dd;

              }
              else
              {
                DeskData dd = new DeskData();
                bool isMyTurn = user.Id == response.actualPlayerId;
                ESymbol mySymbol;
                if (isMyTurn)
                {
                  mySymbol = response.actualPlayerId == response.playerCircleId ? ESymbol.Circle : ESymbol.Cross;
                }
                else
                {
                  mySymbol = response.actualPlayerId == response.playerCircleId ? ESymbol.Cross : ESymbol.Circle;
                }

                dd.SquareSize = 5;
                dd.Fill(response, mySymbol, isMyTurn);
                bsdi.DeskData = dd;

                if (isMyTurn)
                {
                  AIEngine.StartAIEngine(game, user, dd);
                }
              }
            });
          }
        }
      }
      catch (Exception ex)
      {

      }
    }

    private bool stop = false;

    private void Run_Click(object sender, RoutedEventArgs e)
    {
      stop = false;
      Timer timer2 = new Timer();
      timer2.Elapsed += Timer2_Elapsed;
      timer2.Interval = 1000;
      timer2.Start();
    }

    private Dictionary<BulkSolveDataItem, int> RemoveCounter = new Dictionary<BulkSolveDataItem, int>();

    private void Play()
    {
      try
      {
        if (DataContext is Data data)
        {
          foreach (var d in data.BulkSolveData.Data)
          {
            if (d.DeskData.Winner != null)
            {
              if (RemoveCounter.ContainsKey(d))
              {
                RemoveCounter[d]++;
              }
              else
              {
                RemoveCounter.Add(d, 0);
              }

              if (RemoveCounter[d] >= 5)
              {
                data.BulkSolveData.Data.Remove(d);
                RemoveCounter.Remove(d);
              }
            }
          }

          if(!stop)
          {
            while (data.BulkSolveData.Data.Count < 24)
            {
              BulkSolveDataItem bsdi = CreateGame();
              data.BulkSolveData.Data.Add(bsdi);
            }
          }
          

          foreach (var item in data.BulkSolveData.Data)
          {

            Task.Run(() =>
            {
              try
              {
                UserDV user = item.User;
                GameDV game = item.Game;

                if (game == null) return;

                JsonCheckStatusRequest checkStatusRequest = new JsonCheckStatusRequest();
                checkStatusRequest.userToken = user.Token;
                checkStatusRequest.gameToken = game.GameToken;

                JsonCheckStatusResponse response = JsonCommunication.CheckStatus(checkStatusRequest);

                if (response.winnerId != null)
                {
                  DeskData dd = item.DeskData.Copy();
                  if (response.winnerId == user.Id)
                  {
                    dd.Winner = user.Nickname;
                  }
                  else
                  {
                    dd.Winner = "Opponent";
                  }
                  item.DeskData = dd;

                }
                else
                {
                  DeskData dd = new DeskData();
                  bool isMyTurn = user.Id == response.actualPlayerId;
                  ESymbol mySymbol;
                  if (isMyTurn)
                  {
                    mySymbol = response.actualPlayerId == response.playerCircleId ? ESymbol.Circle : ESymbol.Cross;
                  }
                  else
                  {
                    mySymbol = response.actualPlayerId == response.playerCircleId ? ESymbol.Cross : ESymbol.Circle;
                  }

                  dd.SquareSize = 5;
                  dd.Fill(response, mySymbol, isMyTurn);
                  item.DeskData = dd;

                  if (isMyTurn)
                  {
                    AIEngine.StartAIEngine(game, user, dd);
                  }
                }
              }
              catch (Exception ex)
              {

              }

            });
          }

        }
      }
      catch (Exception ex)
      {

      }
    }


    private void Timer2_Elapsed(object sender, ElapsedEventArgs e)
    {
      Dispatcher.Invoke(new Action(() => Play()));
    }

    private void Stop_Click(object sender, RoutedEventArgs e)
    {
      Dispatcher.Invoke(new Action(() => Stop()));
    }

    private void Stop()
    {
      stop = true;
    }
  }
}
