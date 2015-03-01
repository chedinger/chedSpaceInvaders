using System;
using CocosSharp;
using CocosDenshion;

namespace chedSpaceInvaders.Shared
{
	public class SpaceInvadersApplicationDelegate : CCApplicationDelegate
	{
		public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
		{
			application.PreferMultiSampling = false;
			application.ContentRootDirectory = "Content";

			application.ContentSearchPaths.Add ("hd");

			CCSize winSize = mainWindow.WindowSizeInPixels;

			LoadSoundEffects ();

			mainWindow.SetDesignResolutionSize(winSize.Width, winSize.Height, CCSceneResolutionPolicy.ExactFit);
			mainWindow.RunWithScene (SpaceInvadersStartLayer.SpaceInvadersStartScene(mainWindow));
		}

		private void LoadSoundEffects ()
		{
			CCSimpleAudioEngine.SharedEngine.PreloadEffect ("sounds/startup");
			CCSimpleAudioEngine.SharedEngine.PreloadEffect ("sounds/explosion");
			CCSimpleAudioEngine.SharedEngine.PreloadEffect ("sounds/collected");
			CCSimpleAudioEngine.SharedEngine.PreloadEffect ("sounds/comet");
		}

		public override void ApplicationDidEnterBackground (CCApplication application)
		{
			application.Paused = true;
			CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic ();
		}

		public override void ApplicationWillEnterForeground (CCApplication application)
		{
			application.Paused = false;
			CCSimpleAudioEngine.SharedEngine.ResumeBackgroundMusic ();
		}
	}
}

