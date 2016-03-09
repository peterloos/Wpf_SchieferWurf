namespace Wpf_TrajectorySimulation
{
    using System;
    using System.Windows;
    using System.Windows.Threading;

    public delegate void PositionChangedHandler(Point p, double elapsed);

    public class TrajectoryModel
    {
        private const double Gravity = 9.81;  // m/s2

        private double startHeight;
        private double startVelocity;
        private double startAngleDegree;      // Startwinkel im Gradmaß
        private double startAngleRadian;      // Startwinkel im Bogenmaß

        private const int TimerInterval = 10; 
        private double TimeDelta = 1.0 / 100.0; 

        private DispatcherTimer timer;
        private double t;   // elapsed time

        public event PositionChangedHandler PositionChanged;

        // c'tor
        public TrajectoryModel ()
        {
            this.startHeight = 0.0;
            this.startVelocity = 0.0;
            this.startAngleDegree = 0.0;

            // timer setup
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.TrajectoryTimerTick;
            this.timer.Interval = TimeSpan.FromMilliseconds(TimerInterval);
        }

        // properties
        public double StartHeight
        {
            set
            {
                this.startHeight = 0.0;
                if (value >= 0.0 && value <= 15.0)
                {
                    this.startHeight = value;
                }
            }

            get
            {
                return this.startHeight; 
            }
        }

        public double StartVelocity
        {
            set
            {
                this.startVelocity = 0.0;
                if (value >= 0.0 && value <= 100.0)
                {
                    this.startVelocity = value;
                }
            }

            get
            {
                return this.startVelocity;
            }
        }

        public double StartAngle
        {
            set
            {
                this.startAngleDegree = 0.0;
                this.startAngleRadian = 0.0;

                if (value >= 0.0 && value <= 90.0)
                {
                    this.startAngleDegree = value;
                    this.startAngleRadian =
                        this.ConvertDegreesToRadians(this.startAngleDegree);
                }
            }

            get
            {
                return this.startAngleDegree;
            }
        }

        // public methods
        public void Start()
        {
            this.t = 0.0;
            this.timer.Start();
        }

        // private helper methods
        private void TrajectoryTimerTick(Object sender, EventArgs e)
        {
            Point p = this.CalcPosition(t);

            if (p.Y < 0)
            {
                this.timer.Stop();
            }

            this.t += TimeDelta;

            if (this.PositionChanged != null)
                this.PositionChanged.Invoke(p, this.t);
        }

        private double CalcX(double t)
        {
            return this.startVelocity * Math.Cos(this.startAngleRadian) * t;
        }

        private double CalcY(double t)
        {
            return
                this.startHeight
                    + (this.startVelocity * Math.Sin(this.startAngleRadian) * t)
                    - 0.5 * Gravity * t * t;
        }

        private Point CalcPosition(double t)
        {
            return new Point(this.CalcX(t), this.CalcY(t));
        }

        private double ConvertDegreesToRadians(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }
    }
}
