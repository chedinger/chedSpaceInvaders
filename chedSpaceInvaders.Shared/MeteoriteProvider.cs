using System;
using CocosSharp;
using System.Collections.Generic;

namespace chedSpaceInvaders.Shared
{
	public class MeteroiteProvider
	{
		private const string ONE_SHOT_METEROITE = "meteorite.png";
		private const string ONE_SHOT_METEROITE1 = "meteorite1.png";
		private const string ONE_SHOT_METEROITE2 = "meteorite2.png";
		private const string METEORITE_FIRE = "meteoriteFire.png";

		private int currentOneShotMeteroite = 0;
		private int currentMeteoriteFire = 0;

		private CCSpriteSheet spriteSheet;

		private CCRotateBy rotateSpeedMeteroiteSlow = new CCRotateBy (3.0f, 360);
		private CCRotateBy rotateSpeedMeteroiteFast = new CCRotateBy (2.0f, 360);

		private CCCallFuncN meteroiteComplete = new CCCallFuncN (node => node.RemoveFromParent ());

		public MeteroiteProvider (CCSpriteSheet spriteSheet)
		{
			this.spriteSheet = spriteSheet;
			VisibleMeteorites = new List<CCSprite> ();
			HitMeteorites = new List<CCSprite> ();
		}

		#region properties

		public List<CCSprite> VisibleMeteorites { get; set; }
		public List<CCSprite> HitMeteorites { get; set; }

		#endregion

		public CCSprite GetOneShotMeteroite()
		{
			var mId = GetOneShotMeteroiteId ();
			var meteroite = new CCSprite (spriteSheet.Frames.Find (x => x.TextureFilename.Equals (mId)));
			meteroite.Position = new CCPoint (CCRandom.Next(100, 650), 1500);
			meteroite.Scale = 0.5f;

			VisibleMeteorites.Add (meteroite);

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
			var moveMeteorite = new CCMoveTo (5.0f, new CCPoint (meteroite.Position.X, 0));

			meteroite.RunActions (moveMeteorite, meteroiteComplete);
			meteroite.RepeatForever (rotateSpeedMeteroiteSlow);

			return meteroite;
		}

		public CCSprite GetMeteoriteFire()
		{
			currentMeteoriteFire++;
			var fromLeftToRight = (currentMeteoriteFire % 2) == 0;
			var meteoriteFire = new CCSprite (spriteSheet.Frames.Find (x => x.TextureFilename.Equals (METEORITE_FIRE)));

			meteoriteFire.Position = fromLeftToRight
				? new CCPoint (0, 1136)
				: new CCPoint (752, 1136);
			meteoriteFire.Scale = 0.25f;

			var meteoriteFireMove = new CCMoveTo(5.0f, (currentMeteoriteFire % 2) == 0
				? new CCPoint (752, -100)
				: new CCPoint (-100, -100));

			meteoriteFire.RunAction (meteoriteFireMove);

			if ( ! fromLeftToRight)
				meteoriteFire.Rotation = 90;

			return meteoriteFire;
		}
	}
}

