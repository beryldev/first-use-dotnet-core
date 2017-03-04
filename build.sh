#!/bin/bash

cd ./src/Wrhs.WebApp;
dotnet restore;
dotnet build;
cd ../../;
echo "Done...";



