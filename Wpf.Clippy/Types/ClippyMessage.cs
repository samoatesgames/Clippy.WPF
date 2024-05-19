namespace Wpf.Clippy.Types
{
    internal abstract class ClippyMessage
    {
        public abstract string Message { get; }
        public abstract bool ShouldDismiss { get; }
    }
}
