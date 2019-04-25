namespace DotnetGpiozero.Robot
open DotnetGpiozero.Shared
open System.Device.Gpio

module RobotController =

    let private motorForward speed (Motor motor) =
        let (pin1, pin2, pinPwm) = motor

        pin1 |> write PinValue.High
        pin2 |> write PinValue.Low
        pinPwm |> writePwm speed

    let private motorBack speed (Motor motor) =
        let (pin1, pin2, pinPwm) = motor

        pin1 |> write PinValue.Low
        pin2 |> write PinValue.High
        pinPwm |> writePwm speed
    
    let private motorStop (Motor motor) =
        let (pin1, pin2, pinPwm) = motor

        pin1 |> write PinValue.Low
        pin2 |> write PinValue.Low
        pinPwm |> writePwm 0.0

    let tryOpenMotorPins (pins : (NotCheckedPin * NotCheckedPin * NotCheckedPwmPin)) = 
        let (pin1, pin2, pinPwm) = pins
        let pinLeft = tryOpenPin pin1
        let pinRight = tryOpenPin pin2
        let pinPwm = tryOpenPwmPin pinPwm
        
        match pinLeft,pinRight, pinPwm with
        | Ok pinLeft,Ok pinRight, Ok pinPwm-> Ok(Motor(pinLeft,pinRight, pinPwm))
        | _,_, Error e -> Error(e)
        | _,Error e,_ -> Error(e)
        | Error e, _,_ -> Error(e)

    let forward speed (robot: Robot) = 
        robot.LeftMotor  |> motorForward speed
        robot.RightMotor |> motorForward speed

    let left speed (robot: Robot) = 
        robot.LeftMotor |> motorBack speed
        robot.RightMotor |> motorForward speed

    let right speed (robot: Robot)  = 
        robot.LeftMotor |> motorForward speed
        robot.RightMotor |> motorBack speed
    
    let backward speed (robot: Robot) =
        robot.LeftMotor |> motorBack speed
        robot.RightMotor |> motorBack speed
    
    let stop (robot: Robot) = 
        robot.LeftMotor |> motorStop 
        robot.RightMotor |> motorStop 