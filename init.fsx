// include Fake lib
#r @"./packages/FAKE/tools/FakeLib.dll"


open Fake
open Fake.AssemblyInfoFile
open Fake.Testing.NUnit3

open System

RestorePackages()

// Directories
let buildDir  = @"./build/"
let testDir   = @"./test/"
let deployDir = @"./deploy/"
let packagesDir = @"./packages"

let elmDir = @"./src/"

// version info
let version = "0.2"  // or retrieve from CI server

let appReferences = !! "/**/*.fsproj"
let dotnetcliVersion = "2.0.0"
let mutable dotnetExePath = "dotnet"

// Helpers
let run' timeout cmd args dir =
    if execProcess (fun info ->
        info.FileName <- cmd
        if not (String.IsNullOrWhiteSpace dir) then
            info.WorkingDirectory <- dir
        info.Arguments <- args
    ) timeout |> not then
        failwithf "Error while running '%s' with args: %s" cmd args

let run = run' System.TimeSpan.MaxValue

let runDotnet workingDir args =
    let result =
        ExecProcess (fun info ->
            info.FileName <- dotnetExePath
            info.WorkingDirectory <- workingDir
            info.Arguments <- args) TimeSpan.MaxValue
    if result <> 0 then failwithf "dotnet %s failed" args

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testDir; deployDir]
)

Target "SetVersions" (fun _ ->
    CreateCSharpAssemblyInfo "./src/app/chat-backend/Properties/AssemblyInfo.cs"
        [Attribute.Title "Chat-Backend"
         Attribute.Description "A backend project for my chat fun"
         Attribute.Guid "EE5621DB-B86B-44eb-987F-9C94BCC98441"
         Attribute.Product "Chat"
         Attribute.Version version
         Attribute.FileVersion version]
)

Target "InstallDotNetCLI" (fun _ ->
    dotnetExePath <- DotNetCli.InstallDotNetSDK dotnetcliVersion
)

Target "Restore" (fun _ ->
    appReferences
    |> Seq.iter (fun p ->
        let dir = System.IO.Path.GetDirectoryName p
        runDotnet dir "restore"
    )
)

Target "CompileApp" (fun _ ->
    !! @"src\app\**\*.fsproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Target "CompileTest" (fun _ ->
    !! @"src\tests\**\*.fsproj"
      |> MSBuildDebug testDir "Build"
      |> Log "TestBuild-Output: "
)

Target "NUnitTest" (fun _ ->
    !! (testDir + @"\*_test.dll")
      |> NUnit3 (fun p ->
                 {p with
                   ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe";
                   OutputDir  = testDir + "TestResults.xml" })
)

Target "CompileElm" (fun _ ->
    Shell.Exec("elm-make", "src/Main.elm --output build/Main.js --yes", "src/frontend")
    |> ignore
)

Target "Copy" (fun _ ->
    !! (buildDir + "\**\*.*")
        -- "*.zip"
        |> Zip buildDir (deployDir + "chat-backend." + version + ".zip")
)



// Build and pack
"Clean"
  ==> "SetVersions"
  ==> "InstallDotNetCLI"
  ==> "Restore"
  ==> "CompileApp"
  ==> "CompileTest"
  ==> "NUnitTest"
  ==> "CompileElm"
  ==> "Copy"
