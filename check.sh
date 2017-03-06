#!/bin/bash
error=0;
projects=(
    './Wrhs.Tests/Wrhs.Tests.csproj'
    './Wrhs.Data.Tests/Wrhs.Data.Tests.csproj'
    './Wrhs.WebApp.Tests/Wrhs.WebApp.Tests.csproj'
)

./build.sh;
if [ $? -ne 0 ]; then error=1; fi;

pushd ./src;

count=0;
while [ "x${projects[count]}" != "x" ]
do
    echo "${projects[count]}";
    dotnet test "${projects[count]}";
    if [ $? -ne 0 ]; then error=1; fi;
    count=$(( $count + 1 ))
done

popd;
exit $error;
