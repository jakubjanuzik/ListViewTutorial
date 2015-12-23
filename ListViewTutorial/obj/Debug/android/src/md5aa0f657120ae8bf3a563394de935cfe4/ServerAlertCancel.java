package md5aa0f657120ae8bf3a563394de935cfe4;


public class ServerAlertCancel
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.content.DialogInterface.OnCancelListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCancel:(Landroid/content/DialogInterface;)V:GetOnCancel_Landroid_content_DialogInterface_Handler:Android.Content.IDialogInterfaceOnCancelListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("ListViewTutorial.ServerAlertCancel, ListViewTutorial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ServerAlertCancel.class, __md_methods);
	}


	public ServerAlertCancel () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ServerAlertCancel.class)
			mono.android.TypeManager.Activate ("ListViewTutorial.ServerAlertCancel, ListViewTutorial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ServerAlertCancel (android.content.Context p0, android.app.Activity p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == ServerAlertCancel.class)
			mono.android.TypeManager.Activate ("ListViewTutorial.ServerAlertCancel, ListViewTutorial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public void onCancel (android.content.DialogInterface p0)
	{
		n_onCancel (p0);
	}

	private native void n_onCancel (android.content.DialogInterface p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
