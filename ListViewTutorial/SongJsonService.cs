using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ListViewTutorial
{
	public class SongJsonService: SongDataService
	{
		private string _storagePath;
		private List<Song> _songs = new List<Song>();

		#region SongDataService implementation
		public void RefreshCache ()
		{
			_songs.Clear ();
			string[] filenames = Directory.GetFiles (_storagePath, "*.json");

			foreach (string filename in filenames) {
				string songString = File.ReadAllText (filename);
				Song song = JsonConvert.DeserializeObject<Song> (songString);
				_songs.Add (song);
			}
		}

		private int GetNextId() 
		{
			if (_songs.Count == 0)
				return 1;
			else
				return _songs.Max (p => p.Id.Value) + 1;
		}
			
		public Song GetSong (int id)
		{
			Song song = _songs.Find (p => p.Id == id);
			return song;
		}
		public void SaveSong (Song song)
		{
			//	Boolean newSong = false;
			if (!song.Id.HasValue) {
				song.Id = GetNextId ();
				//newSong = true;
			}
			//
			//string songString = JsonConvert.SerializeObject (song);
			//
			//File.WriteAllText (GetFilename (song.Id.Value), songString);

			//if (newSong)
			_songs.Add (song);
		}
		public void DeleteSong (Song song)
		{
			File.Delete (GetFilename (song.Id.Value));
			_songs.Remove (song);
		}
		public IReadOnlyList<Song> Songs {
			get {
				return _songs;
			}
		}
		#endregion

		private string GetFilename(int id) 
		{
			return Path.Combine (_storagePath, "song" + id.ToString () + ".json");
		}

		public SongJsonService (string storagePath)
		{
			_storagePath = storagePath;

			if (!Directory.Exists (_storagePath))
				Directory.CreateDirectory (_storagePath);

			RefreshCache ();
		}
	}
}

