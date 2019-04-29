namespace DotnetGpiozero.Buzzer
open DotnetGpiozero.Shared

type Buzzer(pin: OpenedPin) = 
    member __.OpenedPin = pin

type BuzzerState = 
    On | Off | Beep

type BuzzerBlinkEventHandler = 
    {
        BlinkTask: Async<Unit>
        StopBlinking: Unit -> Unit
    }