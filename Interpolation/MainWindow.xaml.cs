using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Interpolation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<Point> points = new List<Point>();
    public MainWindow()
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Point point = e.GetPosition(DrawingCanvas);
        points.Add(point);

        // Отрисовать точку
        Ellipse ellipse = new Ellipse
        {
            Width = 5,
            Height = 5,
            Fill = Brushes.Black
        };
        Canvas.SetLeft(ellipse, point.X - 1.5);
        Canvas.SetTop(ellipse, point.Y - 1.5);
        DrawingCanvas.Children.Add(ellipse);

        // Соединить точки, если их больше одной
        if (points.Count > 1)
        {
            DrawLagrangeCurve();
        }
    }

    private void DrawLagrangeCurve()
    {
        // Очистить предыдущие линии
        for (int i = DrawingCanvas.Children.Count - 1; i >= 0; i--)
        {
            if (DrawingCanvas.Children[i] is Line)
            {
                DrawingCanvas.Children.RemoveAt(i);
            }
        }

        for (double x = 0; x < DrawingCanvas.ActualWidth; x += 1)
        {
            double y = LagrangeInterpolate(points, x);

            if (x > 0)
            {
                double prevY = LagrangeInterpolate(points, x - 1);
                Line line = new Line
                {
                    X1 = x - 1,
                    Y1 = prevY,
                    X2 = x,
                    Y2 = y,
                    Stroke = Brushes.DarkBlue,
                    StrokeThickness = 2
                };
                DrawingCanvas.Children.Add(line);
            }
        }
    }

    private double LagrangeInterpolate(List<Point> points, double x)
    {
        int n = points.Count;
        double result = 0;

        for (int i = 0; i < n; i++)
        {
            double term = points[i].Y;
            for (int j = 0; j < n; j++)
            {
                if (j != i)
                {
                    term *= (x - points[j].X) / (points[i].X - points[j].X);
                }
            }

            result += term;
        }

        return result;
    }
}