#!/bin/bash
fakepath=packages/build/fake-cli/tools/netcoreapp2.1/any/fake-cli.dll

if test "$OS" = "Windows_NT"; then
  .paket/paket.exe restore || exit $?
else
  mono .paket/paket.exe restore || exit $?
fi

dotnet ${fakepath} run build.fsx "$@"