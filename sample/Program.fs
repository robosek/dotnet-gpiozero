open System 
open DotnetGpiozero.Robot
open DotnetGpiozero.Shared
open System.Threading
open DotnetGpiozero.Led

[<EntryPoint>]
let main argv =

    let leftMotorPins = NotCheckedPin(27),NotCheckedPin(22)
    let rightMotorPins = NotCheckedPin(26),NotCheckedPin(17)
    let ledPin = NotCheckedPin(19)

    let resultLedPin = tryOpenPin ledPin
    let resultLeftMotor = RobotController.tryOpenMotorPins leftMotorPins
    let resultRightMotor = RobotController.tryOpenMotorPins rightMotorPins
    
    let led = match resultLedPin with
              | Ok openedPin -> Led(openedPin)
              | Error e -> failwith(e)

    let robot = match resultLeftMotor, resultRightMotor with
                | Ok motorLeft, Ok motorRight -> Robot(motorLeft, motorRight)
                | _, Error e -> failwith(e)
                | Error e, _ -> failwith(e)

    LedController.on led
    robot |> RobotController.forward 0.5
    Console.WriteLine("Forward")
    Thread.Sleep 2000
    robot |> RobotController.backward 1.0
    Console.WriteLine("Backward")
    Thread.Sleep 2000
    robot |> RobotController.left 0.3
    Console.WriteLine("Left")
    Thread.Sleep 2000
    robot |> RobotController.right 0.7
    Console.WriteLine("Right")
    Thread.Sleep 2000
    RobotController.stop robot
    Console.WriteLine("Stop")
    LedController.off led

    0