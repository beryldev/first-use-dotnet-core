#!/bin/bash

cd ./src/Wrhs;
dotnet restore;
cd ../Wrhs.Tests;
dotnet restore;
dotnet test;
cd ../Wrhs.Data;
dotnet restore;
cd ../Wrhs.Data.Tests;
dotnet restore;
dotnet test;
cd ../Wrhs.WebApp;
dotnet restore;
cd ../Wrhs.WebApp.Tests;
dotnet restore;
dotnet test;
cd ../../;
echo "Done.";
