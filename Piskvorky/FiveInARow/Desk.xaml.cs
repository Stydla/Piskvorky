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

    //private DeskData deskData;
    private int squareSize = 15;

    public Desk()
    {
      InitializeComponent();
    }

    //public void Draw(DeskData dd)
    //{
    //  deskData = dd;
    //  Draw();
    //}

    private void Draw()
    {
      cnvDesk.Children.Clear();
      if (DataContext is Data data)
      {
        DeskData deskData = data.DeskData;
        if (deskData == null) return;

        DisplayInfo(deskData);
        SetSize(deskData);
        DrawGrid(deskData);
        DrawPoints(deskData);
        //cnvDesk.RenderTransformOrigin = new Point(0.5, 0.5);
        //cnvDesk.RenderTransform = new ScaleTransform(1, -1);
      }
    
    }

    private void DisplayInfo(DeskData deskData)
    {
      tbTurn.Text = deskData.IsMyTurn ? "My turn" : "Oponent's turn";
      tbMySymbol.Text = deskData.MySymbol == ESymbol.Circle ? "Circle" : "Cross";
    }

    private void SetSize(DeskData deskData)
    {
      int xColumns = deskData.EndIndexX - deskData.StartIndexX + 1;
      int yRows = deskData.EndIndexY - deskData.StartIndexY + 1;

      cnvDesk.Width = xColumns * squareSize;
      cnvDesk.Height = yRows * squareSize;
  
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
      int offset = 2;
      int lineWidth = 2;
      foreach(PointData p in deskData.CirclePoints)
      {
        DrawCircle(deskData, p, Brushes.Green, offset, lineWidth);
      }
      foreach(PointData p in deskData.CrossPoints)
      {
        DrawCross(deskData, p, Brushes.Red, offset, lineWidth);
      }
    }

    private void DrawCircle(DeskData deskData, PointData p, Brush color, int offset, int lineWidth)
    {
      int totalSize = (deskData.EndIndexY - deskData.StartIndexY) * squareSize;
      Ellipse elipse = new Ellipse() {  Width = squareSize - 2 * offset, Height = squareSize - 2 * offset };
      elipse.RenderTransform = new TranslateTransform(offset, offset);
      Canvas.SetTop(elipse, totalSize - (p.Y +  (-deskData.StartIndexY)) * squareSize);
      Canvas.SetLeft(elipse, (p.X + (-deskData.StartIndexX)) * squareSize);
      elipse.Stroke = color;
      elipse.StrokeThickness = lineWidth;
      cnvDesk.Children.Add(elipse);
    }

    private void DrawCross(DeskData deskData,PointData p, Brush color, int offset, int lineWidth)
    {
      int totalSize = (deskData.EndIndexY - deskData.StartIndexY) * squareSize;
      Line line1 = new Line() { X1 = offset, X2 = squareSize - offset, Y1 = offset, Y2 = squareSize - offset };
      Canvas.SetTop(line1, totalSize - (p.Y +  (-deskData.StartIndexY)) * squareSize);
      Canvas.SetLeft(line1, (p.X + (-deskData.StartIndexX)) * squareSize);
      line1.Stroke = color;
      line1.StrokeThickness = lineWidth;
      cnvDesk.Children.Add(line1);

      Line line2 = new Line() { X1 = offset, X2 = squareSize - offset, Y2 = offset, Y1 = squareSize - offset };
      Canvas.SetTop(line2, totalSize - (p.Y + (-deskData.StartIndexY)) * squareSize);
      Canvas.SetLeft(line2, (p.X + (-deskData.StartIndexX)) * squareSize);
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
      if(e.NewValue is Data data)
      {
        data.PropertyChanged += Data_PropertyChanged;
      }
    }

    private void Data_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      Draw();
    }

    private void cnvDesk_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      
    }
  }
}
