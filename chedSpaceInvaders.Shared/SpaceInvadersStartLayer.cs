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

			touchListener.OnTouchesEnded = (touches, ccevent) => 
			{ 
				CCSimpleAudioEngine.SharedEngine.StopAllEffects ();
				Window.DefaultDirector.ReplaceScene (SpaceInvadersGameLayer.SpaceInvadersGameScene (Window));
			}; 

			AddEventListener (touchListener, this);
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			space.Scale = 2;
			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.NoBorder;

			space.Position = VisibleBoundsWorldspace.Center;

			AddChild (new CCLabelTtf("Start new game!", "arial", 22) {
				Position = VisibleBoundsWorldspace.Center,
				Color = CCColor3B.White,
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				AnchorPoint = CCPoint.AnchorMiddle
			});
		}

		public static CCScene SpaceInvadersStartScene (CCWindow mainWindow)
		{
			return SceneProvider.GetScene<SpaceInvadersStartLayer> (mainWindow);
		}
	}
}

