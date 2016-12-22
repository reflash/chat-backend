#!/usr/bin/env bash

mono .paket/paket.bootstrapper.exe
exit_code=$?
if [ $exit_code -ne 0 ]; then
  exit $exit_code
fi

mono .paket/paket.exe restore
exit_code=$?
if [ $exit_code -ne 0 ]; then
  exit $exit_code
fi

command -v npm >/dev/null 2>&1 || { sudo apt-get --force-yes --yes install nodejs || brew install node; }

npm list -g | grep elm@0.18 >/dev/null 2>&1

if [ $? -ne 0 ]; then
  sudo npm install -g elm@0.18
fi


[ ! -e build.fsx ] && mono .paket/paket.exe update
[ ! -e build.fsx ] && mono packages/FAKE/tools/FAKE.exe init.fsx
mono packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
