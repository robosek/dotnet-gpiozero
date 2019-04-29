namespace DotnetGpiozero.Buzzer
open DotnetGpiozero.Shared
open System.Device.Gpio
open System.Threading

type BuzzerController() =
    let beepOnce (pauseTime: int) (buzzer: Buzzer) =
        buzzer.OpenedPin |> write PinValue.High
        Thread.Sleep pauseTime
        buzzer.OpenedPin |> write PinValue.Low
        Thread.Sleep pauseTime

    member __.On (buzzer: Buzzer) = 
        buzzer.OpenedPin |> write PinValue.High
   
    member __.Off (buzzer: Buzzer) = 
        buzzer.OpenedPin |> write PinValue.Low
    
    member __.Beep (beepTime: int) (buzzer: Buzzer) =
        let eventStream = createTimerAndObservable beepTime
        eventStream.Observable 
        |> Observable.subscribe (fun _ -> beepOnce beepTime buzzer) |> ignore
        {BlinkTask = eventStream.Task; StopBlinking = eventStream.StopTask}