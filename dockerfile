FROM microsoft/dotnet:latest
RUN curl -sL https://deb.nodesource.com/setup_6.x | bash -
RUN apt-get install -y nodejs
RUN git clone https://github.com/beryldev/first-use-dotnet-core.git
RUN cd /first-use-dotnet-core/src && dotnet restore
RUN cd /first-use-dotnet-core/src/Wrhs.Data && dotnet build && dotnet ef database update
RUN cd /first-use-dotnet-core/src/Wrhs.WebApp && dotnet build
RUN cd /first-use-dotnet-core/src && cp Wrhs.Data/bin/Debug/netcoreapp1.0/wrhs.db Wrhs.WebApp/bin/Debug/netcoreapp1.0/wrhs.db
RUN cd /first-use-dotnet-core/src/Wrhs.WebApp && npm install \
    && node_modules/bower/bin/bower install --allow-root && node_modules/gulp/bin/gulp.js all
ENV ASPNETCORE_URLS http://+:80
EXPOSE 5000/tcp
EXPOSE 80/tcp
ENTRYPOINT cd /first-use-dotnet-core/src/Wrhs.WebApp && dotnet run
