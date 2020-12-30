using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piskvorky.AI
{
  public class SolverField
  {

    public int X { get; set; }
    public int Y { get; set; }

    public EType Type { get; set; }

    public SolverField(int x, int y, EType type)
    {
      X = x;
      Y = y;
      Type = type;
    }

  }

  public enum EType
  {
    Empty,
    Cross,
    Circle
  }
}
