using System;
using System.Windows;
using Microsoft.Win32;

namespace WindowsSystemWidget
{
    public partial class App : Application
    {
        private const string HasLaunchedBeforeKey = "HasLaunchedBefore";
        private const string AppName = "WindowsSystemWidget";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CheckFirstLaunch();
        }

        private void CheckFirstLaunch()
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + AppName, true) 
                ?? Registry.CurrentUser.CreateSubKey(@"SOFTWARE\" + AppName);
            
            var hasLaunched = key?.GetValue(HasLaunchedBeforeKey);
            
            if (hasLaunched == null)
            {
                key?.SetValue(HasLaunchedBeforeKey, true);
                ShowAutoLaunchPrompt();
            }
        }

        private void ShowAutoLaunchPrompt()
        {
            var result = MessageBox.Show(
                "Vuoi che WindowsSystemWidget si avvii automaticamente all'accensione del PC?",
                "Avvio automatico",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                EnableAutoLaunch();
            }
        }

        private void EnableAutoLaunch()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                
                var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location
                    .Replace(".dll", ".exe");
                
                key?.SetValue(AppName, exePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore nell'abilitare l'avvio automatico: {ex.Message}");
            }
        }
    }
}

