namespace DotnetGpiozero.Shared
open System.Device.Gpio
open System.Device.Pwm
open System.Device.Pwm.Drivers

type Pin = int
type Channel = int
type PwmPin = Pin * Channel

type NotCheckedPin = private NotCheckedPin of Pin * GpioController
                     with static member Create pin = 
                            NotCheckedPin(pin, new GpioController())

type NotCheckedPwmPin = private NotCheckedPwmPin of PwmPin * PwmController
                        with static member Create pin = 
                              NotCheckedPwmPin(pin, new PwmController(new UnixPwmDriver())) 
                                
type OpenedPin = private OpenedPin of Pin * GpioController
                  with static member Create pin gpioController = 
                               OpenedPin(pin, gpioController)

type OpenedPwmPin = private OpenedPwmPin of PwmPin * PwmController
                    with static member Create pin pwmController = 
                                 OpenedPwmPin(pin, pwmController)

type EventStream = 
        {
            Task: Async<unit>; 
            Observable: IEvent<System.Timers.ElapsedEventHandler,System.Timers.ElapsedEventArgs>
            StopTask: Unit -> Unit
        }