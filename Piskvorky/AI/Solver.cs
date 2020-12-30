using Piskvorky.FiveInARow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piskvorky.AI
{
  public class Solver
  {
    DeskData DeskData;
    List<List<SolverField>> Array = new List<List<SolverField>>();

    public PointData GetNextMove(DeskData deskData)
    {
      DeskData = deskData;
      Initialize();


      SolverField sf = GetRandomField();
      PointData pd = ConvertSolverField(sf);
      return pd;
    }

    private SolverField GetRandomField()
    {
      Random rnd = new Random((int)DateTime.Now.Ticks);
      int y = rnd.Next(0, Array.Count);
      var row = Array[y];
      int x = rnd.Next(0, row.Count);
      return row[x];
    }

    private PointData ConvertSolverField(SolverField field)
    {
      int x = field.X + DeskData.StartIndexX;
      int y = field.Y + DeskData.StartIndexY;
      return new PointData() { X = x, Y = y };
    }

    private void Initialize()
    {
      int indexI = 0;
      for (int i = DeskData.StartIndexY; i <= DeskData.EndIndexY; i++, indexI++)
      {
        
        Array.Add(new List<SolverField>());
        int indexJ = 0;
        for (int j = DeskData.StartIndexX; j <= DeskData.EndIndexX; j++, indexJ++)
        {
          Array[indexI].Add(new SolverField(indexJ, indexI, EType.Empty));
        }
      }

      foreach (var circle in DeskData.CirclePoints)
      {
        int x = circle.X - DeskData.StartIndexX;
        int y = circle.Y - DeskData.StartIndexY;
        if (DeskData.MySymbol == ESymbol.Circle)
        {
          Array[y][x].Type = EType.Circle;
        }
        else 
        {
          Array[y][x].Type = EType.Cross;
        }
      }

      foreach (var circle in DeskData.CrossPoints)
      {
        int x = circle.X - DeskData.StartIndexX;
        int y = circle.Y - DeskData.StartIndexY;
        if (DeskData.MySymbol == ESymbol.Cross)
        {
          Array[y][x].Type = EType.Circle;
        }
        else
        {
          Array[y][x].Type = EType.Cross;
        }
      }
    }
  }
}
