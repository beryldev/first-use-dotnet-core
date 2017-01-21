#!/bin/bash
dotnet ef database drop;
dotnet ef migrations remove;
dotnet ef migrations add initial;
dotnet ef database update;
cp bin/Debug/netcoreapp1.1/wrhs.db ../Wrhs.WebApp/bin/Debug/netcoreapp1.1/wrhs.db
