namespace DotnetGpiozero.Shared
open System.Device.Gpio

type Common() =

    [<Literal>]
    let SoftPwmFrequencyHz = 50.0

    member __.Write pinValue (OpenedPin (pin, gpioController)) =
        gpioController.Write(pin, pinValue )
    
    member __.WritePwm pinValue (OpenedPwmPin (pwmPin, pwmController)) =
        let (pin, channel) = pwmPin
        pwmController.ChangeDutyCycle(pin, channel, pinValue * 100.0)

    member __.TryOpenPin (NotCheckedPin (pin, gpioController)) : Result<OpenedPin, string> =
        try
            match gpioController.IsPinOpen pin with
            | true -> Ok(OpenedPin(pin, gpioController))
            | false -> gpioController.OpenPin(pin, PinMode.Output)
                       let openedPin = OpenedPin(pin, gpioController)
                       __.Write PinValue.Low openedPin
                       Ok(openedPin)
        with
        | ex -> Error ex.Message

    member __.TryOpenPwmPin (NotCheckedPwmPin (pinPwm, pwmController)) : Result<OpenedPwmPin, string> =
        try
           let (pin, channel) = pinPwm
           pwmController.OpenChannel(pin, channel)
           pwmController.StartWriting(pin, channel, SoftPwmFrequencyHz, 100.0)
           Ok(OpenedPwmPin(pinPwm, pwmController))
        with
        | ex -> Error ex.Message

    member __.TryClosePin (OpenedPin (pin, gpioController)) : Result<Unit,string> =
        try
            gpioController.ClosePin(pin)
            Ok(gpioController.Dispose())
        with
        | ex -> Error ex.Message

    member __.TryClosePwmPin (OpenedPwmPin (pin, pwmController)) : Result<Unit,string> =
        try
            let (pin, channel) = pin
            pwmController.CloseChannel(pin, channel)
            Ok(pwmController.Dispose())
        with
        | ex -> Error ex.Message
