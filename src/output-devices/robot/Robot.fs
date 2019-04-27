namespace DotnetGpiozero.Robot
open DotnetGpiozero.Shared
open System.Device.Gpio

type RobotController() =

    let motorForward speed (Motor motor) =
        let (pin1, pinPwm) = motor

        pin1 |> write PinValue.High
        pinPwm |> writePwm (1.0 - speed)

    let motorBackward speed (Motor motor) =
        let (pin1, pinPwm) = motor

        pin1 |> write PinValue.Low
        pinPwm |> writePwm speed
    
    let motorStop (Motor motor) =
        let (pin1, pinPwm) = motor

        pin1 |> write PinValue.Low
        pinPwm |> writePwm 0.0

    member __.TryOpenMotorPins (pins : (NotCheckedPin *  NotCheckedPwmPin)) = 
        let (pin1, pinPwm) = pins
        let pinLeft = tryOpenPin pin1
        let pinPwm = tryOpenPwmPin pinPwm
        
        match pinLeft,pinPwm with
        | Ok pinLeft,Ok pinPwm-> Ok(Motor(pinLeft,pinPwm))
        | _, Error e -> Error(e)
        | Error e,_ -> Error(e)

    member __.Forward speed (robot: Robot) = 
        robot.LeftMotor  |> motorForward speed 
        robot.RightMotor |> motorForward speed  

    member __.LeftSoft speed (robot: Robot) = 
        robot.LeftMotor |> motorForward 0.15
        robot.RightMotor |> motorForward speed

    member __.LeftInPlace speed (robot: Robot) = 
        robot.LeftMotor |> motorBackward speed
        robot.RightMotor |> motorForward speed

    member __.RightSoft speed (robot: Robot)  = 
        robot.LeftMotor |> motorForward speed 
        robot.RightMotor |> motorForward 0.15

    member __.RightInPlace speed (robot: Robot)  = 
        robot.LeftMotor |> motorForward speed 
        robot.RightMotor |> motorBackward speed
    
    member __.Backward speed (robot: Robot) =
        robot.LeftMotor |> motorBackward speed  
        robot.RightMotor |> motorBackward speed
    
    member __.Stop (robot: Robot) = 
        robot.LeftMotor |> motorStop 
        robot.RightMotor |> motorStop