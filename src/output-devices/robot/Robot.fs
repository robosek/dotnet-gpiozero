namespace DotnetGpiozero.Robot
open DotnetGpiozero.Shared
open System.Device.Gpio

type RobotController(common: Common) =

    let motorForward speed (Motor motor) =
        let (pin1, pinPwm) = motor

        pin1 |> common.Write PinValue.High
        pinPwm |> common.WritePwm (1.0 - speed)

    let motorBack speed (Motor motor) =
        let (pin1, pinPwm) = motor

        pin1 |> common.Write PinValue.Low
        pinPwm |> common.WritePwm speed
    
    let motorStop (Motor motor) =
        let (pin1, pinPwm) = motor

        pin1 |> common.Write PinValue.Low
        pinPwm |> common.WritePwm 0.0

    member __.TryOpenMotorPins (pins : (NotCheckedPin *  NotCheckedPwmPin)) = 
        let (pin1, pinPwm) = pins
        let pinLeft = common.TryOpenPin pin1
        let pinPwm = common.TryOpenPwmPin pinPwm
        
        match pinLeft,pinPwm with
        | Ok pinLeft,Ok pinPwm-> Ok(Motor(pinLeft,pinPwm))
        | _, Error e -> Error(e)
        | Error e,_ -> Error(e)
     

    member __.Forward speed (robot: Robot) = 
        robot.LeftMotor  |> motorForward speed 
        robot.RightMotor |> motorForward speed  

    member __.Left speed (robot: Robot) = 
        robot.LeftMotor |> motorForward 0.2
        robot.RightMotor |> motorForward speed

    member __.Right speed (robot: Robot)  = 
        robot.LeftMotor |> motorForward speed 
        robot.RightMotor |> motorForward 0.2 
    
    member __.Backward speed (robot: Robot) =
        robot.LeftMotor |> motorBack speed  
        robot.RightMotor |> motorBack speed
    
    member __.Stop (robot: Robot) = 
        robot.LeftMotor |> motorStop 
        robot.RightMotor |> motorStop