using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Views;
using Android.Content;
using System;


namespace ListViewTutorial
{
	[Activity (Label = "ListViewTutorial", MainLauncher = true, Icon = "@mipmap/icon")]
	public class SongListActivity : Activity
	{
		//private List<string> exampleItems;
		//private ListView myListView;
		private ListView _songListView;
		private SongListViewAdapter _adapter;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			//SetContentView (Resource.Layout.SongList);
			//myListView = FindViewById<ListView> (Resource.Id.exampleListView);

//			exampleItems = new List<string> ();
//			exampleItems.Add ("Kuba Januzik");
//			exampleItems.Add ("Kuba Buda");
//			exampleItems.Add ("test123");
//			exampleItems.Add ("test124");
//			exampleItems.Add ("test125");
			Console.WriteLine("Starting...");
			SetContentView(Resource.Layout.SongList);
			_songListView = FindViewById<ListView> (Resource.Id.songListView);;
			_adapter = new SongListViewAdapter (this);
			//MyListViewadapter adapter = new MyListViewadapter (this, exampleItems);
			//myListView.Adapter = adapter;
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
			// setup the intent to pass the POI id to the detail view
			Console.WriteLine ("Inside SongClicked");
			Intent songDetailIntent = new Intent (this, typeof(SongDetailActivity));
			songDetailIntent.PutExtra ("songId", (int) e.Id);
			StartActivity (songDetailIntent);
			Console.WriteLine ("SongClicked Finished");
		}

	}
}


