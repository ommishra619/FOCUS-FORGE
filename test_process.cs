using System;
using System.Diagnostics;
using System.Linq;

namespace Test;

class Program
{
    static void Main(string[] args)
    {
        var processName = "chrome";
        var normalizedName = System.IO.Path.GetFileNameWithoutExtension(processName);
        var currentId = Process.GetCurrentProcess().Id;
        
        var matches = Process.GetProcesses()
            .Where(p => p.Id != currentId && 
                        p.ProcessName.Contains(normalizedName, StringComparison.OrdinalIgnoreCase))
            .ToList();
            
        Console.WriteLine($"Found {matches.Count} matches for '{processName}'.");
        foreach(var m in matches)
        {
            Console.WriteLine($" - {m.ProcessName} ({m.Id})");
        }
    }
}
