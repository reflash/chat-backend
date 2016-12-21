#load "init.fsx"

open Fake
open Fake.AssemblyInfoFile

#r @"build/Chat_Backend.dll"
open ChatBackend

Target "Run" (fun _ ->
    App.start ()
)

RunTargetOrDefault "Run"
