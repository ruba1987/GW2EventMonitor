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

namespace GW2EventMonitor
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();
            //Reset to correct for changes in XAML for testing
            arch.Size = new Size(100, 100);
            Close.Visibility = System.Windows.Visibility.Hidden;
            Settings.Visibility = System.Windows.Visibility.Hidden;
            Mute.Visibility = System.Windows.Visibility.Hidden;
            ViewTimers.Visibility = System.Windows.Visibility.Hidden;
        }

        private void UserControl_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Show();
        }

        private void UserControl_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Hide();

        }

        private void Hide()
        {
            arch.Size = new Size(100, 100);
            Close.Visibility = System.Windows.Visibility.Hidden;
            Settings.Visibility = System.Windows.Visibility.Hidden;
            Mute.Visibility = System.Windows.Visibility.Hidden;
            ViewTimers.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Show()
        {
            Keyboard.Focus(this);
            this.Focus();
            arch.Size = new Size(1, 1);
            Close.Visibility = System.Windows.Visibility.Visible;
            Settings.Visibility = System.Windows.Visibility.Visible;
            Mute.Visibility = System.Windows.Visibility.Visible;
            ViewTimers.Visibility = System.Windows.Visibility.Visible;
        }


    }
}
