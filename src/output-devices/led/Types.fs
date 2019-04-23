namespace DotnetGpiozero.Led
open DotnetGpiozero.Shared

type Led(pin: OpenedPin) = 
    member __.OpenedPin = pin

type LedState = 
    On | Off | Blink