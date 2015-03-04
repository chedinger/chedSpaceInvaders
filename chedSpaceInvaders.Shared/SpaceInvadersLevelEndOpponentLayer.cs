using System;
using CocosSharp;

namespace chedSpaceInvaders.Shared
{
	public class SpaceInvadersLevelEndOpponentLayer : CCLayerColor
	{
		public SpaceInvadersLevelEndOpponentLayer ()
			: base()
		{
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			AddChild (new CCSprite("scrollingBG") {
				AnchorPoint = new CCPoint (0, 0),
				Position = new CCPoint (0,0)
			});
		}

		public static CCScene SpaceInvadersLevelEndOpponentScene (CCWindow mainWindow)
		{
			return SceneProvider.GetScene<SpaceInvadersLevelEndOpponentLayer> (mainWindow);
		}
	}
}
