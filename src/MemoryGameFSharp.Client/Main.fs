namespace Main

open System
open System.Timers
open Elmish
open Bolero
open Bolero.Html
open Cards

module Main =

    type Model = 
        { Status : string
          GameActive : bool
          Timer : Timer
          TimeStart : DateTime
          TimeElapsed : TimeSpan
          CardContainer : CardContainer.Model }

        static member Default = 
            { Status = ""; 
              GameActive = true
              TimeStart = DateTime.Now 
              Timer = new Timer(1.0)
              TimeElapsed = TimeSpan.Zero
              CardContainer = CardContainer.Model.Default }

    type Message =
        | Tick of DateTime
        | StopTimer
        | StartTimer
        | Restart
        | SelectSize
        | CardContainerMessage of CardContainer.Message

    let timer (initModel : Model) =
        Cmd.ofSub <| fun dispatch ->
            let tmr = initModel.Timer
            tmr.Elapsed.Add ( fun _ -> Tick DateTime.Now |> dispatch )
            tmr.Start()

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
                    let timeElapsedString = model.TimeElapsed.ToString(@"mm\:ss\.fff")
                    text $"time elapsed: {timeElapsedString}"
                    button [
                        on.click <| (fun _ -> dispatch StartTimer)
                    ] [
                        text "Start Timer"
                    ]
                    button [
                        on.click <| (fun _ -> dispatch StopTimer)
                    ] [
                        text "Stop Timer"
                    ]
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
        | StopTimer ->
            model.Timer.Stop()
            model, Cmd.none
        | StartTimer ->
            model.Timer.Start()
            { model with TimeElapsed = TimeSpan.Zero; TimeStart = DateTime.Now }, Cmd.none
        | _ -> model, Cmd.none

    type MyApp() =
        inherit ProgramComponent<Model, Message>()

        override this.Program =
            Program.mkProgram (fun _ -> Model.Default, Cmd.ofMsg (Tick DateTime.Now)) update view
            |> Program.withSubscription timer
