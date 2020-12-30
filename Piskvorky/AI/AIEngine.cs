using Piskvorky.FiveInARow;
using Piskvorky.User;
using Piskvorky.WebApi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piskvorky.AI
{
  class AIEngine
  {

    private static ConcurrentDictionary<GameDV, int> InProgress = new ConcurrentDictionary<GameDV,int>();

    public static bool StartAIEngine(GameDV game, UserDV user, DeskData data)
    {
      
      if(InProgress.Keys.Any(x=>x.GameId == game.GameId)) 
      {
        return false;
      }
      InProgress.TryAdd(game, 0);

      Task.Run(new Action(() =>
      {
        try
        {
          Solver solver = new Solver();
          PointData pd = solver.GetNextMove(data);

          JsonPlayRequest request = new JsonPlayRequest();
          request.gameToken = game.GameToken;
          request.userToken = user.Token;
          request.positionX = pd.X;
          request.positionY = pd.Y;
          JsonCommunication.Play(request); // ignore response
          int outval;
          InProgress.TryRemove(game, out outval);
        }
        catch (Exception ex)
        {
          int outval;
          InProgress.TryRemove(game, out outval);
        }

      }));
      return true;
    }

  }
}
