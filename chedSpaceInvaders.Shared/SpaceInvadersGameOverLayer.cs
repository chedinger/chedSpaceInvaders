using System;
using CocosSharp;

namespace chedSpaceInvaders.Shared
{
	public class SpaceInvadersGameOverLayer : CCLayerColor
	{
		private static int score;
		private static int bonus;
		private CCSprite space;
		private string scoreMessage;

		public SpaceInvadersGameOverLayer ()
		{
			var touchListener = new CCEventListenerTouchAllAtOnce ();

			touchListener.OnTouchesEnded = 
				(touches, ccevent) => Window.DefaultDirector.ReplaceScene (SpaceInvadersGameLayer.SpaceInvadersGameScene (Window));

			AddEventListener (touchListener, this);

			if (bonus.Equals (0))
				scoreMessage = String.Format ("Game Over. You collected {0} stars!", score);
			else
				scoreMessage = String.Format ("Congratulations!\nYou collected {0} stars and gained a bonus of {1}!", score, bonus);

			Color = new CCColor3B (CCColor4B.Black);
			space = new CCSprite("startupBG");
			AddChild (space);

			Opacity = 255;
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			space.Scale = 2;
			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.NoBorder;

			space.Position = VisibleBoundsWorldspace.Center;

			var scoreLabel = new CCLabelTtf (scoreMessage, "arial", 22) {
				Position = new CCPoint (VisibleBoundsWorldspace.Size.Center.X, VisibleBoundsWorldspace.Size.Center.Y + 50),
				Color = new CCColor3B (CCColor4B.White),
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				AnchorPoint = CCPoint.AnchorMiddle,
				Dimensions = ContentSize
			};

			AddChild (scoreLabel);

			var playAgainLabel = new CCLabelTtf ("Tap to Play Again", "arial", 22) {
				Position = VisibleBoundsWorldspace.Size.Center,
				Color = new CCColor3B (CCColor4B.White),
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				AnchorPoint = CCPoint.AnchorMiddle,
				Dimensions = ContentSize
			};

			AddChild (playAgainLabel);
		}

		public static CCScene SpaceInvadersGameOverScene (CCWindow mainWindow, int scoreFromGame, int bonusFromGame)
		{
			score = scoreFromGame;
			bonus = bonusFromGame;
			return SceneProvider.GetScene<SpaceInvadersGameOverLayer> (mainWindow);
		}
	}
}

