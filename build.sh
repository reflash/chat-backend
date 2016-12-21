#!/usr/bin/env bash

function npm_package_is_installed {
  # set to 1 initially
  local return_=1
  # set to 0 if not found
  npm list -g | grep $1 >/dev/null 2>&1 || { local return_=0; }
  # return value
  return "$return_"
}

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

PKG_OK=$(dpkg-query -W --showformat='${Status}\n' nodejs|grep "install ok installed")
echo Checking for nodejs: $PKG_OK
if [ "" == "$PKG_OK" ]; then
  echo "No nodejs. Setting up nodejs."
  sudo apt-get --force-yes --yes install nodejs
fi

npm_package_is_installed elm@0.18
installed=$?

if [ $installed -ne 1 ]; then
  sudo npm install -g elm@0.18
fi


[ ! -e build.fsx ] && mono .paket/paket.exe update
[ ! -e build.fsx ] && mono packages/FAKE/tools/FAKE.exe init.fsx
mono packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
