#!/bin/bash

cd ../Wrhs.WebApp;
dotnet restore;
dotnet build;
cd ../../;
echo "Done.";



