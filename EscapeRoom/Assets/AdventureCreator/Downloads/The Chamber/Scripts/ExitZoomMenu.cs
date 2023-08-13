using UnityEngine;

namespace AC.TheChamber
{

	public class ExitZoomMenu : MonoBehaviour
	{

		#region Variables

		[SerializeField] private string exitZoomMenuName = "ExitZoom";
		[SerializeField] private string itemCloseUpMenuName = "ItemCloseUp";
		private Menu exitZoomMenu;

		#endregion


		#region UnityStandards

		private void Start ()
		{
			if (!string.IsNullOrEmpty (exitZoomMenuName))
			{
				exitZoomMenu = PlayerMenus.GetMenuWithName (exitZoomMenuName);
			}
		}


		private void OnEnable ()
		{
			EventManager.OnMenuElementClick += OnMenuElementClick;
			EventManager.OnMenuTurnOn += OnMenuTurnOn;
			EventManager.OnMenuTurnOff += OnMenuTurnOff;
		}


		private void OnDisable ()
		{
			EventManager.OnMenuTurnOff -= OnMenuTurnOff;
			EventManager.OnMenuTurnOn -= OnMenuTurnOn;
			EventManager.OnMenuElementClick -= OnMenuElementClick;
		}

		#endregion


		#region PublicFunctions

		public void OnSetActiveZoomHotspot (ZoomHotspot zoomHotspot)
		{
			if (exitZoomMenu != null)
			{
				if (zoomHotspot == null || zoomHotspot.NeverZoomOut)
				{
					exitZoomMenu.isLocked = true;
					exitZoomMenu.TurnOff ();
				}
				else
				{
					exitZoomMenu.isLocked = false;
					exitZoomMenu.TurnOn ();
				}
			}
		}

		#endregion


		#region CustomEvents

		private void OnMenuElementClick (Menu menu, MenuElement element, int slot, int buttonPressed)
		{
			if (menu == exitZoomMenu && ZoomInput.Instance.ActiveZoomHotspot)
			{
				ZoomInput.Instance.ActiveZoomHotspot.ZoomOut ();
			}
		}


		private void OnMenuTurnOn (Menu menu, bool isInstant)
		{
			if (menu.title == itemCloseUpMenuName)
			{
				exitZoomMenu.isLocked = true;
				exitZoomMenu.TurnOff ();
			}
		}


		private void OnMenuTurnOff (Menu menu, bool isInstant)
		{
			if (menu.title == itemCloseUpMenuName)
			{
				OnSetActiveZoomHotspot (ZoomInput.Instance.ActiveZoomHotspot);
			}
		}

		#endregion

	}

}