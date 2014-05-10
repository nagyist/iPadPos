using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libsomo.combined.a", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, ForceLoad = true)]
