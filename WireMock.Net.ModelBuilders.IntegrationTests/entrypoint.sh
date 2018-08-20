#!/bin/bash

./wait && exec dotnet vstest WireMock.Net.ModelBuilders.IntegrationTests.dll --logger:trx\;LogFileName=/output/integration-test-results.trx