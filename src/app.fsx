//---------------------------------------------------------------------

#I "../packages/Suave/lib/net40"
#r "../packages/Suave/lib/net40/Suave.dll"
#r "System.Runtime.Serialization.dll"

open System
open System.Net
open System.IO
open System.Runtime.Serialization

open Suave            // always open suave
open Suave.Http
open Suave.Filters
open Suave.Operators
open Suave.Successful // for OK-result
open Suave.Web        // for config
open Suave.Json


[<DataContract>]
type Foo =
  { [<field: DataMember(Name = "foo")>]
    foo : string }

printfn "initializing script..."


let config =
    let ip = IPAddress.Parse "0.0.0.0"
    { defaultConfig with
        logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Verbose
        bindings=[ HttpBinding.mk HTTP ip 8080us ] }

printfn "starting web server..."

let jsonMime = Writers.setMimeType "application/json"
let app =
  choose
    [ GET >=> choose
                [
                  // path "/api/login" >=> jsonMime >=> OK (toString <| toJson { foo = "foo" })
                  // pathScan "/api/json/%d" (fun n -> jsonMime >=> OK (jsonText n))*)
                ]
      POST >=> choose
                [ path "/api/login" >=> jsonMime >=> OK (fromString >> fromJson) ] ]


startWebServer config app
printfn "exiting server..."
