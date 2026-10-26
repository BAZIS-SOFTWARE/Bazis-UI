using System;
using System.Collections.Generic;

namespace WinGUI.DataBases
{
    public class ReacEventArgs : EventArgs
    {
        string[] times;
        public string ReactionName { get; }

        public bool IsTimeDependent { get; } = false;

        public ReacEventArgs(string [] times, string initialPhase, string finalPhase) : this(initialPhase, finalPhase)
        {
            this.times = times;
            IsTimeDependent = true;
        }

        public ReacEventArgs(string initialPhase, string finalPhase)
        {
            ReactionName = initialPhase + "-" + finalPhase;
        }

        public IEnumerable<string> GetTimes()
        {
            foreach (var time in times)
            {
                yield return time;
            }
        }
    }
}