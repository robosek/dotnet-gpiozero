namespace DotnetGpiozero.Buzzer
open DotnetGpiozero.Shared
open System.Threading

type BuzzerController() =
    let beepOnce (pauseTime: int) volume (buzzer: Buzzer) =
        buzzer.OpenedPwmPin |> writePwm volume
        Thread.Sleep pauseTime
        buzzer.OpenedPwmPin |> writePwm volume
        Thread.Sleep pauseTime

    member __.On volume (buzzer: Buzzer) = 
        buzzer.OpenedPwmPin |> writePwm volume
   
    member __.Off (buzzer: Buzzer) = 
        buzzer.OpenedPwmPin |> stopWritingPwm
        buzzer.OpenedPwmPin |> tryClosePwmPin
    
    member __.Beep (beepTime: int) volume (buzzer: Buzzer) =
        let eventStream = createTimerAndObservable beepTime
        eventStream.Observable 
        |> Observable.subscribe (fun _ -> beepOnce beepTime volume buzzer) |> ignore
        {BeepTask = eventStream.Task; StopBeeping = eventStream.StopTask}