using Piskvorky.FiveInARow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piskvorky.AI
{
  // My symbol is always circle!
  public class Solver
  {
    DeskData DeskData;
    List<List<SolverField>> Array = new List<List<SolverField>>();

    public PointData GetNextMove(DeskData deskData)
    {
      DeskData = deskData;
      Initialize();

      EvaluateFields(EType.Circle);
      EvaluateFields(EType.Cross);
      EvaluateFieldsValues(EType.Circle);

      //PrintValues();

      SolverField sfBest = GetBestField(EType.Circle);
      return ConvertSolverField(sfBest);

      //SolverField sf = GetRandomField();
      //PointData pd = ConvertSolverField(sf);
      //return pd;
    }

    private void PrintValues()
    {
      for(int i = 0; i < Array.Count; i++)
      {
        for(int j = 0; j < Array[i].Count; j++)
        {
          var field = Array[i][j];

          System.Diagnostics.Debug.Write($"{field.ValueBy[EType.Circle],5}");
        }
        System.Diagnostics.Debug.WriteLine("");
      }
    }

    private void EvaluateFieldsValues(EType player)
    {
      int iMax = Array.Count;
      for (int i = 0; i < iMax; i++)
      {
        int jMax = Array[i].Count;
        for (int j = 0; j < jMax; j++)
        {
          int totalValue = 0;

          int positionValueBonusY = Math.Min(i, iMax - i);
          int positionValueBonusX = Math.Min(j, jMax - j);
          int positionBonus = Math.Min(positionValueBonusX, positionValueBonusY);

          totalValue += positionBonus;


          totalValue += Array[i][j].GetThreatValue();


          totalValue += GetNeighboursValue(Array[i][j]);


          if (Array[i][j].Type == EType.Empty)
          {
            Array[i][j].ValueBy[player] = totalValue;
          } else
          {
            Array[i][j].ValueBy[player] = -1;
          }
          
        }
      }
    }

    private int GetNeighboursValue(SolverField sf)
    {
      int distance = 3;
      int totalValue = 0;

      int iMax = Array.Count;
      int jMax = Array[0].Count;


      for(int i = -distance; i <= distance; i++)
      {
        for(int j = -distance; j <= distance; j++)
        {
          int iIndex = sf.Y  + i;
          int jIndex = sf.X  + j;

          if (iIndex < 0 || jIndex < 0 || iIndex >= iMax || jIndex >= jMax
           || (iIndex == sf.Y && jIndex == sf.X))
          {
            continue;
          }
          else
          {
            SolverField sfCheck = Array[iIndex][jIndex];
            if (sfCheck.Type != EType.Empty)
            {
              if (sfCheck.Type == sf.Type)
              {
                totalValue += (5 * ((distance - Math.Abs(i) + (distance - Math.Abs(j)))));
              }
              else
              {
                totalValue += (((distance - Math.Abs(i)) + (distance - Math.Abs(j))));
              }
            }
          }
        }
      }

      //for (int i = sf.Y - distance; i <= sf.Y + distance; i++)
      //{
      //  for(int j = sf.X - distance; j <= sf.X + distance; j++)
      //  {
         

      //  }
      //}

      return totalValue;
    }

    private SolverField GetBestField(EType player)
    {
      var fields = Array.SelectMany(x => x);

      var winFieldImmediate = fields.Where(x => x.IsWinningMoveImmediate(player)).FirstOrDefault();
      if (winFieldImmediate != null)
      {
        return winFieldImmediate;
      }

      var isForcedImmediate = fields.Where(x => x.IsForcedThreatImmediate(player));
      if (isForcedImmediate.Count() > 0)
      {
        return isForcedImmediate.Aggregate((i1, i2) => i1.ValueBy[player] > i2.ValueBy[player] ? i1 : i2);
      }

      var winField = fields.Where(x => x.IsWinningMoveCurrent(player));
      if(winField.Count() > 0)
      {
        return winField.Aggregate((i1, i2) => i1.ValueBy[player] > i2.ValueBy[player] ? i1 : i2);
      }

      var isForced = fields.Where(x => x.IsForcedMoveCurrent(player));
      if (isForced.Count() > 0)
      {
        return isForced.Aggregate((i1, i2) => i1.ValueBy[player] > i2.ValueBy[player] ? i1 : i2);
      }

      var winFieldNext = fields.Where(x => x.IsWinningMoveNext(player));
      if (winField.Count() > 0)
      {
        return winField.Aggregate((i1, i2) => i1.ValueBy[player] > i2.ValueBy[player] ? i1 : i2);
      }

      var isForcedNext = fields.Where(x => x.IsForcedMoveNext(player));
      if (isForcedNext.Count() > 0)
      {
        return isForcedNext.Aggregate((i1, i2) => i1.ValueBy[player] > i2.ValueBy[player] ? i1 : i2);
      }

      var maxValueField = fields.Aggregate((i1, i2) => i1.ValueBy[player] > i2.ValueBy[player] ? i1 : i2);
      return maxValueField;
    }

    private void EvaluateFields(EType player)
    {
      EType oponent = player == EType.Circle ? EType.Cross : EType.Circle;

      int iMax = Array.Count;
      for(int i = 0; i < iMax; i++)
      {
        int jMax = Array[i].Count;
        for(int j = 0; j < jMax; j++)
        {

          // Top ->  Down
          List<SolverField> topDownList = new List<SolverField>();
          for (int i_2 = i - 5; i_2 <= i + 5; i_2++)
          {
            
            if (i_2 < 0 || i_2 >= iMax)
            {
              topDownList.Add(new SolverField(j, i_2, oponent));
            } else
            {
              topDownList.Add(Array[i_2][j]);
            }
          }
          Array[i][j].ScantThreatsFor(player, topDownList);



          // Left -> Right
          List<SolverField> leftRightList = new List<SolverField>();
          for (int j_2 = j - 5; j_2 <= j + 5; j_2++)
          {

            if (j_2 < 0 || j_2 >= jMax)
            {
              leftRightList.Add(new SolverField(j_2, i, oponent));
            }
            else
            {
              leftRightList.Add(Array[i][j_2]);
            }
          }
          Array[i][j].ScantThreatsFor(player, leftRightList);



          // Top-Left -> Bottom-Right
          List<SolverField> tl_br_List = new List<SolverField>();
          for (int i_2 = i - 5, j_2 = j - 5; i_2 <= i + 5; i_2++, j_2++)
          {

            if (i_2 < 0 || j_2 < 0 || i_2 >= iMax || j_2 >= jMax)
            {
              tl_br_List.Add(new SolverField(j_2, i_2, oponent));
            }
            else
            {
              tl_br_List.Add(Array[i_2][j_2]);
            }
          }
          Array[i][j].ScantThreatsFor(player, tl_br_List);


          // Bottom-Left -> Top-Right
          List<SolverField> bl_tr_List = new List<SolverField>();
          for (int i_2 = i + 5, j_2 = j - 5; i_2 >= i - 5; i_2--, j_2++)
          {

            if (i_2 < 0 || j_2 < 0 || i_2 >= iMax || j_2 >= jMax)
            {
              bl_tr_List.Add(new SolverField(j_2, i_2, oponent));
            }
            else
            {
              bl_tr_List.Add(Array[i_2][j_2]);
            }
          }
          Array[i][j].ScantThreatsFor(player, bl_tr_List);


        }
      }
     
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
