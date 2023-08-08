using System;
using UnityEngine;
using UnityEngine.UI;

namespace AC
{

	public class GraphicOptionsURP : MonoBehaviour
	{

		#region Variables

		[SerializeField] private string globalStringVariable = "GraphicOptionsData";
		[SerializeField] private Dropdown resolutionDropdown = null;
		[SerializeField] private Toggle fullScreenToggle = null;
		[SerializeField] private Dropdown qualityPresetDropdown = null;
		[SerializeField] private Dropdown antiAliasingDropdown = null;
		[SerializeField] private Dropdown textureQualityDropdown = null;
		[SerializeField] private Dropdown vSyncDropdown = null;
		private int nonCustomQualityLevel;

		#endregion


		#region UnityStandards

		private void OnEnable ()
		{
			UpdateUIValues ();
		}

		#endregion


		#region PublicFunctions

		public void SaveAndApply ()
		{
			GVar gVar = GlobalVariables.GetVariable (globalStringVariable);
			if (gVar != null && gVar.type == VariableType.String)
			{
				bool usingAdvancedOptions = qualityPresetDropdown.value == qualityPresetDropdown.options.Count - 1;
				GraphicOptionsDataURP graphicOptionsData = new GraphicOptionsDataURP (resolutionDropdown.value, fullScreenToggle.isOn, usingAdvancedOptions ? nonCustomQualityLevel : qualityPresetDropdown.value, usingAdvancedOptions, antiAliasingDropdown.value, textureQualityDropdown.value, vSyncDropdown.value);
				gVar.TextValue = JsonUtility.ToJson (graphicOptionsData);
				Options.SavePrefs ();
			}
			else
			{
				ACDebug.LogWarning ("Could not apply Graphic Options data because no Global String variable was found", this);
			}

			Apply ();
		}


		public void Apply ()
		{
			GraphicOptionsDataURP graphicOptionsData = GetSaveData ();
			if (graphicOptionsData != null)
			{
				graphicOptionsData.Apply ();
			}
		}


		public void OnSetAdvancedOption ()
		{
			SetDropdownValue (qualityPresetDropdown, qualityPresetDropdown.options.Count - 1);
		}


		public void OnSetQualityPreset ()
		{
			if (qualityPresetDropdown.value <= qualityPresetDropdown.options.Count - 1)
			{
				QualitySettings.SetQualityLevel (qualityPresetDropdown.value, false);
				UpdateAdvancedUIValues ();
				QualitySettings.SetQualityLevel (nonCustomQualityLevel, false);

				nonCustomQualityLevel = qualityPresetDropdown.value;
			}
		}


		public static int QualityIndexToLevel (int index)
		{
			return (int) Mathf.Pow (2, index);
		}


		public static int QualityLevelToIndex (int level)
		{
			switch (level)
			{
				case 0:
				default:
					return 0;

				case 2:
					return 1;

				case 4:
					return 2;

				case 8:
					return 3;
			}
		}

		#endregion


		#region PrivateFunctions

