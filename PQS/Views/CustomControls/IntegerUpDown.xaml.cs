using System;
using System.Windows;
using System.Windows.Controls;

namespace PQS.Views.CustomControls
{
    /// <summary>
    /// Interaction logic for IntegerUpDown.xaml
    /// </summary>
    public partial class IntegerUpDown : UserControl
    {
        public IntegerUpDown()
        {
            InitializeComponent();
        }

        #region Dependency Properties
        public static readonly DependencyProperty SetMinimumProperty =
         DependencyProperty.Register("Minimum", typeof(int?), typeof(IntegerUpDown), new
            PropertyMetadata(int.MinValue, null));

        public int? Minimum
        {
            get { return (int?)GetValue(SetMinimumProperty); }
            set { SetValue(SetMinimumProperty, value); }
        }

        public static readonly DependencyProperty SetMaximumProperty =
         DependencyProperty.Register("Maximum", typeof(int?), typeof(IntegerUpDown), new
            PropertyMetadata(int.MaxValue, null));

        public int? Maximum
        {
            get { return (int?)GetValue(SetMaximumProperty); }
            set { SetValue(SetMaximumProperty, value); }
        }

        public static readonly DependencyProperty SetValueProperty =
         DependencyProperty.Register("Value", typeof(int?), typeof(IntegerUpDown),
             new FrameworkPropertyMetadata(null,
                                  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int? Value
        {
            get
            {
                return (int?)GetValue(SetValueProperty);
            }
            set
            {
                SetValue(SetValueProperty, value);
                numText.Text = value == null ? null : value.ToString();
            }
        }
        #endregion

        private void Increase(object sender, RoutedEventArgs e)
        {
            if (ValidateText())
            {
                int Num = ConvertStringToInt();

                if (Num == Maximum)
                    AssignValue(Minimum, 0, '+'); // loop to start if at end
                else
                    AssignValue(Num, 1, '+'); // increment current value
            }
            else
            {
                if (string.IsNullOrWhiteSpace(numText.Text) && Minimum != null)
                    AssignValue(Minimum, 1, '+'); // add 1 to minimum
                else
                    AssignValue(Minimum, 0, '+'); // assume 0 is the start, to put 0 in 
            }
        }

        private void Decrease(object sender, RoutedEventArgs e)
        {
            if (ValidateText())
            {
                int Num = ConvertStringToInt();

                if (Num == Minimum)
                    AssignValue(Maximum, 0, '-'); // loop to end if at minimum and decremented
                else numText.Text = (Num - 1).ToString();
            }
            else
            {
                if (ValidateIfTextIsEmpty(Maximum))
                    AssignValue(Maximum, 1, '-'); // minus 1 from the max
                else AssignValue(Maximum, 0, '-'); // show max if dec pressed and no valu
            }
        }

        private bool ValidateText()
        {
            int result;
            return int.TryParse(numText.Text, out result);
        }

        private bool ValidateIfTextIsEmpty(int? range)
        {
            return string.IsNullOrWhiteSpace(numText.Text) && range != null;
        }

        private int ConvertStringToInt()
        {
            return Convert.ToInt32(numText.Text);
        }

        private void AssignValue(int? number, int changeValue, char oper)
        {
            switch (oper)
            {
                case '+':
                    numText.Text = (number + changeValue).ToString();
                    break;
                case '-':
                    numText.Text = (number - changeValue).ToString();
                    break;
            }
        }
    }
}