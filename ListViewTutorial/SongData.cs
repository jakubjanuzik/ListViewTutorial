using System;
using System.IO;

namespace ListViewTutorial
{
	public class SongData
	{
		public static readonly SongDataService Service = new 
			SongJsonService (
				Path.Combine (
					Android.OS.Environment.ExternalStorageDirectory.Path,
					"ListViewApp"
				)
			);
	}
}

