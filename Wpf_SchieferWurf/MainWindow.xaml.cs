namespace Wpf_TrajectorySimulation
{
    using System;
    using System.Windows;

    public partial class MainWindow : Window
    {
        private TrajectoryModel model;

        public MainWindow()
        {
            this.InitializeComponent();

            this.model = new TrajectoryModel();
            this.model.PositionChanged += this.Model_PositionChanged;
        }
        private void Button_Click(Object sender, RoutedEventArgs e)
        {
            if (sender == this.ButtonStart)
            {
                this.TrajectoryView.Clear();
                this.TrajectoryView.ShowTrajectory = true;

                this.model.StartAngle = this.TrajectorySettings.StartAngle;
                this.model.StartHeight = this.TrajectorySettings.StartHeight;
                this.model.StartVelocity = this.TrajectorySettings.StartVelocity;
                this.model.Start();
            }
            else if (sender == this.ButtonClear)
            {
                this.TrajectoryView.Clear();
            }
        }

        private void Model_PositionChanged(Point p, double t)
        {
            this.TrajectoryView.BallPosition = p;
            this.TextBoxTimeElapsed.Text = String.Format("{0:00.00} sec.", t);
        }
    }
}
