using System;

namespace Wpf.Clippy.Types
{
    internal class ClippySpeechMessage : ClippyMessage
    {
        private readonly TimeSpan m_dismissAfter;
        private readonly DateTime m_startTime;

        public ClippySpeechMessage(string message, TimeSpan dismissAfter)
        {
            Message = message;
            m_dismissAfter = dismissAfter;
            m_startTime = DateTime.Now;
        }

        public override string Message { get; }

        public override bool ShouldDismiss => DateTime.Now - m_startTime >= m_dismissAfter;
    }
}
