using System;
using CocosSharp;
using CocosDenshion;

namespace chedSpaceInvaders.Shared
{
	public class SpaceInvadersStartLayer : CCLayerColor
	{
		private CCSprite space;

		public SpaceInvadersStartLayer() 
			: base()
		{
			InitializeTouchListener ();

			Color = CCColor3B.Black;

			space = new CCSprite("startupBG");
			AddChild (space);

			CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/startup");
		}

		private void  InitializeTouchListener ()
		{
			var touchListener = new CCEventListenerTouchAllAtOnce ();

			touchListener.OnTouchesEnded = 
				(touches, ccevent) => Window.DefaultDirector.ReplaceScene (SpaceInvadersGameLayer.SpaceInvadersGameScene (Window));

			AddEventListener (touchListener, this);
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			AddChild (new CCLabelTtf("Start new game!", "arial", 22) {
				Position = VisibleBoundsWorldspace.Center,
				Color = CCColor3B.White,
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				AnchorPoint = CCPoint.AnchorMiddle
			});

			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.NoBorder;

			space.Position = VisibleBoundsWorldspace.Center;
		}

		public static CCScene SpaceInvadersStartScene (CCWindow mainWindow)
		{
			return SceneProvider.GetScene<SpaceInvadersStartLayer> (mainWindow);
		}
	}
}

