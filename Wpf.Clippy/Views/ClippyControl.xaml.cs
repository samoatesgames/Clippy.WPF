using System.ComponentModel;
using System.Windows;
using Wpf.Clippy.Types;
using Wpf.Clippy.ViewModels;

namespace Wpf.Clippy.Views
{
    public partial class ClippyControl
    {
        private readonly ClippyViewModel m_viewModel;

        public ClippyControl(ClippyViewModel viewModel)
        {
            InitializeComponent();
            DataContext = m_viewModel = viewModel;

            Closing += OnClosing;
            Canvas.PreviewMouseLeftButtonDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    if (!viewModel.PlayAnimation("Wave", AnimationMode.Once))
                    {
                        viewModel.PlayAnimation("Pleased", AnimationMode.Once);
                    }
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
