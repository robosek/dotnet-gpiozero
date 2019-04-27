open System 
open DotnetGpiozero.Robot
open DotnetGpiozero.Shared
open DotnetGpiozero.Led

[<EntryPoint>]
let main argv =

    let robotController = RobotController()
    let ledController = LedController()

    let leftMotorPins = NotCheckedPin.Create 27, NotCheckedPwmPin.Create 22
    let rightMotorPins = NotCheckedPin.Create 26, NotCheckedPwmPin.Create 17
    let ledPin = NotCheckedPin.Create 16

    let resultLedPin = tryOpenPin ledPin
    let resultLeftMotor = robotController.TryOpenMotorPins leftMotorPins
    let resultRightMotor = robotController.TryOpenMotorPins rightMotorPins
    
    let led = match resultLedPin with
              | Ok openedPin -> Led(openedPin)
              | Error e -> failwith(e)
    
    let ledEventHandler = led |> ledController.Blink 200
    ledEventHandler.BlinkTask |> Async.RunSynchronously

    let robot = match resultLeftMotor, resultRightMotor with
                | Ok motorLeft, Ok motorRight -> Robot(motorLeft, motorRight)
                | Error error1, Error error2 -> failwith(error1 + ", " + error2)
                | _, Error e -> failwith(e)
                | Error e, _ -> failwith(e)



    let forward = 
        robotController.Forward   
    
    let backward = 
        robotController.Backward

    let left = 
        robotController.LeftSoft
    
    let right = 
        robotController.RightSoft

    let stop = 
        robotController.Stop
    
    Async.Sleep 2000 |> Async.RunSynchronously
    ledEventHandler.StopBlinking()

    led |> ledController.On
    robot |> forward 1.0
    Console.WriteLine("Forward")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> backward 1.0
    Console.WriteLine("Backward")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> left 1.0
    Console.WriteLine("Left")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> right 1.0
    Console.WriteLine("Right")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> stop
    Console.WriteLine("Stop")
    led |> ledController.Off

    0