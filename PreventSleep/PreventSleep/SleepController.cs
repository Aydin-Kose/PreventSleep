using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PreventSleep
{
    static class SleepController
    {
        [Flags]
        enum ExecutionStates : uint
        {
            Continuous = 0x80000000,
            AwaymodeRequired = 0x00000040,
            UserPresent = 0x00000004,
            DisplayRequired = 0x00000002,
            SystemRequired = 0x00000001
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern ExecutionStates SetThreadExecutionState(ExecutionStates executionStateFlag);

        internal static void Activate()
        {
            SetThreadExecutionState(ExecutionStates.DisplayRequired | ExecutionStates.Continuous);
        }

        internal static void Deactivate()
        {
            SetThreadExecutionState(ExecutionStates.Continuous);
        }
    }
}