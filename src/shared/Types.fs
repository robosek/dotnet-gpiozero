namespace DotnetGpiozero.Shared

type Pin = int
type Channel = int
type NotCheckedPin = NotCheckedPin of Pin
type NotCheckedPwmPin = NotCheckedPwmPin of (Pin * Channel)
type OpenedPin = OpenedPin of Pin
type OpenedPwmPin = OpenedPwmPin of (Pin * Channel)