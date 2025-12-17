using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ComponentModel;
using System.Timers;

namespace WindowsSystemWidget.Services
{
    public class ProcessInfo
    {
        public int Pid { get; set; }
        public string Name { get; set; } = "";
        public long MemoryBytes { get; set; }
        public string MemoryFormatted => Formatters.FormatBytes((ulong)MemoryBytes);
    }

    public class ProcessService : INotifyPropertyChanged
    {
        private static ProcessService? _instance;
        public static ProcessService Instance => _instance ??= new ProcessService();

        private List<ProcessInfo> _topProcesses = new();
        private Timer? _timer;

        public List<ProcessInfo> TopProcesses
        {
            get => _topProcesses;
            private set
            {
                _topProcesses = value;
                OnPropertyChanged(nameof(TopProcesses));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private ProcessService()
        {
            Refresh();
            StartMonitoring();
        }

        public void StartMonitoring(double intervalMs = 5000)
        {
            _timer?.Stop();
            _timer = new Timer(intervalMs);
            _timer.Elapsed += (s, e) => Refresh();
            _timer.Start();
        }

        public void Refresh()
        {
            try
            {
                var processes = Process.GetProcesses()
                    .Where(p => !string.IsNullOrEmpty(p.ProcessName))
                    .Select(p =>
                    {
                        try
                        {
                            return new ProcessInfo
                            {
                                Pid = p.Id,
                                Name = p.ProcessName,
                                MemoryBytes = p.WorkingSet64
                            };
                        }
                        catch
                        {
                            return null;
                        }
                    })
                    .Where(p => p != null)
                    .OrderByDescending(p => p!.MemoryBytes)
                    .Take(10)
                    .ToList();

                TopProcesses = processes!;
            }
            catch { }
        }

        public bool KillProcess(ProcessInfo processInfo, out string message)
        {
            try
            {
                var process = Process.GetProcessById(processInfo.Pid);
                process.Kill();
                message = $"Processo {processInfo.Name} terminato.";
                Refresh();
                return true;
            }
            catch (Exception ex)
            {
                message = $"Impossibile terminare il processo: {ex.Message}";
                return false;
            }
        }

        public (int count, string message) KillHeavyUserProcesses()
        {
            var heavyProcesses = TopProcesses
                .Where(p => p.MemoryBytes > 500_000_000) // > 500MB
                .Where(p => !IsSystemProcess(p.Name))
                .ToList();

            int killed = 0;
            foreach (var proc in heavyProcesses)
            {
                try
                {
                    var process = Process.GetProcessById(proc.Pid);
                    process.Kill();
                    killed++;
                }
                catch { }
            }

            Refresh();
            
            if (killed > 0)
                return (killed, $"Terminati {killed} processi pesanti.");
            else
                return (0, "Nessun processo pesante da terminare.");
        }

        private bool IsSystemProcess(string name)
        {
            var systemProcesses = new[]
            {
                "System", "svchost", "csrss", "wininit", "services",
                "lsass", "smss", "dwm", "explorer", "winlogon"
            };
            return systemProcesses.Contains(name, StringComparer.OrdinalIgnoreCase);
        }

        public void OpenTaskManager()
        {
            try
            {
                Process.Start("taskmgr.exe");
            }
            catch { }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

