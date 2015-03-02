using System;
using CocosSharp;
using System.Collections.Generic;
using CocosDenshion;

namespace chedSpaceInvaders.Shared
{
	public class SpaceInvadersGameLayer : CCLayerColor
	{
		private const string SCROLLING_BG = "scrollingBG";
		private const float SPACE_SHIP_SPEED = 400.0f;
		private const int MIN_SPACE_SHIP_Y_POSITION = 100;
		private const string METEROITE_SPRITE_ID = "meteorite";

		private CCSpriteSheet spriteSheet;
		private CCSprite bgPart1;
		private CCSprite bgPart2;
		private CCSprite spaceShip;
		private CCLabelTtf scoreLabel;
		private List<CCSprite> lifes;

		private MeteroiteProvider meteroiteProvider;
		private StarsProvider starProvider;

		public SpaceInvadersGameLayer ()
		{
			spriteSheet = new CCSpriteSheet ("animations/spaceSprites.plist");

			meteroiteProvider = new MeteroiteProvider (spriteSheet);
			starProvider = new StarsProvider (spriteSheet);
			lifes = new List<CCSprite> ();

			AddScrollingBackground ();
			AddSpaceShip ();
			AddScoreStarAndLabel ();
			AddLifes ();

			StartSchedules ();
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
			Schedule (t => Scroll (new CCDelayTime (1000f)));
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

		private void AddSpaceShip ()
		{
			spaceShip = new CCSprite (spriteSheet.Frames.Find ((x) => x.TextureFilename.StartsWith ("spaceship")));
			AddChild (spaceShip);

			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = OnTouchesEnded;
			AddEventListener (touchListener, this);
		}

		private void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
		{
			spaceShip.StopAllActions ();

			var location = touches [0].LocationOnScreen;
			location = WorldToScreenspace (location); 

			if (location.Y < MIN_SPACE_SHIP_Y_POSITION)
				location.Y = MIN_SPACE_SHIP_Y_POSITION;

			float ds = CCPoint.Distance (spaceShip.Position, location);

			var dt = ds / SPACE_SHIP_SPEED;

			var moveSpaceShip = new CCMoveTo (dt, location);
			spaceShip.RunActions (moveSpaceShip);
		}

		private CCSprite AddOneShotMeteroite()
		{
			CCSprite meteroite = this.meteroiteProvider.GetOneShotMeteroite ();
			AddChild (meteroite);

			return meteroite;
		}

		private void StartSchedules ()
		{
			Schedule (t => AddOneShotMeteroite(), 3f);
			Schedule (t => AddMeteoriteFire (), 15f);
			Schedule (t => AddStar (), 8f);
			Schedule (t => CheckCollision());
		}

		private CCSprite AddMeteoriteFire ()
		{
			CCSprite meteoriteFire = this.meteroiteProvider.GetMeteoriteFire ();
			AddChild (meteoriteFire);

			return meteoriteFire;
		}

		private void CheckCollision ()
		{
			CheckMeteoriteCollision ();
			CheckStarsCollision ();
		}

		private void CheckMeteoriteCollision ()
		{
			this.meteroiteProvider.VisibleMeteorites.ForEach (m =>  {
				bool hit = m.BoundingBoxTransformedToParent.IntersectsRect (spaceShip.BoundingBoxTransformedToParent);

				if (hit) {
					this.meteroiteProvider.HitMeteorites.Add (m);
					CCSimpleAudioEngine.SharedEngine.PlayEffect ("sounds/explosion");
					Explode (m.Position);
					m.RemoveFromParent ();
					lifes[lifes.Count - 1].RemoveFromParent ();
					lifes.RemoveAt (lifes.Count - 1);
				}
			});

			this.meteroiteProvider.HitMeteorites.ForEach (m => this.meteroiteProvider.VisibleMeteorites.Remove (m));

			if (this.meteroiteProvider.HitMeteorites.Count.Equals (3))
				EndGame ();
		}

		private void CheckStarsCollision ()
		{
			this.starProvider.VisibleStars.ForEach (s => {
				bool hit = s.BoundingBoxTransformedToParent.IntersectsRect (spaceShip.BoundingBoxTransformedToParent);

				if (hit) {
					this.starProvider.HitStars.Add (s);
					CCSimpleAudioEngine.SharedEngine.PlayEffect ("sounds/collected");
					s.RemoveFromParent ();
					scoreLabel.Text = string.Format("{0}", this.starProvider.HitStars.Count);
				}
			});

			this.starProvider.HitStars.ForEach (hitStar => this.starProvider.VisibleStars.Remove (hitStar));
		}

		private void Explode (CCPoint point)
		{
			var explosion = new CCParticleExplosion (point);
			explosion.TotalParticles = 100;
			explosion.StartColor = new CCColor4F (CCColor3B.Red);
			explosion.EndColor = new CCColor4F (CCColor3B.Yellow);
			explosion.AutoRemoveOnFinish = true;
			AddChild (explosion);
		}

		private void EndGame ()
		{
			var spaceInvadersGameOverScene = SpaceInvadersGameOverLayer.SpaceInvadersGameOverScene (Window, this.starProvider.HitStars.Count);
			var transitionToGameOver = new CCTransitionMoveInR (0.3f, spaceInvadersGameOverScene);
			Director.ReplaceScene (transitionToGameOver);
		}

		private void AddScoreStarAndLabel ()
		{
			AddChild (this.starProvider.GetScoreStar ());
			scoreLabel = new CCLabelTtf ("0", "arial", 22);
			scoreLabel.Position = new CCPoint (70, 1300);
			scoreLabel.AnchorPoint = CCPoint.AnchorUpperLeft;
			AddChild (scoreLabel);
		}

		private void AddLifes ()
		{
			lifes.Add (CreateLifeSpaceShip ());
			lifes.Add (CreateLifeSpaceShip (addToXPosition: 50));
			lifes.Add (CreateLifeSpaceShip (addToXPosition: 100));

			lifes.ForEach (l => AddChild (l));
		}

		private CCSprite CreateLifeSpaceShip (int addToXPosition = 0)
		{
			return new CCSprite (spriteSheet.Frames.Find ((x) => x.TextureFilename.StartsWith ("spaceship"))) 
			{
				Position = new CCPoint (40 + addToXPosition, 1210),
				Scale = 0.5f
			};
		}

		private CCSprite AddStar ()
		{
			CCSprite star = this.starProvider.GetStar ();
			AddChild (star);

			return star;
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			spaceShip.Position = new CCPoint(VisibleBoundsWorldspace.MidX, MIN_SPACE_SHIP_Y_POSITION);

			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.NoBorder;
		}

		public static CCScene SpaceInvadersGameScene(CCWindow mainWindow)
		{
			return SceneProvider.GetScene<SpaceInvadersGameLayer> (mainWindow);
		}
	}
}

