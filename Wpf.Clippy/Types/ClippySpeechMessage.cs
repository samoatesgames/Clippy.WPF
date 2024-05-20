using System;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Clippy.Types
{
    internal class ClippySpeechMessage : ClippyMessage
    {
        private readonly TimeSpan m_dismissAfter;
        private readonly DateTime m_startTime;

        public ClippySpeechMessage(string message, TimeSpan dismissAfter)
        {
            m_dismissAfter = dismissAfter;
            m_startTime = DateTime.Now;
            Content = new TextBlock
            {
                Text = message
            };
        }

        public override FrameworkElement Content { get; }

        public override bool ShouldDismiss => DateTime.Now - m_startTime >= m_dismissAfter;
    }
}
