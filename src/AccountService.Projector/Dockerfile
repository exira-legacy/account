FROM mcr.microsoft.com/dotnet/core/runtime-deps:2.2.4

LABEL maintainer "exira.com <info@exira.com>"

COPY / /app
WORKDIR /app

RUN apt-get update && \
    apt-get install curl jq -y && \
    chmod +x ./init.sh

EXPOSE 3001/tcp
ENV ASPNETCORE_URLS http://*:3001

ENTRYPOINT ["./init.sh"]
