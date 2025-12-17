using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Timers;
using WindowsSystemWidget.Services;

namespace WindowsSystemWidget.Views
{
    public partial class MainWindow : Window
    {
        private readonly MemoryService _memoryService;
        private readonly DiskService _diskService;
        private readonly ProcessService _processService;
        private Timer? _updateTimer;

        public MainWindow()
        {
            InitializeComponent();

            _memoryService = MemoryService.Instance;
            _diskService = DiskService.Instance;
            _processService = ProcessService.Instance;

            // Mostra la finestra all'avvio
            Show();

            // Avvia aggiornamenti UI
            StartUIUpdates();
            UpdateUI();
        }

        private void StartUIUpdates()
        {
            _updateTimer = new Timer(2000);
            _updateTimer.Elapsed += (s, e) => Dispatcher.Invoke(UpdateUI);
            _updateTimer.Start();
        }

        private void UpdateUI()
        {
            // Memory
            var memStats = _memoryService.CurrentStats;
            MemoryUsedText.Text = $"{memStats.UsedFormatted} / {memStats.TotalFormatted}";
            MemoryPercentText.Text = $"{memStats.UsedPercentage:0}%";
            MemoryProgressBar.Value = memStats.UsedPercentage;
            MemoryProgressBar.Maximum = 100;

            var (statusText, statusColor) = memStats.Status switch
            {
                MemoryStatus.Ok => ("Stato: OK ✓", Brushes.LimeGreen),
                MemoryStatus.Warning => ("Stato: Attenzione ⚠", Brushes.Orange),
                MemoryStatus.Critical => ("Stato: Critico ⚠", Brushes.Red),
                _ => ("Stato: Sconosciuto", Brushes.Gray)
            };
            MemoryStatusText.Text = statusText;
            MemoryStatusText.Foreground = statusColor;

            // Disk
            var diskStats = _diskService.CurrentStats;
            if (diskStats != null)
            {
                DiskUsedText.Text = $"{diskStats.UsedFormatted} / {diskStats.TotalFormatted}";
                DiskPercentText.Text = $"{diskStats.UsedPercentage:0}%";
                DiskProgressBar.Value = diskStats.UsedPercentage;
                DiskProgressBar.Maximum = 100;
                DiskFreeText.Text = $"Libero: {diskStats.FreeFormatted}";
            }

            // Processes
            ProcessList.ItemsSource = _processService.TopProcesses.Take(5).ToList();

            // Update tray tooltip
            TrayIcon.ToolTipText = $"RAM: {memStats.UsedPercentage:0}%";
        }

        private void TrayIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            ToggleWindow();
        }

        private void ToggleWindow()
        {
            if (IsVisible)
            {
                Hide();
            }
            else
            {
                // Posiziona vicino alla tray
                var workArea = SystemParameters.WorkArea;
                Left = workArea.Right - Width - 10;
                Top = workArea.Bottom - Height - 10;
                
                Show();
                Activate();
            }
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ToggleWindow();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _memoryService.Refresh();
            _diskService.Refresh();
            _processService.Refresh();
            UpdateUI();
        }

        private void FreeMemoryButton_Click(object sender, RoutedEventArgs e)
        {
            var success = _memoryService.FreeMemory(out string message);
            MessageBox.Show(message, success ? "Completato" : "Attenzione", 
                MessageBoxButton.OK, success ? MessageBoxImage.Information : MessageBoxImage.Warning);
            UpdateUI();
        }

        private void TaskManagerButton_Click(object sender, RoutedEventArgs e)
        {
            _processService.OpenTaskManager();
        }

        private void KillHeavyButton_Click(object sender, RoutedEventArgs e)
        {
            var (count, message) = _processService.KillHeavyUserProcesses();
            MessageBox.Show(message, count > 0 ? "Completato" : "Info", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            UpdateUI();
        }

        private void KillProcessButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is ProcessInfo proc)
            {
                var result = MessageBox.Show($"Terminare il processo {proc.Name}?", 
                    "Conferma", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    var success = _processService.KillProcess(proc, out string message);
                    MessageBox.Show(message, success ? "Completato" : "Errore",
                        MessageBoxButton.OK, success ? MessageBoxImage.Information : MessageBoxImage.Error);
                    UpdateUI();
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitApplication();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ExitApplication();
        }

        private void ExitApplication()
        {
            TrayIcon.Dispose();
            Application.Current.Shutdown();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // Nascondi invece di chiudere
            e.Cancel = true;
            Hide();
        }
    }
}

