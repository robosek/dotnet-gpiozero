open Fake.DotNet
#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

let publishConfig (config:DotNet.PublishOptions) =
  {
    config with Runtime = Some("linux-arm")
  }

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    ++ "sample/bin"
    ++ "sample/obj"
    |> Shell.cleanDirs 
)

Target.create "Build" (fun _ ->
    !! "src/**/*.*proj"
    ++ "sample/*.*proj"
    |> Seq.iter (DotNet.build id)
)

Target.create "PublishSample" (fun _ ->
   !! "sample/*.*proj"
   |> Seq.iter (DotNet.publish publishConfig)
)

Target.create "All" ignore

"Clean"
  ==> "Build"
  ==> "PublishSample"
  ==> "All"

Target.runOrDefault "All"