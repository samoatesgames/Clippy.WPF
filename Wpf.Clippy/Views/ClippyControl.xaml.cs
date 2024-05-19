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

            m_viewModel.SetSpeechPopup(SpeechPopup);

            Closing += OnClosing;
            LocationChanged += ClippyControl_LocationChanged;

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

        private void ClippyControl_LocationChanged(object sender, System.EventArgs e)
        {
            var offset = SpeechPopup.HorizontalOffset;
            SpeechPopup.HorizontalOffset = offset + 1;
            SpeechPopup.HorizontalOffset = offset;
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
