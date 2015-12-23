using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;

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
				_socket = new Socket(AddressFamily.InterNetwork, 
					SocketType.Stream, ProtocolType.Tcp);

				_socket.BeginConnect (ip_endpoint, new AsyncCallback (ConnectCallback), _socket);
				connectDone.WaitOne ();
				Console.WriteLine("Sending Password");
				SendMessage(_socket, password);
				sendDone.WaitOne ();
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
	}
}

