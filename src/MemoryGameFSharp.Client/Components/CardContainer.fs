namespace Cards

open Elmish
open Bolero
open Bolero.Html

module Card =
    type CardLabel = string

    type CardImage = 
        { Uri : string 
          Label : CardLabel }
        static member Default = 
            { Uri = "" 
              Label = "" }

    type Card =
        { Front : CardImage 
          Back : CardImage }
        static member Default = 
            { Front = CardImage.Default 
              Back = CardImage.Default }

module CardContainer = 
    type Model = 
        { Cards : Card.Card array }
        static member Default = 
            { Cards = Array.create 12 Card.Card.Default }

    type Message =
        | Flip
        | Unflip
        | Match

    let view model dispatch = 
        Empty

    let update message model = model, Cmd.none