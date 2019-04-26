namespace DotnetGpiozero.Led
open DotnetGpiozero.Shared
open System.Threading
open System.Device.Gpio

type LedController(common: Common) =

    member __.On (led: Led) = 
        led.OpenedPin |> common.Write PinValue.High
   
    member __.Off (led: Led) = 
        led.OpenedPin |> common.Write PinValue.Low
    
    member __.Blink (led: Led) (pauseTime: int) =
        led.OpenedPin |> common.Write PinValue.High
        Thread.Sleep pauseTime
        led.OpenedPin |> common.Write PinValue.Low
        Thread.Sleep pauseTime