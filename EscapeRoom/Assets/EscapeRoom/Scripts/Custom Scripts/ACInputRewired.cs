using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ACInputRewired : MonoBehaviour
{
	public int playerId = 0;
	private Rewired.Player player;

	void Start()
	{
		AC.KickStarter.playerInput.InputGetButtonDownDelegate = CustomGetButtonDown;
		AC.KickStarter.playerInput.InputGetButtonUpDelegate = CustomGetButtonUp;
		AC.KickStarter.playerInput.InputGetButtonDelegate = CustomGetButton;
		AC.KickStarter.playerInput.InputGetAxisDelegate = CustomGetAxis;
		player = Rewired.ReInput.players.GetPlayer(playerId);
	}

	private bool CustomGetButtonDown(string buttonName)
	{
		return player.GetButtonDown(buttonName);
	}

	private float CustomGetAxis(string AxisName)
	{
		return player.GetAxis(AxisName);
	}

	private bool CustomGetButton(string buttonName)
	{
		return player.GetButton(buttonName);
	}

	private bool CustomGetButtonUp(string buttonName)
	{
		return player.GetButtonUp(buttonName);
	}

}