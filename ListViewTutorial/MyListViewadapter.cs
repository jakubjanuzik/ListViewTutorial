using System;

using Android.Views;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;

namespace ListViewTutorial
{
	public class MyListViewadapter: BaseAdapter<string>
	{	
		private List<string> mItems;
		private Context mContext;

		public MyListViewadapter(Context context, List<string> items)
		{
			mItems = items;
			mContext = context;
		}

		public override int Count {
			get {
				return mItems.Count;
			}
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override string this[int position] {
			get {
				return mItems [position];
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View row = convertView;

			if (row == null) {
				row = LayoutInflater.From (mContext).Inflate(Resource.Layout.listview_row, null, false);
			}

			TextView txtName = row.FindViewById<TextView> (Resource.Id.txtName);
			txtName.Text = mItems [position];

			return row;
		}
	}
}

