
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
		//Intent songDetailIntent = new Intent (this, typeof(SongDetailActivity));
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
			} else
				_song = new Song ();	

			UpdateUI ();
		}

		protected void UpdateUI() 
		{
			_titleEditText.Text = _song.Title;
			_artistEditText.Text = _song.Artist;
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.SongDetailMenu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId) 
			{
			case Resource.Id.actionSave:
				SaveSong();
				return true;

			case Resource.Id.actionDelete:
				DeleteSong ();
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}
		}

		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			base.OnPrepareOptionsMenu (menu);

			if (!_song.Id.HasValue) {
				IMenuItem item = menu.FindItem (Resource.Id.actionDelete);
				item.SetEnabled (false);
			}
			return true;
		}

		protected void SaveSong()
		{
			bool errors = false;

			if (string.IsNullOrEmpty (_song.Artist = _artistEditText.Text)) {
				_artistEditText.Error = "Cannot be empty";
				errors = true;
			}
			else
				_artistEditText.Error =  null;

			if (string.IsNullOrEmpty (_song.Artist = _titleEditText.Text)) {
			_titleEditText.Error = "Cannot be empty";
			errors = true;
			}
			else
				_titleEditText.Error = null;
			if (!errors) {
				_song.Artist = _artistEditText.Text;
				_song.Title = _titleEditText.Text;

				SongData.Service.SaveSong (_song);
				Finish ();
			}
		}

		protected void DeleteSong()
		{
			SongData.Service.DeleteSong (_song);
			Finish ();
		}

		protected void PlayPauseButtonClicked()
		{
			if (_playPauseButton.Text == "Play") {
				Console.WriteLine ("Button Play Clicked");
				_playPauseButton.Text = "Pause";
			} else {
				Console.WriteLine ("Button Pause Clicked");
				_playPauseButton.Text = "Play";
			}
		}

		protected void StopButtonClicked()
		{
			if (_playPauseButton.Text == "Pause")
				_playPauseButton.Text = "Play";
			Console.WriteLine ("Button has been clicked, song should stop");
		}
	}
}

