Remove-Item -Path ".\TestResults" -Recurse -Force
Remove-Item -Path ".\coverageReport" -Recurse -Force
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:TestResults/**/coverage.cobertura.xml -targetdir:coverageReport
Start-Process ./coverageReport/index.html