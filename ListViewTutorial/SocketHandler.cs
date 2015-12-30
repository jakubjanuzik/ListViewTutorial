using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ListViewTutorial
{
	public class SocketHandler
	{
		private static Socket socket;
		private static ManualResetEvent connectDone = 
			new ManualResetEvent(false);
		private static ManualResetEvent sendDone = 
			new ManualResetEvent(false);
		private static ManualResetEvent receiveDone = 
			new ManualResetEvent(false);
		public const int BufferSize = 16;
		static string server_message = "Client accepted" + Char.MinValue;

		public static Socket _socket {
			get {
				return socket;
			}
			set {
				socket = value;
			}
		}
		public static void ConnectToServer(IPEndPoint ip_endpoint, string password) {
			try {
				Console.WriteLine("Connecting...");
				byte[] bytes = new byte[16];
				_socket = new Socket(AddressFamily.InterNetwork, 
					SocketType.Stream, ProtocolType.Tcp);

				_socket.BeginConnect (ip_endpoint, new AsyncCallback (ConnectCallback), _socket);
				connectDone.WaitOne ();
				Console.WriteLine("Sending Password");
				SendMessage(_socket, password);
				sendDone.WaitOne ();

				int bytesRec = _socket.Receive(bytes);
				if (Encoding.Default.GetString(bytes, 0, bytesRec) != server_message) {
					Console.WriteLine("Incorrect message, received {0}", Encoding.UTF8.GetString(bytes,0,bytesRec));
					Console.WriteLine(server_message == Encoding.UTF8.GetString(bytes, 0, bytesRec));
					Console.WriteLine(Encoding.UTF8.GetString(bytes,0,bytesRec));
					throw new Exception();
				} 
				//socket.Shutdown(SocketShutdown.Both);
				//socket.Close();
				Console.WriteLine("Finished Connection");
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
		public static List<Song> GetListFromServer ()
		{			
			string jsonString;
			int size = 8;
			byte[] buffer = new byte[size];
			int received = 0;
			StringBuilder strBuild = new StringBuilder ("");
			List<Song> songs = new List<Song> ();


			Console.WriteLine("Reading from server");
			byte[] byteData = Encoding.ASCII.GetBytes ("ll\0");
			SocketHandler.socket.BeginSend (byteData, 0, byteData.Length, 0,
				new AsyncCallback (SendCallback), SocketHandler.socket);
			Console.WriteLine("Sent Message, waiting for JSON");

			//	NetworkStream netStream = new NetworkStream (SocketHandler.socket);
			//bytesReadNum = netStream.Read (bytes, offset, size);
			//Console.WriteLine ("Read " + System.Text.Encoding.UTF8.GetString (bytes));
			//do {
			//	Console.WriteLine ("Read from server" + System.Text.Encoding.UTF8.GetString (bytes));
			//	jsonString += System.Text.Encoding.UTF8.GetString (bytes);
			//	Array.Clear (bytes, 0, bytes.Length);
			//	offset += bytesReadNum;
			//	Console.WriteLine ("Message is: " + jsonString);
			//	bytesReadNum = netStream.Read (bytes, offset, size);
		//
		//	} while (bytesReadNum != 0);,
			socket.ReceiveTimeout = 1000;
			List<byte> responseBytes =	new List<byte>();
			try {
				do {
					received = socket.Receive(buffer);
					Console.WriteLine ("Read from server" + System.Text.Encoding.UTF8.GetString (buffer));
					//strBuild.Append(System.Text.Encoding.ASCII.GetString (bytes));
					responseBytes.AddRange(buffer.Take(received));
					Console.WriteLine ("Message is: " + responseBytes);
					Console.WriteLine ("received " + received);
				} while (received > 0);
			} catch (SocketException ex) {
				Console.WriteLine ("exception!");
				Console.WriteLine (ex.ToString ());
			}

	
			jsonString = System.Text.Encoding.ASCII.GetString(responseBytes.ToArray());
			jsonString = jsonString.Remove(jsonString.Length - 2); // Small fix for last characters
			Console.WriteLine ("Json string is: " + jsonString);
			JObject json = JObject.Parse (jsonString);

			foreach (JToken track in json["tracks"].Children()) {
				Song song = JsonConvert.DeserializeObject<Song> (track.ToString ());
				songs.Add (song);
			}
			return songs;
		}
	}
}

