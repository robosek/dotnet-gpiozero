namespace DotnetGpiozero.Shared
open System.Device.Gpio

[<AutoOpen>]
module Common =

    [<Literal>]
    let SoftPwmFrequencyHz = 100.0

    [<Literal>]
    let HardwarePwmFrequencyHz = 100.0

    let write pinValue (OpenedPin (pin, gpioController)) =
        gpioController.Write(pin, pinValue )
    
    let writePwm pinValue (OpenedPwmPin (pwmPin, pwmController)) =
        let pin, channel = match pwmPin with
                            | SoftPwmPin pin -> (pin, 0)
                            | HardwarePwmPin (pin, channel) -> (pin, channel)
        pwmController.ChangeDutyCycle(pin, channel, pinValue * 100.0)
    
    let stopWritingPwm (OpenedPwmPin (pwmPin, pwmController)) =
        match pwmPin with
        | SoftPwmPin pin -> pwmController.StopWriting(pin, 0)
        | HardwarePwmPin (pin,channel) -> pwmController.StopWriting(pin, channel)

    let tryOpenPin (NotCheckedPin (pin, gpioController)) : Result<OpenedPin, string> =
        try
            match gpioController.IsPinOpen pin with
            | true -> Ok(OpenedPin(pin, gpioController))
            | false -> gpioController.OpenPin(pin, PinMode.Output)
                       let openedPin = OpenedPin(pin, gpioController)
                       write PinValue.Low openedPin
                       Ok(openedPin)
        with
        | ex -> Error ex.Message

    let tryOpenPwmPin (NotCheckedPwmPin (pinPwm, pwmController)) : Result<OpenedPwmPin, string> =
        try
           match pinPwm with
           | SoftPwmPin pin -> pwmController.OpenChannel(pin, 0)
                               pwmController.StartWriting(pin, 0, SoftPwmFrequencyHz, 0.0)
           | HardwarePwmPin (pin, channel) -> pwmController.OpenChannel(pin, channel)
                                              pwmController.StartWriting(pin, channel, HardwarePwmFrequencyHz, 0.0)
           Ok(OpenedPwmPin(pinPwm, pwmController))
        with
        | ex -> Error ex.Message

    let tryClosePin (OpenedPin (pin, gpioController)) : Result<Unit,string> =
        try
            gpioController.ClosePin(pin)
            Ok(gpioController.Dispose())
        with
        | ex -> Error ex.Message

    let tryClosePwmPin (OpenedPwmPin (pwmPin, pwmController)) : Result<Unit,string> =
        try
           let pin, channel = match pwmPin with
                                | SoftPwmPin pin -> (pin, 0)
                                | HardwarePwmPin (pin, channel) -> (pin, channel)
           pwmController.CloseChannel(pin, channel)
           Ok(pwmController.Dispose())
        with
        | ex -> Error ex.Message
    
    let createTimerAndObservable timerInterval =
        let timer = new System.Timers.Timer(float timerInterval)
        timer.AutoReset <- true
        let observable = timer.Elapsed  
        let task = async {timer.Start()}
        {Task = task; Observable =  observable; StopTask = timer.Stop}
