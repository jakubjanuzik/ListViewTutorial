using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Views;
using Android.Content;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Text;

namespace ListViewTutorial
{
	[Activity (Label = "Music Client", MainLauncher = true, Icon = "@mipmap/icon")]
	public class SongListActivity : Activity
	{
		private ListView _songListView;
		private SongListViewAdapter _adapter;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			Intent serverConnectionIntent = new Intent (this, typeof(ServerConnectionActivity));
			StartActivity (serverConnectionIntent);
			SetContentView(Resource.Layout.SongList);
			_songListView = FindViewById<ListView> (Resource.Id.songListView);
			_adapter = new SongListViewAdapter (this);
			_songListView.Adapter = _adapter;
			_songListView.ItemClick += SongClicked;
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.SongListView, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.actionNew:
				StartActivity (typeof(SongDetailActivity));
				return true;

			case Resource.Id.actionRefresh:
				SongData.Service.RefreshCache ();
				_adapter.NotifyDataSetChanged ();
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}
		}

		protected void SongClicked(object sender, ListView.ItemClickEventArgs e)
		{
			Intent songDetailIntent = new Intent (this, typeof(SongDetailActivity));
			songDetailIntent.PutExtra ("songId", (int) e.Id);
			StartActivity (songDetailIntent);
		}
	}
}


