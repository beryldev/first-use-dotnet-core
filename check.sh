#!/bin/bash

pushd ./src;
dotnet restore;
dotnet build;
dotnet test ./Wrhs.Tests/Wrhs.Tests.csproj;
dotnet test ./Wrhs.Data.Tests/Wrhs.Data.Tests.csproj;
dotnet test ./Wrhs.WebApp.Tests/Wrhs.WebApp.Tests.csproj;
popd;
