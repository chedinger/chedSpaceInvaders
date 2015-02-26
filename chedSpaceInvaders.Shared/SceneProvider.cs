using System;
using CocosSharp;

namespace chedSpaceInvaders.Shared
{
	public class SceneProvider
	{
		public static CCScene GetScene<T> (CCWindow mainWindow)
			where T : new()
		{
			var scene = new CCScene (mainWindow);
			var layer = new T ();

			scene.AddChild (scene);

			return scene;
		}
	}
}

