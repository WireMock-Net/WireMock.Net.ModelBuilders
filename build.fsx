#r "paket: groupref fake //"
#load "./.fake/build.fsx/intellisense.fsx"
#if !FAKE
  #r "Facades/netstandard"
  #r "netstandard"
#endif

open Fake
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Tools
open System

let sln                = "WireMock.Net.ModelBuilders.sln"
let nupkgsDir          = "nupkgs"
let currentDirectory   = IO.Directory.GetCurrentDirectory ()
let gitVersionPath     = Globbing.Tools.findToolInSubPath "GitVersion.exe" "packages/build/GitVersion.CommandLine/tools"

let isCi               = Environment.environVarOrNone "CI" |> Option.isSome
let gitVersion         = GitVersion.generateProperties id
let composeFile        = if not isCi then "docker-compose.override.yml" else "docker-compose.ci.yml"
let imageTag           = if not isCi then "dev" else gitVersion.SemVer
let configuration      = Environment.environVarOrDefault "CONFIG" "Debug"
let buildConfig        = DotNet.BuildConfiguration.fromString configuration
let ansicolor          = Environment.environVarAsBool "ANSICOLOR"

module Docker =
    let compose (envVars: List<string * string>) (args: string) : bool =
        Process.directExec <| fun p ->
            { p with
                WorkingDirectory = currentDirectory
                FileName = "docker-compose"
                Arguments = args }
            |> Process.setEnvironmentVariables envVars

if ansicolor then
    [ ConsoleTraceListener(
        CoreTracing.importantMessagesToStdErr,
        ConsoleWriter.colorMap,
        true) :> ITraceListener ]
    |> CoreTracing.setTraceListeners

Target.create "Clean" <| fun tp ->
    let shouldClean =
        String.Compare(tp.Context.FinalTarget, "clean", true) = 0
        || tp.Context.Arguments |> List.contains "--clean"

    let clean () =
        let r = DotNet.exec id "clean" (sprintf "%s /v:m" sln)
        if r.ExitCode <> 0 then failwithf "dotnet clean failed to run successfully"

        !! "**/bin"
        ++ "**/artifacts"
        ++ "test-results"
        ++ nupkgsDir
        |> Shell.cleanDirs

    match shouldClean with
    | true -> clean ()
    | false -> Trace.logfn "Skipping clean, no --clean argument specified"

Target.create "Restore" <| fun _ ->
    DotNet.restore id "WireMock.Net.ModelBuilders.sln"

Target.create "AssemblyInfo" <| fun _ ->
    let success = Process.directExec <| fun p ->
        { p with
            FileName = gitVersionPath
            WorkingDirectory = currentDirectory
            Arguments = "/updateassemblyinfo" }
        |> Process.withFramework

    if not success then failwithf "Updating assembly info failed with non-zero exit code"

Target.create "Build" <| fun _ ->
    let addArgIf arg cond args = if cond then arg :: args else args

    let args =
        [ "--no-restore"; "/p:TreatWarningsAsErrors=\"true\""; "/warnaserror" ]
        |> addArgIf "/consoleloggerparameters:ForceConsoleColor" ansicolor

    sln
    |> DotNet.build (fun p ->
        { p with Configuration = buildConfig; }
        |> DotNet.Options.withAdditionalArgs args)

Target.create "Artifacts:Lib" <| fun _ ->
    "WireMock.Net.ModelBuilders"
    |> DotNet.publish (fun p ->
        { p with
            Configuration = buildConfig
            OutputPath = Some "artifacts" }
        |> DotNet.Options.withAdditionalArgs [ "--no-build"; "--no-restore" ])

Target.create "Artifacts:Tests" <| fun _ ->
    "WireMock.Net.ModelBuilders.IntegrationTests"
    |> DotNet.publish (fun p ->
        { p with
            Configuration = buildConfig
            OutputPath = Some "artifacts" }
        |> DotNet.Options.withAdditionalArgs [ "--no-build"; "--no-restore" ])

Target.create "Artifacts" ignore

Target.create "Test:Unit" <| fun _ ->
    let outputPath = currentDirectory </> "test-results/unit-tests.trx"

    "WireMock.Net.ModelBuilders.UnitTests"
    |> DotNet.test (fun p ->
        { p with
            Configuration = buildConfig
            NoBuild = true
            NoRestore = true
            Logger = Some (sprintf "trx;LogFileName=%s" outputPath) })

Target.create "Test:Integration" <| fun _ ->
    let outputPath = currentDirectory </> "test-results/integration-tests.trx"

    "WireMock.Net.ModelBuilders.IntegrationTests"
    |> DotNet.test (fun p ->
        { p with
            Configuration = buildConfig
            NoBuild = true
            NoRestore = true
            Logger = Some (sprintf "trx;LogFileName=%s" outputPath) })

Target.create "Test" ignore

Target.create "Docker:Build" <| fun _ ->
    let args =
        [ "-f docker-compose.yml"
          sprintf "-f %s" composeFile
          "-f docker-compose.test.yml"
          "build" ]
        |> String.concat " "

    let env = [ "IMAGE_TAG", imageTag ]

    let success = args |> Docker.compose env
    if not success then failwithf "docker-compose build failed"

Target.create "Docker:Up" <| fun tp ->
    let hasTestTarget = tp.Context.TryFindTarget "Test:Integration" |> Option.isSome
    let hasDetachedArg = tp.Context.Arguments |> List.exists (fun x -> x = "-d" || x = "--detached")
    let detached = hasTestTarget || hasDetachedArg

    let env =
        [ "CONFIG", configuration
          "IMAGE_TAG", imageTag ]

    let args =
        [ sprintf "-f docker-compose.yml -f %s" composeFile
          (if detached then "up -d" else "up") ]
        |> String.concat " "

    let success = args |> Docker.compose env
    if not success then failwithf "docker-compose up failed"

Target.create "PaketPack" <| fun _ ->
    IO.Directory.ensure nupkgsDir
    Paket.pack <| fun p ->
        { p with
            OutputPath = nupkgsDir
            Version = gitVersion.SemVer }

Target.create "Root" ignore

open Fake.Core.TargetOperators

"Root"
    ==> "AssemblyInfo"
    ?=> "Clean"
    ==> "Restore"
    ==> "Build"
    ==> "Artifacts"
    ==> "Docker:Build"
    ==> "Docker:Up"

"Artifacts:Lib" ==> "Artifacts"
"Artifacts:Tests" ==> "Artifacts"

"Clean"
    ==> "Restore"
    ==> "Build"
    ==> "Test:Unit"
    ==> "Test"

"Docker:Up" ==> "Test:Integration" ==> "Test"

"Root"
    =?> ("AssemblyInfo", isCi)
    ==> "Artifacts"
    ==> "PaketPack"

Target.runOrDefaultWithArguments "Build"