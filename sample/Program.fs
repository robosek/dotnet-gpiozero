open System 
open DotnetGpiozero.Robot
open DotnetGpiozero.Shared
open DotnetGpiozero.Led

[<EntryPoint>]
let main argv =

    //Initialize controllers 
    let robotController = RobotController()
    let ledController = LedController()

    //Setup devices pins
    let leftMotorPins = NotCheckedPin.Create 27, NotCheckedPwmPin.Create 22
    let rightMotorPins = NotCheckedPin.Create 26, NotCheckedPwmPin.Create 17
    let ledPin = NotCheckedPin.Create 16

    //Initialize pins
    let resultLedPin = tryOpenPin ledPin
    let resultLeftMotor = robotController.TryOpenMotorPins leftMotorPins
    let resultRightMotor = robotController.TryOpenMotorPins rightMotorPins
    
    //Check pins initialization results
    let led = match resultLedPin with
              | Ok openedPin -> Led(openedPin)
              | Error e -> failwith(e)

    let robot = match resultLeftMotor, resultRightMotor with
                | Ok motorLeft, Ok motorRight -> Robot(motorLeft, motorRight)
                | Error error1, Error error2 -> failwith(error1 + ", " + error2)
                | _, Error e -> failwith(e)
                | Error e, _ -> failwith(e)
    
    //Led blinking
    let ledEventHandler = led |> ledController.Blink 200
    ledEventHandler.BlinkTask |> Async.RunSynchronously
    //Simulate waiting for connection or some other awaiting...
    Async.Sleep 2000 |> Async.RunSynchronously
    //Awaiting done!
    ledEventHandler.StopBlinking()

    Async.Sleep 1000 |> Async.RunSynchronously

    //Let's the game begin
    led |> ledController.On
    robot |> robotController.Forward 1.0
    Console.WriteLine("Forward")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> robotController.Backward 1.0
    Console.WriteLine("Backward")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> robotController.LeftSoft 1.0
    Console.WriteLine("Left")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> robotController.RightSoft 1.0
    Console.WriteLine("Right")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> robotController.Stop
    Console.WriteLine("Stop")
    led |> ledController.Off

    0