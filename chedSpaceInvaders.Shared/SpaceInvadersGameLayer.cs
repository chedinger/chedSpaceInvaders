using System;
using CocosSharp;

namespace chedSpaceInvaders.Shared
{
	public class SpaceInvadersGameLayer : CCLayerColor
	{
		private CCSprite background1;
		private CCSprite background2;

		public SpaceInvadersGameLayer ()
		{
			background1 = new CCSprite ("scrollingBG");
			background1.AnchorPoint = new CCPoint (0, 0);
			background1.Position = new CCPoint (0, 0);
			AddChild (background1);

			background2 = new CCSprite ("scrollingBG");
			background2.AnchorPoint = new CCPoint (0, 0);
			background2.Position = new CCPoint (0, background1.BoundingBox.Size.Height);
			AddChild (background2);

			Schedule(t => Scroll(new CCDelayTime(0.01f)));
		}

		private void Scroll(CCDelayTime dt) 
		{
			background1.Position = new CCPoint (background1.Position.X, background1.Position.Y - 1);
			background2.Position = new CCPoint (background2.Position.X, background2.Position.Y - 1);

			if (background1.Position.Y < -background1.BoundingBox.Size.Height) 
				background1.Position = new CCPoint (background2.Position.X, background1.Position.Y + (background2.BoundingBox.Size.Height * 2));

			if (background2.Position.Y < -background2.BoundingBox.Size.Height) 
				background2.Position = new CCPoint (background1.Position.X, background2.Position.Y + (background1.BoundingBox.Size.Height * 2));
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.NoBorder;
		}

		public static CCScene SpaceInvadersGameScene(CCWindow mainWindow)
		{
			return SceneProvider.GetScene<SpaceInvadersGameLayer> (mainWindow);
		}
	}
}

