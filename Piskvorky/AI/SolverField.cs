using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Piskvorky.AI
{
  public class SolverField
  {

    public int X { get; set; }
    public int Y { get; set; }

    public EType Type { get; set; }


    public Dictionary<EType, int> ValueBy = new Dictionary<EType, int>();
    Dictionary<EType, List<EThreat>> ThreatsBy = new Dictionary<EType, List<EThreat>>();

    public List<EThreat> GetMyThreats(EType player)
    {
      return ThreatsBy[player];
    }
    public List<EThreat> GetOponentThreats(EType player)
    {
      return ThreatsBy[player == EType.Circle ? EType.Cross : EType.Circle];
    }

    public bool IsWinningMoveImmediate(EType player)
    {
      var myThreats = GetMyThreats(player);

      if (myThreats.Contains(EThreat.Five))
      {
        return true;
      }
      return false;
    }

    public bool IsWinningMoveCurrent(EType player)
    {
      var myThreats = GetMyThreats(player);

      if (myThreats.Contains(EThreat.OpenFour))
      {
        return true;
      }
      return false;
    }

    public bool IsForcedThreatImmediate(EType player)
    {
      var oponentThreats = GetOponentThreats(player);

      if (oponentThreats.Contains(EThreat.Five))
      {
        return true;
      }
      return false;
    }

    internal bool IsWinningMoveNext(EType player)
    {
      // two or more of following:
      //simple four
      //open three
      //broken three
      var myThreats = GetMyThreats(player);

      int cnt = myThreats.Count(x =>
        x == EThreat.SimpleFour ||
        x == EThreat.OpenThree ||
        x == EThreat.BrokenThree
      );

      return cnt >= 2;
    }

    internal int GetThreatValue()
    {
      int total = 0;
      foreach(EThreat th in ThreatsBy[EType.Circle])
      {
        total += GetThreatValue(th);
      }
      foreach (EThreat th in ThreatsBy[EType.Cross])
      {
        total += GetThreatValue(th);
      }
      return total;
    }

    private int GetThreatValue(EThreat threat)
    {
      switch (threat)
      {
        case EThreat.Five:
          return 10000;
        case EThreat.OpenFour:
          return 5000;
        case EThreat.SimpleFour:
          return 2200;
        case EThreat.OpenThree:
          return 1000;
        case EThreat.BrokenThree:
          return 1000;
        case EThreat.SimpleThree:
          return 250;
        case EThreat.Two:
          return 150;

        default:
          throw new Exception($"Value not assigned for {threat}");
      }
    }




    internal bool IsForcedMoveCurrent(EType player)
    {
      var oponentThreats = GetOponentThreats(player);

      if (oponentThreats.Contains(EThreat.OpenFour) ||
         oponentThreats.Contains(EThreat.Five))
      {
        return true;
      }
      return false;
    }

    internal bool IsForcedMoveNext(EType player)
    {
      var oponentThreats = GetOponentThreats(player);

      int cnt = oponentThreats.Count(x =>
       x == EThreat.SimpleFour ||
       x == EThreat.OpenThree ||
       x == EThreat.BrokenThree
      );
      if (cnt >= 2)
      {
        return true;
      }
      return false;
    }

    public void ScantThreatsFor(EType type, List<SolverField> fields)
    {
      StringBuilder sb = new StringBuilder();
      foreach(SolverField sf in fields)
      {
        switch (sf.Type)
        {
          case EType.Empty:
            if(sf == this)
            {
              sb.Append('#');
            } else
            {
              sb.Append('-');
            }
            break;
          case EType.Cross:
            sb.Append(type == EType.Circle ? 'X' : 'O');
            break;
          case EType.Circle:
            sb.Append(type == EType.Circle ? 'O' : 'X');
            break;
        }
      }

      string input = sb.ToString();


      int myIndex = fields.IndexOf(this);
      if (myIndex == -1) throw new Exception("Field is not in the list");

      if(CheckFive(input))
      {
        ThreatsBy[type].Add(EThreat.Five);
      }

      if (CheckOpenFour(input))
      {
        ThreatsBy[type].Add(EThreat.OpenFour);
      }

      if(CheckSimpeFour(input))
      {
        ThreatsBy[type].Add(EThreat.SimpleFour);
      }

      if (CheckOpenThree(input))
      {
        ThreatsBy[type].Add(EThreat.OpenThree);
      } else
      { 
        // only if not open three
        if (CheckBrokenThree(input))
        {
          ThreatsBy[type].Add(EThreat.BrokenThree);
        }
      }

      if(CheckSimpleThree(input))
      {
        ThreatsBy[type].Add(EThreat.SimpleThree);
      }

      if (CheckTwo(input))
      {
        ThreatsBy[type].Add(EThreat.Two);
      }


    }


    private static Regex RxTwo = new Regex("O#---|O-#--|O--#-|O---#|-O#--|-O-#-|-O--#|--O#-|--O-#|---O#");
    private bool CheckTwo(string input)
    {
      string reverseInput = reverse(input);
      return RxTwo.IsMatch(input) || RxTwo.IsMatch(reverseInput);
      // O#---
      // O-#--
      // O--#-
      // O---#

      // -O#--
      // -O-#-
      // -O--#

      // --O#-
      // --O-#

      // ---O#

    }

    private static Regex RxFive = new Regex("OOOO#|OOO#O|OO#OO|O#OOO|#OOOO");
    private bool CheckFive(string input)
    {
      return RxFive.IsMatch(input);
      // OOOO#
      // OOO#O
      // OO#OO
      // O#OOO
      // #OOOO
    }

    private static Regex RxBrokenThree = new Regex("-#-OO-|-O-#O-|-O-O#-|-#O-O-|-O#-O-|-OO-#-|X-O#O--|X-OO#--|--#OO-X|--O#O-X");
    private bool CheckBrokenThree(string input)
    {
      return RxBrokenThree.IsMatch(input);
      // -#-OO-
      // -O-#O-
      // -O-O#-

      // -#O-O-
      // -O#-O-
      // -OO-#-

      // X-O#O--
      // X-OO#--
      // --#OO-X
      // --O#O-X

    }

    private static Regex RxOpenThree = new Regex("--#OO--|--O#O--|--OO#--|-O-O#-O-|-O-#O-O-|-O-O-#-O-O");
    private bool CheckOpenThree(string input)
    {
      return RxOpenThree.IsMatch(input);
      // --#OO--
      // --O#O--
      // --OO#--

      // -O-O#-O-
      // -O-#O-O-

      // -O-O-#-O-O
    }

    private static Regex RxSimpleFour = new Regex("XOOO#-|-#OOOX|#-OOO|O-#OO|O-O#O|O-OO#|#O-OO|O#-OO|OO-#O|OO-O#|#OO-O|O#O-O|OO#-O|OOO-#");

    private bool CheckSimpeFour(string input)
    {
      return RxSimpleFour.IsMatch(input);

      // XOOO#-
      // -#OOOX

      // #-OOO
      // O-#OO
      // O-O#O
      // O-OO#

      // #O-OO
      // O#-OO
      // OO-#O
      // OO-O#

      // #OO-O
      // O#O-O
      // OO#-O
      // OOO-#
    }
    private static Regex RxOpenFour = new Regex("-#OOO-|-O#OO-|-OO#O-|-OOO#-|XOOO-#-OOOX|XOO-#O-OOX|XOO-O#-OOX");
    private bool CheckOpenFour(string input)
    {
      return RxOpenFour.IsMatch(input);
      // -#OOO-
      // -O#OO-
      // -OO#O-
      // -OOO#-

      // XOOO-#-OOOX
      // XOO-#O-OOX
      // XOO-O#-OOX
    }


    public string reverse(string s)
    {
      char[] charArray = s.ToCharArray();
      Array.Reverse(charArray);
      return new string(charArray);
    }
    private static Regex RxSimpleThree = new Regex("XOO#--|XO#O--|X#OO--|X#-OO-|XO-#O-|XO-O#-|X#--OO|XO--#O|XO--O#|X#--OO|XO--#O|XO--O#|X#-O-O|XO-#-O|XO-O-#|X#O-O-|XO#-O-|XOO-#-|X#O--O|XO#--O|XOO--#");
    private bool CheckSimpleThree(string input)
    {
      string reverseInput = reverse(input);
      return RxSimpleThree.IsMatch(input) || RxSimpleThree.IsMatch(reverseInput);

      // XOO#--
      // XO#O--
      // X#OO--

      // X#-OO-
      // XO-#O-
      // XO-O#-

      // X#--OO
      // XO--#O
      // XO--O#

      // X#--OO
      // XO--#O
      // XO--O#

      // X#-O-O
      // XO-#-O
      // XO-O-#

      // X#O-O-
      // XO#-O-
      // XOO-#-

      // X#O--O
      // XO#--O
      // XOO--#
    }



    public SolverField(int x, int y, EType type)
    {
      X = x;
      Y = y;
      Type = type;
      ThreatsBy.Add(EType.Circle, new List<EThreat>());
      ThreatsBy.Add(EType.Cross, new List<EThreat>());
      ValueBy.Add(EType.Circle, 0);
      ValueBy.Add(EType.Cross, 0);
    }

  }

  public enum EType
  {
    Empty,
    Cross,
    Circle
  }

  public enum EThreat
  {
    Five,
    OpenFour,
    SimpleFour,
    OpenThree,
    BrokenThree,
    SimpleThree,
    Two,
  }
}
