open System 
open DotnetGpiozero.Robot
open DotnetGpiozero.Shared
open System.Threading
open DotnetGpiozero.Led
open System.Device.Gpio
open System.Device.Pwm
open System.Device.Pwm.Drivers
open DotnetGpiozero.Led

[<EntryPoint>]
let main argv =

    let common = Common()
    let robotController = RobotController(common)
    let ledController = LedController(common)

    let leftNotCheckedPin = NotCheckedPin(27, new GpioController())
    let leftNotCheckedPwmPin = NotCheckedPwmPin(PwmPin(22,0), new PwmController(new SoftPwm(true)))
    let rightNotCheckedPin = NotCheckedPin(26, new GpioController())
    let rightNotCheckedPwmPin = NotCheckedPwmPin(PwmPin(17,0), new PwmController(new SoftPwm(true)))

    let leftMotorPins = leftNotCheckedPin,leftNotCheckedPwmPin
    let rightMotorPins = rightNotCheckedPin,rightNotCheckedPwmPin
    let ledPin = NotCheckedPin(19, new GpioController())

    let resultLedPin = common.TryOpenPin ledPin
    let resultLeftMotor = robotController.TryOpenMotorPins leftMotorPins
    let resultRightMotor = robotController.TryOpenMotorPins rightMotorPins
    
    let led = match resultLedPin with
              | Ok openedPin -> Led(openedPin)
              | Error e -> failwith(e)

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
        robotController.Left
    
    let right = 
        robotController.Right

    let stop = 
        robotController.Stop

    led |> ledController.On
    robot |> forward 1.0
    Console.WriteLine("Forward")
    Thread.Sleep 2000

    robot |> backward 1.0
    Console.WriteLine("Backward")
    Thread.Sleep 2000

    robot |> left 1.0
    Console.WriteLine("Left")
    Thread.Sleep 2000

    robot |> right 1.0
    Console.WriteLine("Right")
    Thread.Sleep 2000

    robot |> stop
    Console.WriteLine("Stop")
    led |> ledController.Off

    0