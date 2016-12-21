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

// Package to docker container and run
//  - docker build -t reflash/chat .
//  - docker run -d -p 127.0.0.1:80:4567 reflash/chat /bin/sh -c "cd /root/chat; bundle exec foreman start;"
//  - docker ps -a
//  - docker run reflash/chat /bin/sh -c "cd /root/chat; bundle exec rake test"

Target "Zip" (fun _ ->
    !! (buildDir + "\**\*.*")
        -- "*.zip"
        |> Zip buildDir (deployDir + "chat-backend." + version + ".zip")
)


// Build and pack
"Clean"
  ==> "SetVersions"
  ==> "CompileApp"
  ==> "CompileTest"
  ==> "NUnitTest"
  ==> "Zip"
