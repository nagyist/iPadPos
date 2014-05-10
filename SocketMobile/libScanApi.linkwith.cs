using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libScanApi.a", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, Frameworks = "ExternalAccessory, Foundation, AVFoundation, AudioToolbox", ForceLoad = true, IsCxx = true)]
