namespace DotnetGpiozero.Robot
open DotnetGpiozero.Shared
open System.Device.Gpio

type RobotController() =

    let leftRightSpeed = 0.25
    let motorForward speed (Motor motor) =
        let (pin1,pin2, pinPwm) = motor

        pin1 |> write PinValue.High
        pin2 |> write PinValue.Low
        pinPwm |> writePwm speed

    let motorBackward speed (Motor motor) =
        let (pin1,pin2, pinPwm) = motor

        pin1 |> write PinValue.Low
        pin2 |> write PinValue.High
        pinPwm |> writePwm speed
    
    let motorStop (Motor motor) =
        let (pin1, pin2, pinPwm) = motor

        pin1 |> write PinValue.Low
        pin2 |> write PinValue.Low
        pinPwm |> writePwm 0.0

    member __.TryOpenMotorPins (pins : (NotCheckedPin * NotCheckedPin * NotCheckedPwmPin)) = 
        let (pin1, pin2, pinPwm) = pins
        let pinLeft = tryOpenPin pin1
        let pinRight = tryOpenPin pin2
        let pinPwm = tryOpenPwmPin pinPwm
        
        match pinLeft, pinRight, pinPwm with
        | Ok pinLeft, Ok pinRight, Ok pinPwm-> Ok(Motor(pinLeft,pinRight,pinPwm))
        | _,_, Error e -> Error(e)
        | Error e,_,_ -> Error(e)
        | _,Error e,_ -> Error(e)

    member __.Forward speed (robot: Robot) = 
        robot.LeftMotor  |> motorForward speed 
        robot.RightMotor |> motorForward speed  

    member __.LeftSoft speed (robot: Robot) = 
        robot.LeftMotor |> motorForward leftRightSpeed
        robot.RightMotor |> motorForward speed

    member __.LeftInPlace speed (robot: Robot) = 
        robot.LeftMotor |> motorBackward speed
        robot.RightMotor |> motorForward speed

    member __.RightSoft speed (robot: Robot)  = 
        robot.LeftMotor |> motorForward speed 
        robot.RightMotor |> motorForward leftRightSpeed

    member __.RightInPlace speed (robot: Robot)  = 
        robot.LeftMotor |> motorForward speed 
        robot.RightMotor |> motorBackward speed
    
    member __.Backward speed (robot: Robot) =
        robot.LeftMotor |> motorBackward speed  
        robot.RightMotor |> motorBackward speed
    
    member __.Stop (robot: Robot) = 
        robot.LeftMotor |> motorStop 
        robot.RightMotor |> motorStop