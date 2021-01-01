using Piskvorky.AI;
using Piskvorky.FiveInARow;
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

namespace Piskvorky.Tests
{
  /// <summary>
  /// Interaction logic for TestGameUI.xaml
  /// </summary>
  public partial class TestGameUI : UserControl
  {
    public TestGameUI()
    {
      InitializeComponent();

      ucDesk.EmptyPointClicked += UcDesk_PointClicked;
      ucDesk.FilledPointClicked += UcDesk_FilledPointClicked;
    }

    private void UcDesk_FilledPointClicked(int x, int y)
    {
      if (DataContext is TestGame dd)
      {
        dd.DeskData.CirclePoints.RemoveAll(pt=> pt.X == x && pt.Y == y);
        dd.DeskData.CrossPoints.RemoveAll(pt => pt.X == x && pt.Y == y);
        ucDesk.Draw();
      }
    }

    private void UcDesk_PointClicked(int x, int y)
    {
      if (DataContext is TestGame dd)
      {
        if (dd.DeskData.CurrentPlayerSymbol == ESymbol.Circle)
        {
          dd.DeskData.CirclePoints.Add(new PointData() { X = x, Y = y });
        }
        else
        {
          dd.DeskData.CrossPoints.Add(new PointData() { X = x, Y = y });
        }
        dd.DeskData.IsMyTurn = !dd.DeskData.IsMyTurn;
        ucDesk.Draw();
      }
      
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      if (DataContext is TestGame tg)
      {
        Solver solver = new Solver();
        PointData pd = solver.GetNextMove(tg.DeskData);

        
        if (tg.DeskData.CurrentPlayerSymbol == ESymbol.Circle)
        {
          tg.DeskData.CirclePoints.Add(pd);
        }
        else
        {
          tg.DeskData.CrossPoints.Add(pd);
        }
        tg.DeskData.IsMyTurn = !tg.DeskData.IsMyTurn;
        ucDesk.Draw();
      }
      
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      if (DataContext is TestGame tg)
      {
        tg.DeskData = new DeskData();
      }
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
      if (DataContext is TestGame tg)
      {
        tg.DeskData.IsMyTurn = !tg.DeskData.IsMyTurn;
        ucDesk.Draw();
      }
    }

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
      if (DataContext is TestGame tg)
      {
        tg.Save();
      }
    }

    private void Button_Click_4(object sender, RoutedEventArgs e)
    {
      this.DataContext = TestGame.Load();
    }
  }
}
