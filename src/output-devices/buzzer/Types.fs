namespace DotnetGpiozero.Buzzer
open DotnetGpiozero.Shared

type Buzzer(pin: OpenedPwmPin) = 
    member __.OpenedPwmPin = pin

type BuzzerBlinkEventHandler = 
    {
        BeepTask: Async<Unit>
        StopBeeping: Unit -> Unit
    }