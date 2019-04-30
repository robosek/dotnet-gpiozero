open System 
open DotnetGpiozero.Robot
open DotnetGpiozero.Shared
open DotnetGpiozero.Led
open DotnetGpiozero.Buzzer

[<EntryPoint>]
let main argv =

    //Initialize controllers 
    let robotController = RobotController()
    let ledController = LedController()
    let buzzerController = BuzzerController()

    //Setup device pins
    let leftMotorPins = NotCheckedPin.Create 27,NotCheckedPin.Create 22, NotCheckedPwmPin.Create (HardwarePwmPin(0,0))
    let rightMotorPins = NotCheckedPin.Create 26, NotCheckedPin.Create 17, NotCheckedPwmPin.Create (HardwarePwmPin(0, 1))
    let ledPin = NotCheckedPin.Create 16
    let buzzerPin = NotCheckedPwmPin.Create (SoftPwmPin(21))

    //Initialize pins
    let resultLedPin = tryOpenPin ledPin
    let resultLeftMotor = robotController.TryOpenMotorPins leftMotorPins
    let resultRightMotor = robotController.TryOpenMotorPins rightMotorPins
    let resultBuzzerPin = tryOpenPwmPin buzzerPin
    
    //Check pins initialization results
    let led = match resultLedPin with
              | Ok openedPin -> Led(openedPin)
              | Error e -> failwith(e)
    
    let buzzer = match resultBuzzerPin with
                  | Ok openedPin -> Buzzer(openedPin)
                  | Error e -> failwith(e)

    let robot = match resultLeftMotor, resultRightMotor with
                | Ok motorLeft, Ok motorRight -> Robot(motorLeft, motorRight)
                | Error error1, Error error2 -> failwith(error1 + ", " + error2)
                | _, Error e -> failwith(e)
                | Error e, _ -> failwith(e)

    //Beep
    buzzer |> buzzerController.On 0.6
    // buzzerEventHandler.BeepTask |> Async.RunSynchronously
    Async.Sleep 2000 |> Async.RunSynchronously
    // buzzerEventHandler.StopBeeping()
    buzzer |> buzzerController.Off |> ignore
    
    //Led blinking
    let ledEventHandler = led |> ledController.Blink 200
    ledEventHandler.BlinkTask |> Async.RunSynchronously
    //Simulate waiting for connection or some other awaiting...
    Async.Sleep 2000 |> Async.RunSynchronously
    //Awaiting done!
    ledEventHandler.StopBlinking()

    Async.Sleep 1000 |> Async.RunSynchronously

    //Let the game begin
    led |> ledController.On
    robot |> robotController.Forward 0.5
    Console.WriteLine("Forward")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> robotController.Stop
    Console.WriteLine("Stop")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> robotController.Backward 0.4
    Console.WriteLine("Backward")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> robotController.Stop
    Console.WriteLine("Stop")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> robotController.LeftSoft 1.0
    Console.WriteLine("Left")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> robotController.Stop
    Console.WriteLine("Stop")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> robotController.RightSoft 1.0
    Console.WriteLine("Right")
    Async.Sleep 2000 |> Async.RunSynchronously

    robot |> robotController.Stop
    Console.WriteLine("Stop")
    led |> ledController.Off

    0