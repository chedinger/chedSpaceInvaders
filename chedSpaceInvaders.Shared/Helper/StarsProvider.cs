using System;
using CocosSharp;
using System.Collections.Generic;

namespace chedSpaceInvaders.Shared
{
	public class StarsProvider
	{
		private CCSpriteSheet spriteSheet;

		private CCRotateBy rotateStar = new CCRotateBy (5.0f, 360);
		private CCCallFuncN starComplete = new CCCallFuncN (node => node.RemoveFromParent ());

		public StarsProvider (CCSpriteSheet spriteSheet)
		{
			this.spriteSheet = spriteSheet;
			VisibleStars = new List<CCSprite> ();
			HitStars = new List<CCSprite> ();
		}

		#region properties

		public List<CCSprite> VisibleStars { get; set; }
		public List<CCSprite> HitStars { get; set; }

		#endregion

		public CCSprite GetStar()
		{
			CCSprite star = SelectStarFromSprite();
			star.Position = new CCPoint (CCRandom.Next(100, 650), 1500);
			star.Scale = 0.125f;

			VisibleStars.Add (star);

			return AddMovementAndRotationTo (star);
		}

		private CCSprite SelectStarFromSprite ()
		{
			return new CCSprite (spriteSheet.Frames.Find (x => x.TextureFilename.Equals ("star.png")));
		}

		private CCSprite AddMovementAndRotationTo(CCSprite star)
		{
			var moveStar = new CCMoveTo (5.0f, new CCPoint (star.Position.X, 0));

			star.RunActions (moveStar, starComplete);
			star.RepeatForever (rotateStar);

			return star;
		}

		public CCSprite GetScoreStar()
		{
			CCSprite scoreStar = SelectStarFromSprite ();
			scoreStar.Position = new CCPoint (40, 1285);
			scoreStar.Scale = 0.0625f;

			return scoreStar;
		}
	}
}