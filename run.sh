#!/usr/bin/env bash

mono packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO run.fsx
