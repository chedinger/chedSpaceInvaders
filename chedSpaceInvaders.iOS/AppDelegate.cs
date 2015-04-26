using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using CocosSharp;
using chedSpaceInvaders.Shared;

namespace chedSpaceInvaders.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public override void FinishedLaunching (UIApplication application)
		{
			var gameApp = new CCApplication ();
			gameApp.ApplicationDelegate = new SpaceInvadersApplicationDelegate ();
			gameApp.StartGame ();
		}
	}
}