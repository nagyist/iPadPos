using System;

#if __IOS__
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
#endif

namespace iPadPos
{
	public struct Color
	{
//		public static readonly Color Purple = 0xB455B6;
//		public static readonly Color Blue = 0x3498DB;
//		public static readonly Color DarkBlue = 0x2C3E50;
//		public static readonly Color Green = 0x77D065;
		public static readonly Color Gray = 0x828282;
		public static readonly Color LightGray = 0xC2C2C2;


		public static readonly Color Black = 0x141414;
		public static readonly Color Olive = 0xA4BD0A;
		public static readonly Color LightBlue = 0x0599B0;
		public static readonly Color Orange = 0xFFA615;
		public static readonly Color Red = 0xFF2E00;
		public static readonly Color DarkGray = 0x363636;


		public double R, G, B;

		public static Color FromHex (int hex)
		{
			Func<int, int> at = offset => (hex >> offset) & 0xFF;
			return new Color {
				R = at (16) / 255.0,
				G = at (8) / 255.0,
				B = at (0) / 255.0
			};
		}

		public static implicit operator Color (int hex)
		{
			return FromHex (hex);
		}

		#if __IOS__
		public UIColor ToUIColor ()
		{
			return UIColor.FromRGB ((float)R, (float)G, (float)B);
		}

		public static implicit operator UIColor (Color color)
		{
			return color.ToUIColor ();
		}

		public static implicit operator CGColor (Color color)
		{
			return color.ToUIColor ().CGColor;
		}
		#endif

		#if __ANDROID__
		public Android.Graphics.Color ToAndroidColor ()
		{
			return Android.Graphics.Color.Rgb ((int)(255 * R), (int)(255 * G), (int)(255 * B));
		}

		public static implicit operator Android.Graphics.Color (Color color)
		{
			return color.ToAndroidColor ();
		}
		#endif
	}
}