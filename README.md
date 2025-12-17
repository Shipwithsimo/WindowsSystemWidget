# Windows System Widget

Widget per monitorare le risorse di sistema su Windows.

**Creato da Simone Castiglia**

## Funzionalità

- 📊 Monitoraggio RAM in tempo reale
- 💾 Statistiche disco
- 📋 Lista processi più pesanti
- 🧹 Libera memoria RAM
- ⚠️ Chiudi app pesanti
- 🚀 Avvio automatico (opzionale)

## Requisiti

- Windows 10/11
- .NET 6.0 Runtime

## Come compilare

### Con Visual Studio 2022

1. Apri `WindowsSystemWidget.csproj`
2. Premi `F5` per compilare ed eseguire

### Da riga di comando

```bash
# Installa .NET SDK 6.0+ da https://dotnet.microsoft.com/download

# Compila
dotnet build

# Esegui
dotnet run

# Pubblica (crea .exe standalone)
dotnet publish -c Release -r win-x64 --self-contained true
```

## Struttura progetto

```
WindowsSystemWidget/
├── App.xaml              # Configurazione app e risorse
├── App.xaml.cs           # Logica avvio e auto-launch
├── Services/
│   ├── MemoryService.cs  # Statistiche RAM
│   ├── DiskService.cs    # Statistiche disco
│   ├── ProcessService.cs # Gestione processi
│   └── Formatters.cs     # Utilità formattazione
└── Views/
    ├── MainWindow.xaml   # Interfaccia utente
    └── MainWindow.xaml.cs
```

## Icona

Per aggiungere un'icona personalizzata:
1. Salva l'icona come `icon.ico` nella cartella del progetto
2. Ricompila l'app

## Note

- L'app si minimizza nella System Tray (area notifiche)
- Clicca sull'icona per aprire/chiudere il widget
- Tasto destro per menu contestuale

