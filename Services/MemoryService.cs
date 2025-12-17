using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.ComponentModel;

namespace WindowsSystemWidget.Services
{
    public enum MemoryStatus
    {
        Ok,
        Warning,
        Critical
    }

    public class MemoryStats
    {
        public ulong Total { get; set; }
        public ulong Used { get; set; }
        public ulong Free { get; set; }
        public double UsedPercentage { get; set; }
        public MemoryStatus Status { get; set; }

        public string TotalFormatted => Formatters.FormatBytes(Total);
        public string UsedFormatted => Formatters.FormatBytes(Used);
        public string FreeFormatted => Formatters.FormatBytes(Free);
    }

    public class MemoryService : INotifyPropertyChanged
    {
        private static MemoryService? _instance;
        public static MemoryService Instance => _instance ??= new MemoryService();

        private MemoryStats _currentStats = new();
        private Timer? _timer;

        public MemoryStats CurrentStats
        {
            get => _currentStats;
            private set
            {
                _currentStats = value;
                OnPropertyChanged(nameof(CurrentStats));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private MemoryService()
        {
            Refresh();
            StartMonitoring();
        }

        public void StartMonitoring(double intervalMs = 2000)
        {
            _timer?.Stop();
            _timer = new Timer(intervalMs);
            _timer.Elapsed += (s, e) => Refresh();
            _timer.Start();
        }

        public void StopMonitoring()
        {
            _timer?.Stop();
        }

        public void Refresh()
        {
            CurrentStats = GetMemoryStats();
        }

        public MemoryStats GetMemoryStats()
        {
            var memStatus = new MEMORYSTATUSEX();
            memStatus.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));

            if (GlobalMemoryStatusEx(ref memStatus))
            {
                var total = memStatus.ullTotalPhys;
                var free = memStatus.ullAvailPhys;
                var used = total - free;
                var usedPercentage = (double)used / total * 100;

                var freePercentage = (double)free / total * 100;
                var freeGB = (double)free / 1_073_741_824;

                MemoryStatus status;
                if (freeGB < 1.5 || freePercentage < 15)
                    status = MemoryStatus.Critical;
                else if (freePercentage < 30)
                    status = MemoryStatus.Warning;
                else
                    status = MemoryStatus.Ok;

                return new MemoryStats
                {
                    Total = total,
                    Used = used,
                    Free = free,
                    UsedPercentage = usedPercentage,
                    Status = status
                };
            }

            return new MemoryStats();
        }

        public bool FreeMemory(out string message)
        {
            try
            {
                // Windows: Svuota working set dei processi
                EmptyWorkingSet(System.Diagnostics.Process.GetCurrentProcess().Handle);
                
                // Forza garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                Refresh();
                message = "Memoria ottimizzata con successo!";
                return true;
            }
            catch (Exception ex)
            {
                message = $"Errore: {ex.Message}";
                return false;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

        [DllImport("psapi.dll")]
        private static extern bool EmptyWorkingSet(IntPtr hProcess);

        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }
    }
}

