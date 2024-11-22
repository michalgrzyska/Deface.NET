dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:TestResults/**/coverage.cobertura.xml -targetdir:coverageReport
Start-Process ./coverageReport/index.html