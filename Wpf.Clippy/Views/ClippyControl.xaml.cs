using System.ComponentModel;
using System.Windows;
using Wpf.Clippy.ViewModels;

namespace Wpf.Clippy.Views
{
    public partial class ClippyControl
    {
        private readonly ClippyViewModel m_viewModel;

        public delegate void ClippyControlEventHandler(ClippyControl sender, ClippyViewModel viewModel);
        public event ClippyControlEventHandler OnDoubleClick;

        public ClippyControl(ClippyViewModel viewModel)
        {
            InitializeComponent();
            DataContext = m_viewModel = viewModel;

            Closing += OnClosing;
            Canvas.PreviewMouseLeftButtonDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    OnDoubleClick?.Invoke(this, m_viewModel);
                    return;
                }
                DragMove();
            };

            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Closing += MainWindowOnClosing;
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            m_viewModel.Close();
        }

        private void MainWindowOnClosing(object sender, CancelEventArgs e)
        {
            Close();
        }
    }
}
