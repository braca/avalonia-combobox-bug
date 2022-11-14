using Avalonia.Controls;

namespace AvaloniaComboBoxBug
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new SampleViewModel();
        }
    }
}