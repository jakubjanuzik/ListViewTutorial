using System;
using System.Collections.Generic;

namespace ListViewTutorial
{
	public interface SongDataService
	{
		IReadOnlyList<Song> Songs { get; }
		void RefreshCache();
		Song GetSong (int id);
		void SaveSong(Song song);
		void DeleteSong(Song song);
	}
}

