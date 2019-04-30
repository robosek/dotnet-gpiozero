namespace DotnetGpiozero.Shared
open System.Device.Gpio
open System.Device.Pwm
open System.Device.Pwm.Drivers
open System.Timers

type Pin = int
type Channel = int
type PwmPin = | SoftPwmPin of Pin | HardwarePwmPin of Pin * Channel

type NotCheckedPin = private NotCheckedPin of Pin * GpioController
                     with static member Create pin = 
                            NotCheckedPin(pin, new GpioController())

type NotCheckedPwmPin = private NotCheckedPwmPin of PwmPin * PwmController
                        with static member Create pin =
                                match pin with
                                | HardwarePwmPin _ -> NotCheckedPwmPin(pin, new PwmController(new UnixPwmDriver()))
                                | SoftPwmPin _ -> NotCheckedPwmPin(pin, new PwmController(new SoftPwm()))

type OpenedPin = private OpenedPin of Pin * GpioController
                 with static member Create pin gpioController = 
                               OpenedPin(pin, gpioController)

type OpenedPwmPin = private OpenedPwmPin of PwmPin * PwmController
                    with static member Create pin pwmController = 
                                 OpenedPwmPin(pin, pwmController)

type EventStream = 
        {
            Task: Async<unit>; 
            Observable: IEvent<ElapsedEventHandler, ElapsedEventArgs>
            StopTask: Unit -> Unit
        }