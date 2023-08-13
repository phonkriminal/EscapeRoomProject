using UnityEngine;

namespace AC.TheChamber
{

	public class ZoomHotspot : MonoBehaviour
	{

		#region Variables

		[SerializeField] private _Camera associatedCamera = null;
		[SerializeField] private bool neverZoomOut = false;
		[SerializeField] private _Camera exitOverrideCamera = null;
		private const float switchSpeedFactor = 0.5f;
		private const float minSwitchTime = 1.5f;
		private Hotspot attachedHotspot;

		#endregion


		#region UnityStandards

		private void Awake ()
		{
			attachedHotspot = GetComponent<Hotspot> ();
		}


		private void OnEnable ()
		{
			EventManager.OnHotspotInteract += OnHotspotInteract;
			EventManager.OnSwitchCamera += OnSwitchCamera;
		}

	
		private void OnDisable ()
		{
			EventManager.OnHotspotInteract -= OnHotspotInteract;
			EventManager.OnSwitchCamera -= OnSwitchCamera;
		}

		#endregion


		#region PublicFunctions

		public void ZoomOut ()
		{
			if (neverZoomOut)
			{
				return;
			}

			if (KickStarter.mainCamera.attachedCamera == associatedCamera)
			{
				if (exitOverrideCamera)
				{
					SwitchToCamera (exitOverrideCamera, true);
				}
				else
				{
					SwitchToCamera (attachedHotspot.limitToCamera, true);
				}
			}
			else if (ZoomInput.Instance.ActiveZoomHotspot == this)
			{
				ZoomInput.Instance.SetActiveZoomHotspot (null);
			}
		}

		#endregion


		#region CustomEvents

		private void OnHotspotInteract (Hotspot hotspot, Button button)
		{
			if (hotspot == attachedHotspot && hotspot.GetButtonInteractionType (button) == HotspotInteractionType.Use && attachedHotspot.limitToCamera && KickStarter.mainCamera.attachedCamera == attachedHotspot.limitToCamera)
			{
				SwitchToCamera (associatedCamera);
			}
		}


		private void OnSwitchCamera (_Camera fromCamera, _Camera toCamera, float transitionTime)
		{
			if (toCamera == associatedCamera)
			{
				ZoomInput.Instance.SetActiveZoomHotspot (this);
			}
		}

		#endregion


		#region PrivateFunctions

		private void SwitchToCamera (_Camera _camera, bool isExitTransition = false)
		{
			ZoomInput.Instance.SetActiveZoomHotspot (null);

			float distance = Vector3.Distance (_camera.transform.position, KickStarter.CameraMain.transform.position);
			float switchTime = Mathf.Max (distance * switchSpeedFactor, minSwitchTime);
			if (isExitTransition)
			{
				switchTime *= 0.8f;
			}

			float waitTime = switchTime * 0.8f;

			ActionListAsset asset = ZoomInput.SwitchCameraActionList;
			asset.GetParameter (0).SetValue (_camera.gameObject);
			asset.GetParameter (1).SetValue (switchTime);
			asset.GetParameter (2).SetValue (waitTime);
			asset.Interact ();
		}

		#endregion


		#region GetSet

		public bool NeverZoomOut
		{
			get
			{
				return neverZoomOut;
			}
		}

		#endregion

	}

}