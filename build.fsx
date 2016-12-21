#load "init.fsx"
#r @"FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile

RunTargetOrDefault "Copy"
