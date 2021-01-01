using Piskvorky.User;
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

namespace Piskvorky.FiveInARow
{
  /// <summary>
  /// Interaction logic for Desk.xaml
  /// </summary>
  public partial class Desk : UserControl
  {

    //private double squareSize = 15;

    public event Action<int, int> EmptyPointClicked;
    public event Action<int, int> FilledPointClicked;

    public Desk()
    {
      InitializeComponent();
    }

    public void Draw()
    {
      cnvDesk.Children.Clear();

      if(DataContext is DeskData deskData)
      {
        DisplayInfo(deskData);
        SetSize(deskData);
        DrawGrid(deskData);
        DrawPoints(deskData);
        cnvDesk.RenderTransformOrigin = new Point(0.5, 0.5);
        cnvDesk.RenderTransform = new ScaleTransform(1, -1);
      }
    }

    private void DisplayInfo(DeskData deskData)
    {
      tbTurn.Text = deskData.IsMyTurn ? "My turn" : "Oponent's turn";
      tbMySymbol.Text = deskData.MySymbol == ESymbol.Circle ? "Circle" : "Cross";
      tbWinner.Text = deskData.Winner;
    }

    private void SetSize(DeskData deskData)
    {
      int xColumns = deskData.EndIndexX - deskData.StartIndexX + 1;
      int yRows = deskData.EndIndexY - deskData.StartIndexY + 1;

      cnvDesk.Width = xColumns * deskData.SquareSize;
      cnvDesk.Height = yRows * deskData.SquareSize;
  
    }

    private void DrawGrid(DeskData deskData)
    {
    
      if (deskData == null) return;

      double width = cnvDesk.Width;
      double height = cnvDesk.Height;

      int xColumns = deskData.EndIndexX - deskData.StartIndexX + 1;
      int yRows = deskData.EndIndexY - deskData.StartIndexY + 1;

      Brush color = Brushes.Gray;

      double eps = 0.000001;

      double stepX = width / xColumns;
      for (double i = 0; i - eps <= width; i += stepX)
      {
        Line line = new Line() { X1 = i, Y1 = 0, X2 = i, Y2 = height };
        line.Stroke = color;
        cnvDesk.Children.Add(line);
      }

      double stepY = height / yRows;
      for (double i = 0; i - eps <= height; i += stepY)
      {
        Line line = new Line() { X1 = 0, Y1 = i, X2 = width, Y2 = i };
        line.Stroke = color;
        cnvDesk.Children.Add(line);
      }
    }

    private void DrawPoints(DeskData deskData)
    {
      double offset = deskData.SquareSize / 7;
      double lineWidth = deskData.SquareSize / 7;
      foreach(PointData p in deskData.CirclePoints)
      {
        DrawCircle(deskData, p, Brushes.Green, offset, lineWidth);
      }
      foreach(PointData p in deskData.CrossPoints)
      {
        DrawCross(deskData, p, Brushes.Red, offset, lineWidth);
      }
    }

    private void DrawCircle(DeskData deskData, PointData p, Brush color, double offset, double lineWidth)
    {
      double totalSize = (deskData.EndIndexY - deskData.StartIndexY) * deskData.SquareSize;
      Ellipse elipse = new Ellipse() {  Width = deskData.SquareSize - 2 * offset, Height = deskData.SquareSize - 2 * offset };
      elipse.RenderTransform = new TranslateTransform(offset, offset);
      Canvas.SetTop(elipse,  (p.Y +  (-deskData.StartIndexY)) * deskData.SquareSize);
      Canvas.SetLeft(elipse, (p.X + (-deskData.StartIndexX)) * deskData.SquareSize);
      elipse.Stroke = color;
      elipse.StrokeThickness = lineWidth;
      cnvDesk.Children.Add(elipse);
    }

    private void DrawCross(DeskData deskData,PointData p, Brush color, double offset, double lineWidth)
    {
      double totalSize = (deskData.EndIndexY - deskData.StartIndexY) * deskData.SquareSize;
      Line line1 = new Line() { X1 = offset, X2 = deskData.SquareSize - offset, Y1 = offset, Y2 = deskData.SquareSize - offset };
      Canvas.SetTop(line1,  (p.Y +  (-deskData.StartIndexY)) * deskData.SquareSize);
      Canvas.SetLeft(line1, (p.X + (-deskData.StartIndexX)) * deskData.SquareSize);
      line1.Stroke = color;
      line1.StrokeThickness = lineWidth;
      cnvDesk.Children.Add(line1);

      Line line2 = new Line() { X1 = offset, X2 = deskData.SquareSize - offset, Y2 = offset, Y1 = deskData.SquareSize - offset };
      Canvas.SetTop(line2,  (p.Y + (-deskData.StartIndexY)) * deskData.SquareSize);
      Canvas.SetLeft(line2, (p.X + (-deskData.StartIndexX)) * deskData.SquareSize);
      line2.Stroke = color;
      line2.StrokeThickness = lineWidth;
      cnvDesk.Children.Add(line2);
    }

    private void cnvDesk_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      Draw();
    }

    private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if(e.NewValue is DeskData deskData)
      {
        Draw();
      }
    }

    private void cnvDesk_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (DataContext is DeskData data)
      {
        Point p = e.GetPosition(cnvDesk);
        int x = (int)((int)(p.X / data.SquareSize) + data.StartIndexX);
        int y = (int)((int)(p.Y / data.SquareSize) + data.StartIndexY);

        if(!(data.CirclePoints.Any(pt=> pt.X == x && pt.Y == y) ||
          data.CrossPoints.Any(pt => pt.X == x && pt.Y == y)))
        {
          EmptyPointClicked?.Invoke(x, y);
        }

        
      }
     
    }

    private void cnvDesk_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (DataContext is DeskData data)
      {
        Point p = e.GetPosition(cnvDesk);
        int x = (int)((int)(p.X / data.SquareSize) + data.StartIndexX);
        int y = (int)((int)(p.Y / data.SquareSize) + data.StartIndexY);

        if ((data.CirclePoints.Any(pt => pt.X == x && pt.Y == y) ||
          data.CrossPoints.Any(pt => pt.X == x && pt.Y == y)))
        {
          FilledPointClicked?.Invoke(x, y);
        }
      }
    }
  }
}
