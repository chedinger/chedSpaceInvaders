using System;
using CocosSharp;

namespace chedSpaceInvaders.Shared
{
	public class MeteroiteProvider
	{
		private const string ONE_SHOT_METEROITE = "meteorite.png";
		private const string ONE_SHOT_METEROITE1 = "meteorite1.png";
		private const string ONE_SHOT_METEROITE2 = "meteorite2.png";

		private int currentOneShotMeteroite = 0;

		private CCSpriteSheet spriteSheet;

		private CCRotateBy rotateSpeedMeteroiteSlow = new CCRotateBy (3.0f, 360);
		private CCRotateBy rotateSpeedMeteroiteFast = new CCRotateBy (2.0f, 360);

		private CCCallFuncN meteroiteComplete = new CCCallFuncN (node => node.RemoveFromParent ());

		public MeteroiteProvider (CCSpriteSheet spriteSheet)
		{
			this.spriteSheet = spriteSheet;
		}

		public CCSprite GetOneShotMeteroite()
		{
			var mId = GetOneShotMeteroiteId ();
			var meteroite = new CCSprite (spriteSheet.Frames.Find (x => x.TextureFilename.Equals (mId)));
			meteroite.Position = new CCPoint (CCRandom.Next(100, 650), 1200);
			meteroite.Scale = 0.5f;

			return AddMovementAndRotationTo (meteroite);
		}

		private string GetOneShotMeteroiteId ()
		{
			int rest = (currentOneShotMeteroite++) % 3;

			switch (rest) 
			{
				case 0:
					return ONE_SHOT_METEROITE;

				case 1:
					return ONE_SHOT_METEROITE1;

				case 2:
					return ONE_SHOT_METEROITE2;

				default:
					return ONE_SHOT_METEROITE;
			}
		}

		private CCSprite AddMovementAndRotationTo(CCSprite meteroite)
		{
			var moveSpaceShip = new CCMoveTo (5.0f, new CCPoint (meteroite.Position.X, 0));

			meteroite.RunActions (moveSpaceShip, meteroiteComplete);
			meteroite.RepeatForever (rotateSpeedMeteroiteSlow);

			return meteroite;
		}
	}
}

