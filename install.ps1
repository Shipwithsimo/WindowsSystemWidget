# WindowsSystemWidget - Installer
# Creato da Simone Castiglia

Write-Host "========================================"
Write-Host "  WindowsSystemWidget - Installer"
Write-Host "  Creato da Simone Castiglia"
Write-Host "========================================"
Write-Host ""

# Controlla se dotnet esiste
$dotnetExists = Get-Command dotnet -ErrorAction SilentlyContinue

if (-not $dotnetExists) {
    Write-Host "[!] .NET SDK non trovato nel sistema" -ForegroundColor Yellow
    $needsInstall = $true
} else {
    $runtimes = dotnet --list-runtimes 2>$null
    if ($runtimes -match "Microsoft.WindowsDesktop.App [678]") {
        Write-Host "[OK] .NET Desktop Runtime trovato" -ForegroundColor Green
        $needsInstall = $false
    } else {
        Write-Host "[!] .NET Desktop Runtime non trovato" -ForegroundColor Yellow
        $needsInstall = $true
    }
}

if ($needsInstall) {
    Write-Host ""
    Write-Host "Scaricamento .NET 6 Desktop Runtime in corso..."
    
    $url = "https://download.visualstudio.microsoft.com/download/pr/85473c45-8d91-48cb-ab41-86ec7abc1000/83cd0c82f0cde9a566bae4245ea5a65b/windowsdesktop-runtime-6.0.33-win-x64.exe"
    $outFile = "$env:TEMP\dotnet_runtime.exe"
    
    try {
        [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
        Invoke-WebRequest -Uri $url -OutFile $outFile -UseBasicParsing
        
        Write-Host ""
        Write-Host "Installazione .NET Runtime..."
        Write-Host "(Potrebbe richiedere permessi amministratore)"
        
        Start-Process -FilePath $outFile -ArgumentList "/install /quiet /norestart" -Wait
        
        Remove-Item $outFile -ErrorAction SilentlyContinue
        Write-Host "[OK] .NET installato!" -ForegroundColor Green
    } catch {
        Write-Host "[ERRORE] Download fallito. Apro la pagina di download..." -ForegroundColor Red
        Start-Process "https://dotnet.microsoft.com/download/dotnet/6.0"
        Write-Host "Scarica '.NET Desktop Runtime 6.0' e poi riesegui questo script."
        Read-Host "Premi INVIO per uscire"
        exit 1
    }
}

Write-Host ""
Write-Host "Avvio WindowsSystemWidget..."

$exePath = "bin\Release\net6.0-windows\WindowsSystemWidget.exe"
$exePathDebug = "bin\Debug\net6.0-windows\WindowsSystemWidget.exe"

if (Test-Path $exePath) {
    Start-Process $exePath
} elseif (Test-Path $exePathDebug) {
    Start-Process $exePathDebug
} else {
    Write-Host "Compilazione in corso..."
    dotnet build -c Release
    
    if (Test-Path $exePath) {
        Start-Process $exePath
    } else {
        Write-Host "[ERRORE] Compilazione fallita." -ForegroundColor Red
        Read-Host "Premi INVIO per uscire"
        exit 1
    }
}

Write-Host ""
Write-Host "Fatto!" -ForegroundColor Green

