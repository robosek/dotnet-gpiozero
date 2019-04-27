namespace DotnetGpiozero.Led
open DotnetGpiozero.Shared
open System.Threading
open System.Device.Gpio

type LedController() =
    let blinkOnce (pauseTime: int) (led: Led) =
        led.OpenedPin |> write PinValue.High
        Thread.Sleep pauseTime
        led.OpenedPin |> write PinValue.Low
        Thread.Sleep pauseTime

    member __.On (led: Led) = 
        led.OpenedPin |> write PinValue.High
   
    member __.Off (led: Led) = 
        led.OpenedPin |> write PinValue.Low
    
    member __.Blink (pauseTime: int) (led: Led) =
        let eventStream = createTimerAndObservable pauseTime
        eventStream.Observable 
        |> Observable.subscribe (fun _ -> blinkOnce pauseTime led) |> ignore
        {BlinkTask = eventStream.Task; StopBlinking = eventStream.StopTask}
