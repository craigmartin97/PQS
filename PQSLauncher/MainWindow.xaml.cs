using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PQSLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            /**
             * We need to go to an xml file and find the latest version of the PQS
             * the XML or text file will contain a number i.e. V1.0.0.0
             * and this will tell the launcher with executaable to execute.
             * 
             * The arguments are passed in from here.
             * 
             * 
             * 
             */ 

            
        }
    }
}
