using Piskvorky.WebApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piskvorky.FiveInARow
{
  public class DeskData
  {

    public int StartIndexX { get; set; }
    public int EndIndexX { get; set; }
    public int StartIndexY { get; set; }
    public int EndIndexY {get; set;}

    public bool IsMyTurn { get; set; }
    public ESymbol MySymbol { get; set; }

    public List<PointData> CirclePoints { get; set; } = new List<PointData>();
    public List<PointData> CrossPoints { get; set; } = new List<PointData>();

    public double SquareSize { get; set; } = 15;

    public string Winner { get;  set; }

    public ESymbol CurrentPlayerSymbol
    {
      get
      {
        if(IsMyTurn)
        {
          if(MySymbol == ESymbol.Circle)
          {
            return ESymbol.Circle;
          } else
          {
            return ESymbol.Cross;
          }
        } else
        {
          if (MySymbol == ESymbol.Circle)
          {
            return ESymbol.Cross;
          }
          else
          {
            return ESymbol.Circle;
          }
        }
      }
    }

    public DeskData Copy()
    {
      DeskData copy = (DeskData)this.MemberwiseClone();
      copy.CirclePoints = new List<PointData>(this.CirclePoints);
      copy.CrossPoints = new List<PointData>(this.CrossPoints);
      return copy;
    }
    

    public DeskData() : this(-28, 28, -20, 20)
    {
    }

    public DeskData(int fromX, int toX, int fromY, int toY)
    {
      StartIndexX = fromX;
      EndIndexX = toX;
      StartIndexY = fromY;
      EndIndexY = toY;
    }

    public void Fill(JsonCheckStatusResponse data, ESymbol mySymbol, bool isMyTurn)
    {
      foreach(var p in data.coordinates)
      {
        PointData pd  = new PointData() { X = p.x, Y = p.y };
        if (p.playerId == data.playerCircleId)
        {
          CirclePoints.Add(pd);
        }
        if(p.playerId == data.playerCrossId)
        {
          CrossPoints.Add(pd);
        }
      }

      MySymbol = mySymbol;
      IsMyTurn = isMyTurn;
    }

    internal void Clear()
    {
      CirclePoints.Clear();
      CrossPoints.Clear();
    }
  }

  public enum ESymbol
  {
    Cross,
    Circle
  }

}
