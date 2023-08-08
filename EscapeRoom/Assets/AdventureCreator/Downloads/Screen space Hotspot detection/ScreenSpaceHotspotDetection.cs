using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

	public class ScreenSpaceHotspotDetection : MonoBehaviour
	{

		#region Variables

		[SerializeField] private Texture2D nearbyIcon = null;
		[SerializeField] private Texture2D nearesetIcon = null;
		[SerializeField] private float iconSize = 0.025f;
		[SerializeField] private float distance = 2f;
		[SerializeField] private Vector2 screenCircleCentre = new Vector2 (0.7f, 0.5f);
		[SerializeField] private float screenCircleRadius = 0.3f;
		[SerializeField] private bool showInteractionsWithClick = true;
		[SerializeField] private string unlockCursorWithMenu = "Interaction";
		[SerializeField] private bool raycastWallBlocking;

		private readonly HashSet<Hotspot> nearbyHotspots = new HashSet<Hotspot> ();
		private Hotspot nearestHotspot;
		private bool resetCustomScript;

		#endregion


		#region UnityStandards

		private void OnEnable ()
		{
			EventManager.OnMenuTurnOn += OnMenuTurnOn;
			EventManager.OnMenuTurnOff += OnMenuTurnOff;
			EventManager.OnInventorySelect += OnInventorySelect;
			EventManager.OnInventoryDeselect += OnInventoryDeselect;
		}


		private void OnDisable ()
		{
			EventManager.OnMenuTurnOn -= OnMenuTurnOn;
			EventManager.OnMenuTurnOff -= OnMenuTurnOff;
			EventManager.OnInventorySelect -= OnInventorySelect;
			EventManager.OnInventoryDeselect -= OnInventoryDeselect;

			if (resetCustomScript)
			{
				KickStarter.settingsManager.hotspotDetection = HotspotDetection.CustomScript;
			}
		}


		private void Update ()
		{
			nearbyHotspots.Clear ();
			nearestHotspot = null;

			if (KickStarter.settingsManager.hotspotDetection == HotspotDetection.CustomScript && KickStarter.stateHandler.IsInGameplay ())
			{
				if (KickStarter.playerMenus.MouseOverMenu && KickStarter.playerMenus.MouseOverMenu.title == "Inventory" && KickStarter.playerMenus.MouseOverMenu.IsOn ())
				{
					KickStarter.playerMenus.CloseInteractionMenus ();
					return;
				}
				float nearestHotspotDistance = Mathf.Infinity;

				foreach (Hotspot hotspot in KickStarter.stateHandler.Hotspots)
				{
					if (!hotspot.IsOn ()) continue;

					Vector3 worldPosition = hotspot.GetIconPosition ();
					if (Vector3.Distance (KickStarter.player.transform.position, worldPosition) > distance)
					{
						if (hotspot.interactiveBoundary == null || !hotspot.interactiveBoundary.PlayerIsPresent)
						{
							continue;
						}
					}

					Vector3 viewportPosition = KickStarter.CameraMain.WorldToViewportPoint (worldPosition);
					if (viewportPosition.z < 0f) continue;

					if (raycastWallBlocking)
					{
						RaycastHit hitInfo;
						if (Physics.Linecast (KickStarter.CameraMainTransform.position, hotspot.GetIconPosition (), out hitInfo, 1 << LayerMask.NameToLayer (KickStarter.settingsManager.hotspotLayer)))
						{
							if (hitInfo.collider && hitInfo.collider.gameObject != hotspot.gameObject)
							{
								continue;
							}
						}
					}

					float thisHotspotDistance = Vector2.Distance (viewportPosition, screenCircleCentre);
					if (thisHotspotDistance > screenCircleRadius)
					{
						nearbyHotspots.Add (hotspot);
						continue;
					}

					if (thisHotspotDistance < nearestHotspotDistance)
					{
						if (nearestHotspot)
						{
							nearbyHotspots.Add (nearestHotspot);
						}

						nearestHotspot = hotspot;
						nearestHotspotDistance = thisHotspotDistance;
					}
					else
					{
						nearbyHotspots.Add (hotspot);
					}
				}

				KickStarter.playerInteraction.SetActiveHotspot (nearestHotspot);

				if (KickStarter.settingsManager.interactionMethod == AC_InteractionMethod.ChooseHotspotThenInteraction)
				{
					if (KickStarter.playerMenus.IsInteractionMenuOn ())
					{
						if (KickStarter.settingsManager.cancelInteractions == CancelInteractions.ViaScriptOnly)
						{
							if (nearestHotspot)
							{
								if (KickStarter.playerInput.GetMouseState () == MouseState.SingleClick && !KickStarter.playerMenus.IsMouseOverInteractionMenu ())
								{
									KickStarter.playerMenus.CloseInteractionMenus ();
								}
							}
							else
							{
								KickStarter.playerMenus.CloseInteractionMenus ();
							}
						}
					}
					else
					{
						if (KickStarter.settingsManager.seeInteractions == SeeInteractions.ViaScriptOnly)
						{
							if (nearestHotspot && KickStarter.runtimeInventory.SelectedItem == null)
							{
								if ((!showInteractionsWithClick || KickStarter.playerInput.GetMouseState () == MouseState.SingleClick) && !KickStarter.playerMenus.IsMouseOverInteractionMenu () && !KickStarter.playerMenus.IsInteractionMenuOn ())
								{
									KickStarter.playerMenus.EnableInteractionMenus (nearestHotspot);
								}
							}
						}
					}
				}
			}
		}


		private void OnGUI ()
		{
			if (nearestHotspot)
			{
				GUI.DrawTexture (AdvGame.GUIBox (nearestHotspot.GetIconScreenPosition (), iconSize), nearesetIcon, ScaleMode.ScaleToFit, true, 0f);
			}

			foreach (Hotspot hotspot in nearbyHotspots)
			{
				GUI.DrawTexture (AdvGame.GUIBox (hotspot.GetIconScreenPosition (), iconSize), nearbyIcon, ScaleMode.ScaleToFit, true, 0f);
			}
		}

		#endregion


		#region PublicFunctions

		public void SwitchToMouseOverDetection ()
		{
			KickStarter.settingsManager.hotspotDetection = HotspotDetection.MouseOver;
			KickStarter.playerInput.SetInGameCursorState (false);
		}


		public void SwitchToScreenSpaceDetection ()
		{
			KickStarter.settingsManager.hotspotDetection = HotspotDetection.CustomScript;
			if (string.IsNullOrEmpty (unlockCursorWithMenu) || PlayerMenus.GetMenuWithName (unlockCursorWithMenu).IsOff ())
			{
				KickStarter.playerInput.SetInGameCursorState (true);
			}
		}

		#endregion


		#region CustomEvents

		private void OnMenuTurnOn (Menu menu, bool isInstant)
		{
			if (menu.title == unlockCursorWithMenu && KickStarter.playerInput.GetInGameCursorState () && KickStarter.settingsManager.hotspotDetection == HotspotDetection.CustomScript)
			{
				StartCoroutine (EnableCursorCo (menu));
			}
		}


		private IEnumerator EnableCursorCo (Menu menu)
		{
			yield return null;
			KickStarter.playerInput.SetInGameCursorState (false);
			Rect elementRect = menu.GetElementRect (menu.GetFirstVisibleElement (), 0);
			Vector2 mousePosition = new Vector2 (Mathf.Lerp (elementRect.xMin, elementRect.xMax, 0.1f), elementRect.center.y);
			KickStarter.playerInput.SetSimulatedCursorPosition (mousePosition);
		}


		private void OnMenuTurnOff (Menu menu, bool isInstant)
		{
			if (menu.title == unlockCursorWithMenu && !KickStarter.playerInput.GetInGameCursorState () && KickStarter.settingsManager.hotspotDetection == HotspotDetection.CustomScript)
			{
				KickStarter.playerInput.SetInGameCursorState (true);
			}
		}


		private void OnInventorySelect (InvItem invItem)
		{
			if (KickStarter.settingsManager.hotspotDetection == HotspotDetection.CustomScript)
			{
				KickStarter.settingsManager.hotspotDetection = HotspotDetection.MouseOver;
				resetCustomScript = true;
			}
		}


		private void OnInventoryDeselect (InvItem invItem)
		{
			KickStarter.settingsManager.hotspotDetection = HotspotDetection.CustomScript;
			resetCustomScript = false;
		}

		#endregion

	}

}