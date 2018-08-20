FROM microsoft/dotnet:2.1.302-sdk

WORKDIR /app

RUN curl -L -o /app/wait https://github.com/ufoscout/docker-compose-wait/releases/download/2.3.0/wait \
    && chmod +x /app/wait

COPY artifacts /app

ENV DOCKER=true
RUN chmod +x entrypoint.sh

ENTRYPOINT [ "./entrypoint.sh" ]