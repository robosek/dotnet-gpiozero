namespace DotnetGpiozero.Shared

open System.Device.Gpio
open System.Device.Pwm
open System.Device.Pwm.Drivers

[<AutoOpen>]
module Common =
    let private gpioController = new GpioController(PinNumberingScheme.Logical)
    let private pwmController = new PwmController(new SoftPwm())
    [<Literal>] 
    let private SoftPwmFrequencyHz = 50.0

    let write pinValue (OpenedPin pinNumber) =
        gpioController.Write(pinNumber, pinValue)
    
    let writePwm pinValue (OpenedPwmPin openedPinPwm) =
        let (pin, channel) = openedPinPwm
        pwmController.ChangeDutyCycle(pin, channel, pinValue * 100.0)

    let tryOpenPin (NotCheckedPin pinNumber) : Result<OpenedPin, string> =
        try
            match gpioController.IsPinOpen pinNumber with
            | true -> Ok(OpenedPin(pinNumber))
            | false -> gpioController.OpenPin(pinNumber,PinMode.Output)
                       let openedPin = OpenedPin(pinNumber)
                       write PinValue.Low openedPin
                       Ok(openedPin)
        with
        | ex -> Error ex.Message

    let tryOpenPwmPin (NotCheckedPwmPin pinPwm) : Result<OpenedPwmPin, string> =
        try
           let (pin, channel) = pinPwm
           pwmController.OpenChannel(pin, channel)
           pwmController.StartWriting(pin, channel, SoftPwmFrequencyHz, 0.0)
           Ok(OpenedPwmPin(pinPwm))
        with
        | ex -> Error ex.Message

    let tryClosePin (OpenedPin pinNumber) : Result<Unit,string> =
        try
            Ok(gpioController.ClosePin(pinNumber))
        with
        | ex -> Error ex.Message

    let tryClosePwmPin (OpenedPwmPin openedPin) : Result<Unit,string> =
        try
            let (pin, channel) = openedPin
            Ok(pwmController.CloseChannel(pin, channel))
        with
        | ex -> Error ex.Message
