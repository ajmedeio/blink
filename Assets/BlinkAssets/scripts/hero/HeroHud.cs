using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroHud : MonoBehaviour, IObserver {

	// Chat Variables
	public bool isTyping = false;
	public bool playerChatBubble = false;
	public string msg = "";

	private delegate void VoidFunc();
	private Dictionary<HudAction, VoidFunc> hudActionMap;

	void Start() {
		hudActionMap = new Dictionary<HudAction, VoidFunc> {
			{HudAction.ToggleTyping, ToggleTyping}
		};
	}

	void Update() {

	}

	void OnGUI() {

	}

	void IObserver.Notify(IObservable controller, object msg) {
		HashSet<HudAction> hudActions = msg as HashSet<HudAction>;
		if (hudActions != null) {
			foreach (HudAction a in hudActions) {
				hudActionMap [a] ();
			}
			DoHud();
		}
	}

	private void DoHud() {
		if (isTyping) {
			//GUI.SetNextControlName ("ChatField");
			//msg = GUI.TextField (new Rect (10, Screen.height - 32, 600, 20), msg, 25);
			//GUI.FocusControl ("ChatField");
		}

		if (msg != "") {
			print("going to the function");
			// timeSaid = Time.time;
			playerChatBubble = true;
			if (Event.current.Equals (Event.KeyboardEvent ("return")) ) {
				print("return pressed");
				isTyping = !isTyping;
			}

			if (isTyping) {
				//GUI.SetNextControlName ("ChatField"); 
				//msg = GUI.TextField (new Rect (10, Screen.height - 32, 600, 20), msg, 25);
				//GUI.FocusControl ("ChatField");
			}

			if (msg != "") {
				print("going to the function");
				playerChatBubble = true;
				// timeSaid = Time.time;
				// entering = GetComponent(TextEnter);
				// entering.timeSaid = Time.time;
				// entering.EnterText();
			}

			if(playerChatBubble){
				// entering = GetComponent(TextEnter);
				// entering.PlayerChatBubble();
			}
			// entering = GetComponent(TextEnter);
			// entering.timeSaid = Time.time;
			// entering.EnterText();
		}

		if(playerChatBubble){
			// entering = GetComponent(TextEnter);
			// entering.PlayerChatBubble();
		}
	}

	public void ToggleTyping() {
		isTyping = !isTyping;
	}

}

