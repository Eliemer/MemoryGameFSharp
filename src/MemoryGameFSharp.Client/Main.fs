namespace Main

open System
open System.Threading
open Elmish
open Bolero
open Bolero.Html
open Cards
open System.Threading.Tasks

module Main =

    type Model = 
        { Status : string
          GameActive : bool
          TimeStart : DateTime
          TimeElapsed : TimeSpan
          CardContainer : CardContainer.Model }

        static member Default = 
            { Status = ""; 
              GameActive = true
              TimeStart = DateTime.Now 
              TimeElapsed = TimeSpan.Zero
              CardContainer = CardContainer.Model.Default }

    type Message =
        | Tick of DateTime
        | Restart
        | SelectSize
        | CardContainerMessage of CardContainer.Message

    let timer initModel =
        let sub dispatch = 
            async {
                while true do
                    do dispatch <| Tick DateTime.Now
                    do! Async.Sleep(1)
            } |> Async.RunSynchronously

        Cmd.ofSub sub

    let view model dispatch =
        section [] [
            h1 [
                attr.``class`` "game--title"
            ] [
                text "Memory"
                div [
                    attr.``class`` "game--size-selector"
                ] [
                    // TODO: Implement size selector component
                ]
                div [
                    attr.``class`` "game--timer"
                ] [
                    text $"time elapsed: {model.TimeElapsed}"
                ]
                div [
                    attr.``class`` "game--container"
                ] [ 
                    CardContainer.view model.CardContainer dispatch
                ]
                div [
                    attr.``class`` "game--stat-block"
                ] [
                    // TODO: Implement stat block component
                ]
            ]
            footer [] [
                text "Created by Ellie"
            ]
        ]

    let update message model = 
        match message with 
        | CardContainerMessage msg -> 
            let (mdl, cmd) = CardContainer.update msg model.CardContainer
            { model with CardContainer = mdl }, cmd
        | Tick now -> { model with TimeElapsed = now - model.TimeStart }, Cmd.none
        | _ -> model, Cmd.none

    type MyApp() =
        inherit ProgramComponent<Model, Message>()

        override this.Program =
            Program.mkProgram (fun _ -> Model.Default, Cmd.ofMsg (Tick DateTime.Now)) update view
            |> Program.withSubscription timer
