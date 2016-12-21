namespace ChatBackend

module App = 
    open System
    open System.Net
    open System.IO

    open Suave            // always open suave
    open Suave.Http
    open Suave.Filters
    open Suave.Operators
    open Suave.Successful // for OK-result
    open Suave.Web        // for config
    open Suave.Json

    //open ChatBackend.Helpers

    let private config =
        let ip = IPAddress.Parse "0.0.0.0"
        { defaultConfig with
            logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Verbose
            bindings=[ HttpBinding.mk HTTP ip 8080us ] }


    let private jsonMime = Writers.setMimeType "application/json"

    let private app = 
                choose
                    [ GET >=> choose
                        [ path "/api/login" >=> OK ("Hello")
                          path "/" >=> OK ("Hello") ]
                      POST >=> choose 
                            [
                                path "/" >=> OK ("Hello")
                            ]
                            //[ path "/api/login" >=> jsonMime >=> OK (fromString >> fromJson) ] 
                        ]

    let start () =
        startWebServer config app


