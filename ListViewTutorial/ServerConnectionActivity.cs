
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
using System.Net;

namespace ListViewTutorial
{
	[Activity (Label = "ServerConnectionActivity")]			
	public class ServerConnectionActivity : Activity
	{
		EditText _serverIpText;
		EditText _serverPortText;
		EditText _serverPasswordText;
		Button _connectButton;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.ServerConnectionLayout);
			_serverIpText = FindViewById<EditText> (Resource.Id.serverIpText);
			_serverPasswordText = FindViewById<EditText> (Resource.Id.serverPassText);
			_serverPortText = FindViewById<EditText> (Resource.Id.serverPortText);
			_connectButton = FindViewById<Button> (Resource.Id.connectServerButton);
			_connectButton.Click += delegate {
				ConnectButtonClicked();
			};

		}

		private void ConnectButtonClicked()
		{
			IPAddress ip_address = IPAddress.Parse (_serverIpText.Text);
			IPEndPoint ip_endpoint = new IPEndPoint (ip_address, Int32.Parse (_serverPortText.Text));
			string password = _serverPasswordText.Text;
			Console.WriteLine ("Connecting to {0} with password {1}",
				_serverIpText.Text, password);
			SocketHandler.ConnectToServer (ip_endpoint, password);
			// Some Try-except here, if wrong connection, return errors, else disappear
			//Intent songListActivityIntent = new Intent(this, typeof(SongListActivity));
			Console.WriteLine ("Connect button clicked");
			Finish ();
		}
	}
}