		private void UpdateUIValues ()
		{
			// Advanced options
			GraphicOptionsDataURP graphicOptionsData = GetSaveData ();
			bool usingAdvancedOptions = (graphicOptionsData != null) ? graphicOptionsData.UsingAdvancedOptions : false;

			// Resolution
			if (resolutionDropdown)
			{
				resolutionDropdown.options.Clear ();
				int resolutionIndex = -1;
				for (int i = 0; i < Screen.resolutions.Length; i++)
				{
					if (Screen.resolutions[i].width == Screen.width &&
						Screen.resolutions[i].height == Screen.height &&
						Screen.resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
					{
						resolutionIndex = i;
					}

					string label = Screen.resolutions[i].width.ToString () + " x " + Screen.resolutions[i].height.ToString () + " " + Screen.resolutions[i].refreshRate.ToString () + " hz";
					resolutionDropdown.options.Add (new Dropdown.OptionData (label));
				}
				resolutionDropdown.RefreshShownValue ();
				if (resolutionIndex >= 0) SetDropdownValue (resolutionDropdown, resolutionIndex);
			}

			// Full-screen
			if (fullScreenToggle)
			{
				fullScreenToggle.isOn = Screen.fullScreen;
			}

			// Quality preset
			if (qualityPresetDropdown)
			{
				qualityPresetDropdown.options.Clear ();
				foreach (string qualityName in QualitySettings.names)
				{
					qualityPresetDropdown.options.Add (new Dropdown.OptionData (qualityName));
				}
				qualityPresetDropdown.options.Add (new Dropdown.OptionData ("Custom"));
				qualityPresetDropdown.RefreshShownValue ();
				if (usingAdvancedOptions)
				{
					SetDropdownValue (qualityPresetDropdown, qualityPresetDropdown.options.Count - 1);
				}
				else
				{
					SetDropdownValue (qualityPresetDropdown, QualitySettings.GetQualityLevel ());
				}
				nonCustomQualityLevel = QualitySettings.GetQualityLevel ();
			}

			UpdateAdvancedUIValues ();
		}
		

		private void UpdateAdvancedUIValues ()
		{
			// Anti-aliasing
			int antiAliasingValue = QualityLevelToIndex (QualitySettings.antiAliasing);
			SetDropdownValue (antiAliasingDropdown, antiAliasingValue);

			// Texture quality
			SetDropdownValue (textureQualityDropdown, QualitySettings.masterTextureLimit);

			// Vsync
			SetDropdownValue (vSyncDropdown, QualitySettings.vSyncCount);
		}


		private GraphicOptionsDataURP GetSaveData ()
		{
			GVar gVar = GlobalVariables.GetVariable (globalStringVariable);
			if (gVar != null && gVar.type == VariableType.String)
			{
				string optionsDataString = gVar.TextValue;
				if (!string.IsNullOrEmpty (optionsDataString))
				{
					GraphicOptionsDataURP graphicOptionsData = JsonUtility.FromJson<GraphicOptionsDataURP> (optionsDataString);
					return graphicOptionsData;
				}
				return null;
			}
			else
			{
				ACDebug.LogWarning ("Could not apply Graphic Options data because no Global String variable was found", this);
				return null;
			}
		}


		private void SetDropdownValue (Dropdown dropdown, int value)
		{
			if (dropdown)
			{
				dropdown.SetValueWithoutNotify (value);
			}
		}

		#endregion

	}


	[Serializable]
	public class GraphicOptionsDataURP
	{

		#region Variables

		[SerializeField] private int screenResolutionIndex;
		[SerializeField] private bool isFullScreen;
		[SerializeField] private int qualityPresetIndex;
		[SerializeField] private bool usingAdvancedOptions;
		[SerializeField] private int antiAliasingLevel;
		[SerializeField] private int textureQualityLevel;
		[SerializeField] private int vSyncCount;

		#endregion


		#region Constructors

		public GraphicOptionsDataURP (int _screenResolutionIndex, bool _isFullScreen, int _qualityPresetIndex, bool _usingAdvancedOptions, int _antiAliasingLevel, int _textureQualityLevel, int _vSyncCount)
		{
			screenResolutionIndex = _screenResolutionIndex;
			isFullScreen = _isFullScreen;
			qualityPresetIndex = _qualityPresetIndex;
			usingAdvancedOptions = _usingAdvancedOptions;
			antiAliasingLevel = _antiAliasingLevel;
			textureQualityLevel = _textureQualityLevel;
			vSyncCount = _vSyncCount;
		}

		#endregion


		#region PublicFunctions

		public void Apply ()
		{
			Resolution chosenResolution = Screen.resolutions[screenResolutionIndex];
			Screen.SetResolution (chosenResolution.width, chosenResolution.height, isFullScreen);
			QualitySettings.SetQualityLevel (qualityPresetIndex, true);
			if (usingAdvancedOptions)
			{
				QualitySettings.antiAliasing = GraphicOptionsURP.QualityIndexToLevel (antiAliasingLevel);
				QualitySettings.masterTextureLimit = textureQualityLevel;
				QualitySettings.vSyncCount = vSyncCount;
			}

			KickStarter.playerMenus.RecalculateAll ();
		}

		#endregion


		#region GetSet

		public bool UsingAdvancedOptions { get { return usingAdvancedOptions; } }

		#endregion

	}

}