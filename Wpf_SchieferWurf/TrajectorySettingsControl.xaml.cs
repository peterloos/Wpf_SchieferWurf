namespace Wpf_TrajectorySimulation
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public partial class TrajectorySettingsControl : UserControl
    {
        private double startVelocity;
        private double startAngle;

        public static readonly DependencyProperty StartHeightProperty;

        // static c'tor
        static TrajectorySettingsControl()
        {
            StartHeightProperty = DependencyProperty.Register(
                "StartHeight",   // name of property
                typeof(double),  // type of property
                typeof(TrajectorySettingsControl),   // type of property owner
                new PropertyMetadata(0.0)            // metadata
            );
        }

        // c'tor
        public TrajectorySettingsControl()
        {
            this.InitializeComponent();

            this.SliderStartAngle.Value = 45.0;
            this.SliderStartHeight.Value = 5.0;
            this.SliderStartVelocity.Value = 10.0;
        }

        // properties
        public double StartHeight
        {
            get { return (double) this.GetValue(StartHeightProperty); }
            set { this.SetValue(StartHeightProperty, value); }
        }

        public double StartVelocity
        {
            get { return this.startVelocity; }
        }

        public double StartAngle
        {
            get { return this.startAngle; }
        }

        private void Slider_ValueChanged(
            Object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender == this.SliderStartAngle)
            {
                this.TextBoxLaunchAngle.Text = String.Format("{0:00.00}",
                    this.SliderStartAngle.Value);

                this.startAngle = this.SliderStartAngle.Value;
            }
            else if (sender == this.SliderStartHeight)
            {
                this.TextBoxLaunchHeight.Text = String.Format("{0:00.00}",
                    this.SliderStartHeight.Value);

                this.StartHeight = this.SliderStartHeight.Value;
            }
            else if (sender == this.SliderStartVelocity)
            {
                this.TextBoxLaunchVelocity.Text = String.Format("{0:00.00}",
                    this.SliderStartVelocity.Value);

                this.startVelocity = this.SliderStartVelocity.Value;
            }
        }
    }
}
