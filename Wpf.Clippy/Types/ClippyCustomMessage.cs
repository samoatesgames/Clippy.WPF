using System;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Clippy.Types
{
    internal class ClippyCustomMessage : ClippyMessage
    {
        private readonly TimeSpan? m_dismissAfter;
        private readonly DateTime m_startTime;

        public ClippyCustomMessage(FrameworkElement content, TimeSpan? dismissAfter)
        {
            m_dismissAfter = dismissAfter;
            m_startTime = DateTime.Now;
            Content = content;
        }

        public override FrameworkElement Content { get; }

        public override bool ShouldDismiss
        {
            get
            {
                if (m_dismissAfter == null)
                {
                    return false;
                }

                return DateTime.Now - m_startTime >= m_dismissAfter;
            }
        }
    }
}