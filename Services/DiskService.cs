using System;
using System.IO;
using System.Linq;
using System.ComponentModel;

namespace WindowsSystemWidget.Services
{
    public class DiskStats
    {
        public string DriveName { get; set; } = "";
        public ulong Total { get; set; }
        public ulong Used { get; set; }
        public ulong Free { get; set; }
        public double UsedPercentage { get; set; }

        public string TotalFormatted => Formatters.FormatBytes(Total);
        public string UsedFormatted => Formatters.FormatBytes(Used);
        public string FreeFormatted => Formatters.FormatBytes(Free);
    }

    public class DiskService : INotifyPropertyChanged
    {
        private static DiskService? _instance;
        public static DiskService Instance => _instance ??= new DiskService();

        private DiskStats? _currentStats;

        public DiskStats? CurrentStats
        {
            get => _currentStats;
            private set
            {
                _currentStats = value;
                OnPropertyChanged(nameof(CurrentStats));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private DiskService()
        {
            Refresh();
        }

        public void Refresh()
        {
            CurrentStats = GetDiskStats();
        }

        public DiskStats? GetDiskStats()
        {
            try
            {
                // Ottieni il disco di sistema (C:)
                var systemDrive = DriveInfo.GetDrives()
                    .FirstOrDefault(d => d.IsReady && d.Name.StartsWith("C"));

                if (systemDrive != null)
                {
                    var total = (ulong)systemDrive.TotalSize;
                    var free = (ulong)systemDrive.AvailableFreeSpace;
                    var used = total - free;

                    return new DiskStats
                    {
                        DriveName = systemDrive.Name,
                        Total = total,
                        Used = used,
                        Free = free,
                        UsedPercentage = (double)used / total * 100
                    };
                }
            }
            catch { }

            return null;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

