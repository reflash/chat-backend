// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.Testing.NUnit3

RestorePackages()

// Directories
let buildDir  = @".\build\"
let testDir   = @".\test\"
let deployDir = @".\deploy\"
let packagesDir = @".\packages"

// version info
let version = "0.2"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testDir; deployDir]
)

Target "SetVersions" (fun _ ->
(*    CreateCSharpAssemblyInfo "./src/app/chat-backend/Properties/AssemblyInfo.cs"
        [Attribute.Title "Chat-Backend"
         Attribute.Description "A backend project for my chat fun"
         Attribute.Guid "A539B42C-CB9F-4a23-8E57-AF4E7CEE5BAA"
         Attribute.Product "Chat-Backend"
         Attribute.Version version
         Attribute.FileVersion version] *)

    CreateCSharpAssemblyInfo "./src/app/chat-backend/Properties/AssemblyInfo.cs"
        [Attribute.Title "Chat-Backend"
         Attribute.Description "A backend project for my chat fun"
         Attribute.Guid "EE5621DB-B86B-44eb-987F-9C94BCC98441"
         Attribute.Product "Chat"
         Attribute.Version version
         Attribute.FileVersion version]
)

Target "CompileApp" (fun _ ->
    !! @"src\app\**\*.fsproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Target "CompileTest" (fun _ ->
    !! @"src\test\**\*.fsproj"
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


Target "Zip" (fun _ ->
    !! (buildDir + "\**\*.*")
        -- "*.zip"
        |> Zip buildDir (deployDir + "chat-backend." + version + ".zip")
)

// Dependencies
"Clean"
  ==> "SetVersions"
  ==> "CompileApp"
  ==> "CompileTest"
  ==> "NUnitTest"
  ==> "Zip"

// start build
RunTargetOrDefault "Zip"
