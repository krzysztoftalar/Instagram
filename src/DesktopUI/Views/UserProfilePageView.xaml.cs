using System.Windows;
using System.Windows.Controls;

namespace DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for UserProfilePage.xaml
    /// </summary>
    public partial class UserProfilePage : UserControl
    {
        public UserProfilePage()
        {
            InitializeComponent();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }
    }
}
