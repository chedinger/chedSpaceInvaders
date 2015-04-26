using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Microsoft.Xna.Framework;
using CocosSharp;
using chedSpaceInvaders.Shared;

namespace chedSpaceInvaders.Android
{
	[Activity (Label = "chedSpaceInvaders.Android", AlwaysRetainTaskState = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar", LaunchMode = LaunchMode.SingleInstance, ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true, ConfigurationChanges =  ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
	public class MainActivity : AndroidGameActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			var gameApp = new CCApplication ();

			gameApp.ApplicationDelegate = new SpaceInvadersApplicationDelegate ();
			SetContentView (gameApp.AndroidContentView);
			gameApp.StartGame ();
		}
	}
}