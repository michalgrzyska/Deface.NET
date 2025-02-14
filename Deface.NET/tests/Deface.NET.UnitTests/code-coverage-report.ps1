dotnet tool install -g dotnet-reportgenerator-globaltool

if (Test-Path -Path ".\TestResults") {
    Remove-Item -Path ".\TestResults" -Recurse -Force
}

if (Test-Path -Path ".\coverageReport") {
    Remove-Item -Path ".\coverageReport" -Recurse -Force
}

dotnet test --collect:"XPlat Code Coverage"
& "$env:USERPROFILE\.dotnet\tools\reportgenerator" -reports:TestResults/**/coverage.cobertura.xml -targetdir:./coverageReport -reporttypes:Html

if (Test-Path -Path "./coverageReport/index.html") {
    Start-Process "./coverageReport/index.html"
} else {
    Write-Host "Coverage report not found. Please check the reportgenerator command."
}
