using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LatishSehgal.KillCassini
{
    internal class CassiniUtil
    {
        public static List<ProcessInfo> KillAllCassiniInstances()
        {
            var processNames = new[] { "WebDev.WebServer", "WebDev.WebServer20", "WebDev.WebServer40" };

            var processesKilled = new List<ProcessInfo>();
            foreach (var runningProcess in processNames.Select(Process.GetProcessesByName).SelectMany(p => p))
            {
                var processInfo = new ProcessInfo
                                      {
                                          Handle = runningProcess.Handle,
                                          Id = runningProcess.Id,
                                          Name = runningProcess.ProcessName
                                      };
                runningProcess.Kill();
                processesKilled.Add(processInfo);
            }
            return processesKilled;
        }
    }
}