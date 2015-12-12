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
	[Activity (Label = "ListViewTutorial", MainLauncher = true, Icon = "@mipmap/icon")]
	public class SongListActivity : Activity
	{
		private ListView _songListView;
		private SongListViewAdapter _adapter;
		private IPAddress _ip_address;
		private IPEndPoint _ip_endpoint;
		private Socket _socket;
		private static ManualResetEvent connectDone = 
			new ManualResetEvent(false);
		private static ManualResetEvent sendDone = 
			new ManualResetEvent(false);
		private static ManualResetEvent receiveDone = 
			new ManualResetEvent(false);
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView(Resource.Layout.SongList);
			_songListView = FindViewById<ListView> (Resource.Id.songListView);
			_adapter = new SongListViewAdapter (this);
			_songListView.Adapter = _adapter;
			_songListView.ItemClick += SongClicked;
			ConnectToServer ();
		}
		private void ConnectToServer() {
			try {
				_ip_address =  System.Net.IPAddress.Parse("150.254.45.248");
				_ip_endpoint = new IPEndPoint (_ip_address, 1234);
				_socket = new Socket(AddressFamily.InterNetwork, 
					SocketType.Stream, ProtocolType.Tcp);

				_socket.BeginConnect (_ip_endpoint, new AsyncCallback (ConnectCallback), _socket);

				connectDone.WaitOne ();
				SendMessage(_socket, "PASS");
				sendDone.WaitOne ();
				//_socket.Shutdown(SocketShutdown.Both);
				//_socket.Close();

			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
			}
		}
		private static void ConnectCallback(IAsyncResult ar) {
			try {
				// Retrieve the socket from the state object.
				Socket client = (Socket) ar.AsyncState;

				// Complete the connection.
				client.EndConnect(ar);

				Console.WriteLine("Socket connected to {0}",
					client.RemoteEndPoint.ToString());

				// Signal that the connection has been made.
				connectDone.Set();
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
		}

		public static void SendMessage(Socket client, String data) {
			byte[] byteData = Encoding.ASCII.GetBytes (data);
			client.BeginSend (byteData, 0, byteData.Length, 0,
				new AsyncCallback (SendCallback), client);
		}

		private static void SendCallback(IAsyncResult ar) {
			try {
				// Retrieve the socket from the state object.
				Socket client = (Socket) ar.AsyncState;

				// Complete sending the data to the remote device.
				int bytesSent = client.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to server.", bytesSent);

				// Signal that all bytes have been sent.
				sendDone.Set();
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
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
			SendMessage (_socket, "test\n");
			Intent songDetailIntent = new Intent (this, typeof(SongDetailActivity));
			songDetailIntent.PutExtra ("songId", (int) e.Id);
			StartActivity (songDetailIntent);
		}
	}
}


