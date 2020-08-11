using AverageDoseInSensitiveVolume.ViewModels;
using System.Linq;
using System.Windows;
using VolumeAverage;

namespace AverageDoseInSensitiveVolume
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var mainWindowViewModel = new MainWindowViewModel();

            // Add PinPoint 3D
            mainWindowViewModel.Cylinders.Add(new Cylinder("PinPoint 3D", 1.45, 2.9));
            mainWindowViewModel.Cylinders.Add(new Cylinder("Semiflex 3D", 2.4, 4.8));
            mainWindowViewModel.Cylinders.Add(new Cylinder("Semiflex", 2.75, 6.5));
            mainWindowViewModel.Cylinders.Add(new Cylinder("Farmer", 3.05, 23)); ;

            mainWindowViewModel.FieldReferencePoints.Add(new Models.FieldReferencePoint(0, 1, 2, "Ref1"));
            mainWindowViewModel.FieldReferencePoints.Add(new Models.FieldReferencePoint(3, 4, 5, "Ref2"));
            if (mainWindowViewModel.FieldReferencePoints.Count > 0)
            {
                mainWindowViewModel.SelectedFieldReferencePoint = mainWindowViewModel.FieldReferencePoints.First();
            }
            DataContext = mainWindowViewModel;

        }

        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
