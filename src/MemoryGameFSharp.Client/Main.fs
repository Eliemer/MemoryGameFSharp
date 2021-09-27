module MemoryGameFSharp.Client.Main

open System
open System.Net.Http
open System.Net.Http.Json
open Microsoft.AspNetCore.Components
open Elmish
open Bolero
open Bolero.Html

/// The Elmish application's model.
type Model =

/// The Elmish application's update messages.
type Message =


let update message model =


let view model dispatch =


type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        let update = update this.HttpClient
        Program.mkProgram (fun _ -> initModel, Cmd.ofMsg GetBooks) update view
