using System.Diagnostics;

namespace FocusForge.Infrastructure;

public sealed class ProcessBlocker
{
    public IReadOnlyList<Process> FindRunning(string processName)
    {
        var normalizedName = Path.GetFileNameWithoutExtension(processName).Trim();
        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            return Array.Empty<Process>();
        }

        var currentId = Process.GetCurrentProcess().Id;
        
        return Process.GetProcesses()
            .Where(p => p.Id != currentId && 
                        string.Equals(p.ProcessName, normalizedName, StringComparison.OrdinalIgnoreCase))
            .ToList();
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
                    if (!process.WaitForExit(1000) && !process.HasExited)
                    {
                        process.Kill(entireProcessTree: true);
                        process.WaitForExit(1000);
                    }

                    if (process.HasExited)
                    {
                        closed++;
                    }
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // Access denied or process requires elevation. Ignore.
            }
            catch (UnauthorizedAccessException)
            {
                // Access denied. Ignore.
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
