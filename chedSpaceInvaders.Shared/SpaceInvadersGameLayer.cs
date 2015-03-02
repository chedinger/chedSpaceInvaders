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
		private List<CCSprite> shots;

		private MeteroiteProvider meteoriteProvider;
		private StarsProvider starProvider;

		public SpaceInvadersGameLayer ()
		{
			spriteSheet = new CCSpriteSheet ("animations/spaceSprites.plist");

			meteoriteProvider = new MeteroiteProvider (spriteSheet);
			starProvider = new StarsProvider (spriteSheet);
			lifes = new List<CCSprite> ();
			shots = new List<CCSprite> ();

			AddScrollingBackground ();
			AddScoreStarAndLabel ();
			AddLifes ();
			AddGalaxy ();

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
			spaceShip.Position = new CCPoint (350, 600);
			spaceShip.Scale = 0.25f;
			AddChild (spaceShip);

			spaceShip.RunAction (
				new CCScaleTo (2f, 1f)
			);

			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = OnTouchesEnded;
			AddEventListener (touchListener, this);
		}

		private int tabCount;

		private void OnTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
		{
			spaceShip.StopAllActions ();

			if (tabCount.Equals (1)) {
				this.RunActions (new CCDelayTime (0.2f), new CCCallFunc (() => tabCount = 0));
			} else if (tabCount.Equals (2)) {
				FireShot ();
			}

			var location = touches [0].LocationOnScreen;
			location = WorldToScreenspace (location); 

			if (location.Y < MIN_SPACE_SHIP_Y_POSITION)
				location.Y = MIN_SPACE_SHIP_Y_POSITION;

			float ds = CCPoint.Distance (spaceShip.Position, location);

			var dt = ds / SPACE_SHIP_SPEED;

			var moveSpaceShip = new CCMoveTo (dt, location);
			spaceShip.RunActions (moveSpaceShip);
			tabCount++;
		}

		private void FireShot ()
		{
			tabCount = 0;

			CCSprite newShot = new CCSprite (spriteSheet.Frames.Find (f => f.TextureFilename.Equals ("shot.png")));
			newShot.Scale = 0.25f;
			newShot.Position = new CCPoint (spaceShip.PositionX, spaceShip.PositionY + 10);
			newShot.RunActions (
				new CCMoveTo (2f, new CCPoint (newShot.Position.X, 1500)),
				new CCCallFuncN (node => node.RemoveFromParent()));

			shots.Add (newShot);
			AddChild (newShot);
		}

		private CCSprite AddOneShotMeteroite()
		{
			CCSprite meteroite = this.meteoriteProvider.GetOneShotMeteroite ();
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
			CCSprite meteoriteFire = this.meteoriteProvider.GetMeteoriteFire ();
			AddChild (meteoriteFire);

			return meteoriteFire;
		}

		private void CheckCollision ()
		{
			CheckMeteoriteShotCollision ();
			CheckMeteoriteCollision ();
			CheckStarsCollision ();
		}

		private void CheckMeteoriteCollision ()
		{
			this.meteoriteProvider.VisibleMeteorites.ForEach (m =>  {
				bool hit = m.BoundingBoxTransformedToParent.IntersectsRect (spaceShip.BoundingBoxTransformedToParent);

				if (hit) {
					this.meteoriteProvider.HitMeteorites.Add (m);
					CCSimpleAudioEngine.SharedEngine.PlayEffect ("sounds/explosion");
					Explode (m.Position);
					m.RemoveFromParent ();
					lifes[lifes.Count - 1].RemoveFromParent ();
					lifes.RemoveAt (lifes.Count - 1);
				}
			});

			this.meteoriteProvider.HitMeteorites.ForEach (m => this.meteoriteProvider.VisibleMeteorites.Remove (m));

			if (this.meteoriteProvider.HitMeteorites.Count.Equals (3))
				EndGame ();
		}

		private void CheckMeteoriteShotCollision ()
		{
			var shotsToRemove = new List<CCSprite> ();
			var meteoritesToRemove = new List<CCSprite> ();

			this.meteoriteProvider.VisibleMeteorites.ForEach (m => {
				shots.ForEach (s => {
					var meteoriteShot = s.BoundingBoxTransformedToParent.IntersectsRect (m.BoundingBoxTransformedToParent);

					if (meteoriteShot) {
						Explode (m.Position, 0.5f);
						shotsToRemove.Add (s);
						meteoritesToRemove.Add (m);
						m.RemoveFromParent ();
						s.RemoveFromParent ();
					}
				});
			});

			shotsToRemove.ForEach (s => shots.Remove (s));
			meteoritesToRemove.ForEach (m => this.meteoriteProvider.VisibleMeteorites.Remove (m));
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

		private void Explode (CCPoint point, float duration = 1f)
		{
			var explosion = new CCParticleExplosion (point);
			explosion.Duration = duration;
			explosion.TotalParticles = 100;
			explosion.StartColor = new CCColor4F (CCColor3B.Red);
			explosion.EndColor = new CCColor4F (CCColor3B.Yellow);
			explosion.AutoRemoveOnFinish = true;
			explosion.StartRadius = 1f;
			explosion.EndRadius = 4f;
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

		private void AddGalaxy ()
		{
			var galaxy = new CCParticleGalaxy (new CCPoint(350, 600));
			galaxy.Duration = 3f;
			galaxy.StartRadius = 8f;
			galaxy.EndRadius = 0f;
			galaxy.AutoRemoveOnFinish = true;
			galaxy.RunActions (
				new CCDelayTime (2f),
				new CCCallFunc (() => AddSpaceShip()),
				new CCCallFunc (() => spaceShip.RunAction (new CCMoveTo(2f, new CCPoint (350, 100)))));

			AddChild (galaxy);
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

			//spaceShip.Position = new CCPoint(VisibleBoundsWorldspace.MidX, MIN_SPACE_SHIP_Y_POSITION);

			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.NoBorder;
		}

		public static CCScene SpaceInvadersGameScene(CCWindow mainWindow)
		{
			return SceneProvider.GetScene<SpaceInvadersGameLayer> (mainWindow);
		}
	}
}

