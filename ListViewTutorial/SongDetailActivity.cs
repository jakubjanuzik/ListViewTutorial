
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Sockets;


namespace ListViewTutorial
{
	[Activity (Label = "SongDetailActivity")]			
	public class SongDetailActivity : Activity
	{
		EditText _titleEditText;
		EditText _artistEditText;
		Button _playPauseButton;
		Button _stopButton;
		Song _song;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.SongDetail);
			_titleEditText = FindViewById<EditText> (Resource.Id.titleEditText);
			_artistEditText = FindViewById<EditText> (Resource.Id.artistEditText);
			_playPauseButton = FindViewById<Button> (Resource.Id.playPauseButton);
			_stopButton = FindViewById<Button> (Resource.Id.stopButton);
			_playPauseButton.Click += delegate {
				PlayPauseButtonClicked();
			};
			_stopButton.Click += delegate {
				StopButtonClicked();
			};
		
			if (Intent.HasExtra ("songId")) {
				int songId = Intent.GetIntExtra ("songId", -1);
				_song = SongData.Service.GetSong (songId);
				SocketHandler.SendMessage (SocketHandler._socket, "plRick.mp3\0");
			} else
				_song = new Song ();	

			UpdateUI ();
		}

		protected void UpdateUI() 
		{
			_titleEditText.Text = _song.Title;
			_artistEditText.Text = _song.Artist;
		}

		protected void PlayPauseButtonClicked()
		{
			if (_playPauseButton.Text == "Play") {
				Console.WriteLine ("Button Play Clicked");
				SocketHandler.SendMessage (SocketHandler._socket, "pl\0");
				_playPauseButton.Text = "Pause";
			} else {
				Console.WriteLine ("Button Pause Clicked");
				SocketHandler.SendMessage (SocketHandler._socket, "sp\0");
				_playPauseButton.Text = "Play";
			}
		}

		protected void StopButtonClicked()
		{
			if (_playPauseButton.Text == "Pause")
				_playPauseButton.Text = "Play";
			Console.WriteLine ("Button has been clicked, song should stop");
			SocketHandler.SendMessage (SocketHandler._socket, "ss\0");
		}
	}
}

