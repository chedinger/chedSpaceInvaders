using System;
using CocosSharp;

namespace chedSpaceInvaders.Shared
{
	public class SpaceInvadersGameLayer : CCLayerColor
	{
		private const string SCROLLING_BG = "scrollingBG";

		private CCSpriteSheet spriteSheet;
		private CCSprite bgPart1;
		private CCSprite bgPart2;
		private CCSprite spaceShip;

		public SpaceInvadersGameLayer ()
		{
			spriteSheet = new CCSpriteSheet ("animations/spaceSprites.plist");

			AddScrollingBackground ();
			AddSpaceShip ();
		}

		private void AddScrollingBackground ()
		{
			bgPart1 = new CCSprite (SCROLLING_BG);
			bgPart1.AnchorPoint = new CCPoint (0, 0);
			bgPart1.Position = new CCPoint (0, 0);
			AddChild (bgPart1);

			bgPart2 = new CCSprite (SCROLLING_BG);
			bgPart2.AnchorPoint = new CCPoint (0, 0);
			bgPart2.Position = new CCPoint (0, bgPart1.BoundingBox.Size.Height);
			AddChild (bgPart2);
			Schedule (t => Scroll (new CCDelayTime (0.01f)));
		}

		private void AddSpaceShip ()
		{
			spaceShip = new CCSprite (spriteSheet.Frames.Find ((x) => x.TextureFilename.StartsWith ("spaceship")));
			AddChild (spaceShip);
		}

		private void Scroll(CCDelayTime dt) 
		{
			bgPart1.Position = new CCPoint (bgPart1.Position.X, bgPart1.Position.Y - 1);
			bgPart2.Position = new CCPoint (bgPart2.Position.X, bgPart2.Position.Y - 1);

			if (bgPart1.Position.Y < (-bgPart1.BoundingBox.Size.Height)) 
				bgPart1.Position = new CCPoint (bgPart2.Position.X, bgPart1.Position.Y + (bgPart2.BoundingBox.Size.Height * 2));

			if (bgPart2.Position.Y < (-bgPart2.BoundingBox.Size.Height)) 
				bgPart2.Position = new CCPoint (bgPart1.Position.X, bgPart2.Position.Y + (bgPart1.BoundingBox.Size.Height * 2));
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			spaceShip.Position = new CCPoint(VisibleBoundsWorldspace.MidX, 100);

			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.NoBorder;
		}

		public static CCScene SpaceInvadersGameScene(CCWindow mainWindow)
		{
			return SceneProvider.GetScene<SpaceInvadersGameLayer> (mainWindow);
		}
	}
}

