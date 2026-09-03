using System.Diagnostics;

namespace FocusForge.Infrastructure;

public sealed class ProcessBlocker
{
    public IReadOnlyList<Process> FindRunning(string processName)
    {
        var normalizedName = Path.GetFileNameWithoutExtension(processName);
        return Process.GetProcessesByName(normalizedName);
    }

    public int CloseRunning(string processName)
    {
        var closed = 0;
        foreach (var process in FindRunning(processName))
        {
            try
            {
                if (!process.HasExited)
                {
                    process.CloseMainWindow();
                    if (!process.WaitForExit(1500) && !process.HasExited)
                    {
                        process.Kill(entireProcessTree: true);
                    }

                    closed++;
                }
            }
            catch (InvalidOperationException)
            {
                // The process exited between discovery and termination.
            }
            finally
            {
                process.Dispose();
            }
        }

        return closed;
    }
}
