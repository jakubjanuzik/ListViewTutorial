using System;
using Android.Widget;
using Android.App;
using Android.Views;

namespace ListViewTutorial
{
	public class SongListViewAdapter: BaseAdapter<Song>
	{
		private readonly Activity _context;

		public SongListViewAdapter (Activity context)
		{
			_context = context;
		}

		public override int Count
		{
			get { return SongData.Service.Songs.Count; }
		}
			
		public override long GetItemId(int position)
		{
			return SongData.Service.Songs [position].Id.Value;
		}

		public override Song this[int position]
		{
			get { return SongData.Service.Songs [position];}
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (view == null)
				view = _context.LayoutInflater.Inflate (Resource.Layout.SongListItem, null);

			Song song = SongData.Service.Songs [position];

			view.FindViewById<TextView> (Resource.Id.artistTextView).Text = song.Artist;

			view.FindViewById<TextView> (Resource.Id.nameTextView).Text = song.Title;

			return view;
		}

	}
}

