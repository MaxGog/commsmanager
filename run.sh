#!/bin/bash
set -e
echo "🚀 CommsManager is started..."

dotnet --version | grep -q "10" || { echo "❌ Install .NET 10 SDK"; exit 1; }

dotnet restore CommsManager.slnx
dotnet build CommsManager.slnx --no-restore -c Debug

dotnet ef database update --project CommsManager.Infrastructure --startup-project CommsManager.API || echo "⚠️ Migrations skipped"

gnome-terminal -- bash -c "dotnet run --project CommsManager.API --no-build --urls=http://localhost:5000; exec bash" &
gnome-terminal -- bash -c "dotnet run --project CommsManager.Web --no-build --urls=http://localhost:5001; exec bash" &

echo "✅ Готово! API: http://localhost:5000, Web: http://localhost:5001"
