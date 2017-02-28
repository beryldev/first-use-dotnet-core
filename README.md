# first-use-dotnet-core

First experimental project with .Net Core as simple implementation of warehouse management system.

[![Build Status](https://travis-ci.org/beryldev/first-use-dotnet-core.svg?branch=master)](https://travis-ci.org/beryldev/first-use-dotnet-core)

## Quick start
Require [Docker](https://www.docker.com)
```
$ git clone https://github.com/beryldev/first-use-dotnet-core.git
$ cd first-use-dotnet-core
$ docker build . -t wrhs
$ docker run -itp 5000:80 wrhs
```
Type in your browser http://localhost:5000