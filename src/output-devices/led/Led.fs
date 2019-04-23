namespace DotnetGpiozero.Led
open DotnetGpiozero.Shared
open System.Threading
open System.Device.Gpio


module LedController =

    let on (led: Led) = 
        led.OpenedPin |> write PinValue.High
   
    let off (led: Led) = 
        led.OpenedPin |> write PinValue.Low
    
    let blink (led: Led) (pauseTime: int) =
        led.OpenedPin |> write PinValue.High
        Thread.Sleep pauseTime
        led.OpenedPin |> write PinValue.Low
        Thread.Sleep pauseTime