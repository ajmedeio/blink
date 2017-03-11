using System;
using UnityEngine;

public class HeroHud : MonoBehaviour {

	// Chat Variables
	public bool isTyping = false;
	public bool playerChatBubble = false;
	public string msg = "";

	void Start() {

	}

	void Update() {

	}

	void OnGUI() {

	}

	public void Notify(Hero hero) {
		if (isTyping) {
			GUI.SetNextControlName ("ChatField");
			msg = GUI.TextField (new Rect (10, Screen.height - 32, 600, 20), msg, 25);
			GUI.FocusControl ("ChatField");
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
				GUI.SetNextControlName ("ChatField"); 
				msg = GUI.TextField (new Rect (10, Screen.height - 32, 600, 20), msg, 25);
				GUI.FocusControl ("ChatField");
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

