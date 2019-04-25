namespace DotnetGpiozero.Robot

open DotnetGpiozero.Shared

type Motor = Motor of (OpenedPin * OpenedPin * OpenedPwmPin)

type Robot(leftMotor: Motor, rightMotor: Motor) = 
    member __.LeftMotor = leftMotor
    member __.RightMotor = rightMotor