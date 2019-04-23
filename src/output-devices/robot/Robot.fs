namespace DotnetGpiozero.Robot
open DotnetGpiozero.Shared
open System.Device.Gpio

module RobotController =

    let tryOpenMotorPins (pins : (NotCheckedPin * NotCheckedPin)) = 
        let (pin1,pin2) = pins
        let pinLeft = tryOpenPin pin1
        let pinRight = tryOpenPwmPin pin2
        
        match pinLeft,pinRight with
        | Ok pinLeft,Ok pinRight-> Ok(Motor(pinLeft,pinRight))
        | _, Error e -> Error(e)
        | Error e, _ -> Error(e)

    let private motorForward speed (Motor motor) =
        let (pin1, pin2) = motor

        pin1 |> write PinValue.High
        pin2 |> writePwm speed

    let private motorBack speed (Motor motor) =
        let (pin1, pin2) = motor

        pin1 |> write PinValue.Low
        pin2 |> writePwm speed
    
    let private motorStop (Motor motor) =
        let (pin1, pin2) = motor

        pin1 |> write PinValue.Low
        pin2 |> writePwm 0.0

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