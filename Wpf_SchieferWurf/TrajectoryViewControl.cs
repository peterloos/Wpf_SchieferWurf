namespace Wpf_TrajectorySimulation
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class TrajectoryViewControl : Canvas
    {
        private const double MaxSizeX = 25;  // unit 'meter'
        private const double MaxSizeY = 15;  // unit 'meter'

        private const double BaseUnits = 20;
        private const double CanvasWidth = (MaxSizeX + 2) * BaseUnits;
        private const double CanvasHeight = (MaxSizeY + 2) * BaseUnits;

        private const int BallDiameter = 12;
        private Ellipse ball;

        private int lastChild;
        private double lastX;
        private double lastY;

        public static readonly DependencyProperty BallHeightProperty;

        // static c'tor
        static TrajectoryViewControl()
        {
            BallHeightProperty = DependencyProperty.Register(
                "BallHeight",                   // name of property
                typeof(double),                 // type of property
                typeof(TrajectoryViewControl),  // type of property owner
                new PropertyMetadata(0.0, OnBallHeightPropertyChanged)
            );
        }

        // c'tor
        public TrajectoryViewControl()
        {
            this.Background = Brushes.LightGray;

            // set fixed size of this control
            this.Height = CanvasHeight;
            this.Width = CanvasWidth;

            // draw coordinates
            this.DrawCoordinateAxes();

            // prepare projectile
            this.ball = new Ellipse();
            this.ball.Fill = Brushes.Black;
            this.ball.Height = BallDiameter;
            this.ball.Width = BallDiameter;
            this.DrawBallAbsolute(
                BaseUnits - BallDiameter / 2,
                CanvasHeight - BaseUnits - BallDiameter / 2);
            this.Children.Add(this.ball);

            // store current number of children controls for later control reset
            this.lastChild = this.Children.Count;
            this.lastX = -1;
            this.lastY = -1;
        }

        // properties
        public bool ShowTrajectory { get; set; }

        public double BallHeight
        {
            get { return (double) this.GetValue(BallHeightProperty); }
            set { this.SetValue(BallHeightProperty, value); }
        }

        public Point BallPosition
        {
            set
            {
                this.DrawBall(value);
            }
        }

        // public interface
        public void Clear()
        {
            this.Children.RemoveRange(this.lastChild, this.Children.Count - this.lastChild);

            this.ShowTrajectory = false;
            this.BallPosition = new Point() { X = 0.0, Y = this.BallHeight };

            this.lastX = -1;
            this.lastY = -1;
        }

        // private helper methods
        private static void OnBallHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrajectoryViewControl control = (TrajectoryViewControl)d;

            if (e.NewValue != null)
            {
                control.DrawBall(new Point() { X = 0.0, Y = (double)e.NewValue });
            }
        }

        private void DrawBall(Point p)
        {
            double x = BaseUnits + p.X * BaseUnits;
            double y = CanvasHeight - BaseUnits - p.Y * BaseUnits;

            this.DrawBallAbsolute(x - BallDiameter / 2, y - BallDiameter / 2);

            if (this.ShowTrajectory)
            {
                if (this.lastX == -1 && this.lastY == -1)
                {
                    // just store coordinates, if it's first point
                    this.lastX = x;
                    this.lastY = y;
                }
                else
                {
                    Line segment = new Line();
                    segment.X1 = this.lastX;
                    segment.Y1 = this.lastY;
                    segment.X2 = x;
                    segment.Y2 = y;
                    segment.Stroke = Brushes.Red;
                    segment.StrokeThickness = 2;
                    this.Children.Add(segment);

                    this.lastX = x;
                    this.lastY = y;
                }
            }
        }

        private void DrawBallAbsolute(double x, double y)
        {
            this.ball.SetValue(Canvas.LeftProperty, x);
            this.ball.SetValue(Canvas.TopProperty, y);
        }

        private void DrawCoordinateAxes()
        {
            // draw coordinate axes
            Line xAxes = new Line();
            xAxes.X1 = BaseUnits;
            xAxes.Y1 = 1 * BaseUnits + MaxSizeY * BaseUnits;
            xAxes.X2 = BaseUnits + (MaxSizeX * BaseUnits);
            xAxes.Y2 = 1 * BaseUnits + MaxSizeY * BaseUnits;
            xAxes.Stroke = Brushes.Black;
            xAxes.StrokeThickness = 2;
            this.Children.Add(xAxes);

            Line yAxes = new Line();
            yAxes.X1 = BaseUnits;
            yAxes.Y1 = 1 * BaseUnits + MaxSizeY * BaseUnits;
            yAxes.X2 = BaseUnits;
            yAxes.Y2 = BaseUnits;
            yAxes.Stroke = Brushes.Black;
            yAxes.StrokeThickness = 2;
            this.Children.Add(yAxes);

            // draw units of coordinate axes
            for (int i = 1; i < MaxSizeX; i++)
            {
                Line unit = new Line();
                unit.X1 = BaseUnits + i * BaseUnits;
                unit.Y1 = BaseUnits + MaxSizeY * BaseUnits - 3;
                unit.X2 = BaseUnits + i * BaseUnits;
                unit.Y2 = BaseUnits + MaxSizeY * BaseUnits + 3;
                unit.Stroke = Brushes.Black;
                unit.StrokeThickness = 2;
                this.Children.Add(unit);
            }

            for (int i = 1; i < MaxSizeY; i++)
            {
                Line unit = new Line();
                unit.X1 = BaseUnits - 3;
                unit.Y1 = BaseUnits + i * BaseUnits;
                unit.X2 = BaseUnits + 3;
                unit.Y2 = BaseUnits + i * BaseUnits;
                unit.Stroke = Brushes.Black;
                unit.StrokeThickness = 2;
                this.Children.Add(unit);
            }
        }
    }
}
