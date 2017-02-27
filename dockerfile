FROM microsoft/dotnet

RUN curl -sL https://deb.nodesource.com/setup_6.x | bash -
RUN apt-get install -y nodejs
RUN git clone https://github.com/beryldev/first-use-dotnet-core.git /opt/app

WORKDIR /opt/app
RUN ./build.sh

WORKDIR /opt/app/src/Wrhs.WebApp/
RUN npm install -g bower gulp
RUN npm install
RUN bower install --allow-root
RUN gulp all

ENV ASPNETCORE_URLS http://+:80

EXPOSE 5000/tcp
EXPOSE 80/tcp

ENTRYPOINT dotnet run
