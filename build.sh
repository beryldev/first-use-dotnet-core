#!/bin/bash

result=0;
pushd src;
dotnet restore;
if [ $? -ne "0" ]; then result=1; fi

dotnet build;
if [ $? -ne "0" ]; then result=1; fi

popd;

exit $result;



