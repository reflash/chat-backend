#load "init.fsx"
#r @"packages\FAKE\tools\FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile

RunTargetOrDefault "Zip"
