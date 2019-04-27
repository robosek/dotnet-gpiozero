namespace DotnetGpiozero.Shared
open System.Device.Gpio

[<AutoOpen>]
module Common =

    [<Literal>]
    let SoftPwmFrequencyHz = 50.0

    let write pinValue (OpenedPin (pin, gpioController)) =
        gpioController.Write(pin, pinValue )
    
    let writePwm pinValue (OpenedPwmPin (pwmPin, pwmController)) =
        pwmController.ChangeDutyCycle(pwmPin, 0, pinValue * 100.0)

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
           pwmController.OpenChannel(pinPwm, 0)
           pwmController.StartWriting(pinPwm, 0, SoftPwmFrequencyHz, 100.0)
           Ok(OpenedPwmPin(pinPwm, pwmController))
        with
        | ex -> Error ex.Message

    let tryClosePin (OpenedPin (pin, gpioController)) : Result<Unit,string> =
        try
            gpioController.ClosePin(pin)
            Ok(gpioController.Dispose())
        with
        | ex -> Error ex.Message

    let tryClosePwmPin (OpenedPwmPin (pin, pwmController)) : Result<Unit,string> =
        try
            pwmController.CloseChannel(pin, 0)
            Ok(pwmController.Dispose())
        with
        | ex -> Error ex.Message
    
    let createTimerAndObservable timerInterval =
        let timer = new System.Timers.Timer(float timerInterval)
        timer.AutoReset <- true
        let observable = timer.Elapsed  
        let task = async {timer.Start()}
        {Task = task; Observable =  observable; StopTask = timer.Stop}
