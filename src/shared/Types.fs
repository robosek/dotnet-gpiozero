namespace DotnetGpiozero.Shared
open System.Device.Gpio
open System.Device.Pwm

type Pin = int
type Channel = int
type PwmPin = Pin * Channel
type NotCheckedPin = NotCheckedPin of Pin * GpioController
type NotCheckedPwmPin = NotCheckedPwmPin of PwmPin * PwmController
type OpenedPin = OpenedPin of Pin * GpioController
type OpenedPwmPin = OpenedPwmPin of PwmPin * PwmController