using System.Windows;

namespace Wpf.Clippy.Types
{
    public abstract class ClippyMessage
    {
        public abstract FrameworkElement Content { get; }
        public abstract bool ShouldDismiss { get; }
    }
}
