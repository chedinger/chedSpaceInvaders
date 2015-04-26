using System;
using CocosSharp;
using CocosDenshion;

namespace chedSpaceInvaders.Shared
{
	public class ChedSpaceInvadersStartLayer : CCLayerColor
	{
		private CCSprite space;
		private CCSprite ufo;

		public ChedSpaceInvadersStartLayer() 
			: base()
		{
			InitializeTouchListener ();

			Color = CCColor3B.Black;

			space = new CCSprite ("startupBG");
			AddChild (space);

			AddUfo ();

			CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/startup");
		}

		private void  InitializeTouchListener ()
		{
			var touchListener = new CCEventListenerTouchAllAtOnce ();

			touchListener.OnTouchesEnded = (touches, ccevent) => 
			{ 
				CCSimpleAudioEngine.SharedEngine.StopAllEffects ();
				CCWindow tst;
				Window.DefaultDirector.ReplaceScene (ChedSpaceInvadersGameLayer.SpaceInvadersGameScene (Window));
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

		private void AddUfo ()
		{
			ufo = new CCSprite ("ufo");
			ufo.Position = new CCPoint (-100, -100);

			AddChild (ufo);

			ufo.RunActions (new [] 
			{
				new CCMoveTo (1.5f, new CCPoint (100, 200)),
				new CCMoveTo (1.5f, new CCPoint (500, 500)),
				new CCMoveTo (1.5f, new CCPoint (200, 950)),
				new CCMoveTo (1.5f, new CCPoint (360, 800))
			});
		}

		public static CCScene SpaceInvadersStartScene (CCWindow mainWindow)
		{
			return SceneProvider.GetScene<ChedSpaceInvadersStartLayer> (mainWindow);
		}
	}
}