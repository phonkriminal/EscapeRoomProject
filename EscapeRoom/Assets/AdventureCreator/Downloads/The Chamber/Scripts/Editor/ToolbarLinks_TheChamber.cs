#if UNITY_EDITOR

using UnityEditor;

namespace AC
{

	public class ToolbarLinks_TheChamber : EditorWindow
	{

		[MenuItem ("Adventure Creator/Getting started/Load 'The Chamber'", false, 5)]
		static void TheChamber ()
		{
			ManagerPackage package = AssetDatabase.LoadAssetAtPath ("Assets/AdventureCreator/Downloads/The Chamber/TheChamber_ManagerPackage.asset", typeof (ManagerPackage)) as ManagerPackage;
			if (package != null)
			{
				package.AssignManagers ();
				AdventureCreator.RefreshActions ();

				if (!ACInstaller.IsInstalled ())
				{
					ACInstaller.DoInstall ();
				}

				if (UnityVersionHandler.GetCurrentSceneName () != "TheChamber")
				{
					bool canProceed = EditorUtility.DisplayDialog ("Open scene", "Would you like to open the Chamber scene, TheChamber, now?", "Yes", "No");
					if (canProceed)
					{
						if (UnityVersionHandler.SaveSceneIfUserWants ())
						{
							UnityEditor.SceneManagement.EditorSceneManager.OpenScene ("Assets/AdventureCreator/Downloads/The Chamber/TheChamber.unity");
						}
					}
				}

				AdventureCreator.Init ();
			}
		}

	}

}

#endif