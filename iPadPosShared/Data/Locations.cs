using System;
using System.IO;
using MonoTouch.Foundation;
using System.Collections.Generic;

namespace iPadPos
{
	public static class Locations
	{
		static Locations()
		{
			if (!Directory.Exists(TempDir))
				Directory.CreateDirectory(TempDir);
			if (!Directory.Exists(LibDir))
				Directory.CreateDirectory(LibDir);
		}
		public static readonly string BaseDir = Directory.GetParent (Environment.GetFolderPath (Environment.SpecialFolder.Personal)).ToString ();
		public static readonly string AppDir = Path.GetFileName(NSBundle.MainBundle.BundlePath);
		public static readonly string DocumentsFolder = BaseDir + "/Documents/";
		public static readonly string TempDir = Path.Combine (BaseDir, "tmp/");
		public static readonly string LibDir = Path.Combine (BaseDir, "Library/");


		public static string MakeRelativePath(string path)
		{
			return MakeRelativePath (BaseDir + "/", path);
		}
		public static string MakeRelativePath(string fromPath, string toPath)
		{
			if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
			if (string.IsNullOrEmpty(toPath))   throw new ArgumentNullException("toPath");

			var fromUri = new Uri(fromPath);
			var toUri = new Uri(toPath);

			if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

			var relativeUri = fromUri.MakeRelativeUri(toUri);
			var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

			if (toUri.Scheme.ToUpperInvariant() == "FILE")
			{
				relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			}

			return relativePath;
		}
	}
}

