#!/bin/bash

pushd ./src/Wrhs;
dotnet restore;
dotnet build;
pushd ../Wrhs.Tests;
dotnet restore;
dotnet build;
dotnet test;
pushd ../Wrhs.Data;
dotnet restore;
dotnet build;
pushd ../Wrhs.Data.Tests;
dotnet restore;
dotnet build;
dotnet test;
pushd ../Wrhs.WebApp;
dotnet restore;
dotnet build;
pushd ../Wrhs.WebApp.Tests;
dotnet restore;
dotnet build;
dotnet test;
popd;
popd;
