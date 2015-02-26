using System;
using CocosSharp;

namespace chedSpaceInvaders.Shared
{
	public class SpaceInvadersGameLayer : CCLayerColor
	{
		public SpaceInvadersGameLayer ()
		{
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();
		}

		public static CCScene SpaceInvadersGameScene(CCWindow mainWindow)
		{
			return SceneProvider.GetScene<SpaceInvadersGameLayer> (mainWindow);
		}
	}
}

