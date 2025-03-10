docker compose up -d

do {
  Write-Host "waiting..."
  sleep 3      
} until(Test-NetConnection localhost -Port 3000 | ? { $_.TcpTestSucceeded } )

Start-Process "http://localhost:3000"
