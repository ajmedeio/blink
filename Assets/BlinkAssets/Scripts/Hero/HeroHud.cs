using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class HeroHud : NetworkBehaviour, IObserver {

	private HeroManager hero;
	private HeroCombat heroCombat;

	private GameObject deathPanel;
    private Button resurrectBtn;

	private GameObject heroStatusBar;
	private TextMeshProUGUI heroName;
	private Image health;
	private TextMeshProUGUI healthText;

	private GameObject targetStatusBar;
	private TextMeshProUGUI targetName;
	private Image targetHealth;
	private TextMeshProUGUI targetHealthText;

	// Chat Variables
	public bool isTyping = false;
	public bool playerChatBubble = false;
	public string msg = "";

	private delegate void VoidFunc();
	private Dictionary<HudAction, VoidFunc> hudActionMap;

	void Start() {
        hero = GetComponent<HeroManager>();
		Transform heroHudTransform = transform.FindChild("HeroHud");
		if (!isLocalPlayer) return;
		
        // activate self, only on local hero's screen
        heroHudTransform.gameObject.SetActive (true);
        heroHudTransform.FindChild("EventSystem").gameObject.SetActive(true);
        

		hudActionMap = new Dictionary<HudAction, VoidFunc> {
			{HudAction.ToggleTyping, ToggleTyping}
		};

		deathPanel = transform.FindChild ("HeroHud/DeathPanel").gameObject;
        resurrectBtn = deathPanel.transform.FindChild("ResurrectBtn").GetComponent<Button>();
        resurrectBtn.onClick.AddListener(hero.heroCombat.Resurrect);

		// find all the hud objects on the screen
		heroStatusBar = transform.FindChild ("HeroHud/HeroStatusBar").gameObject;
		heroName = heroStatusBar.transform.FindChild ("HeroName").GetComponent<TextMeshProUGUI>();
		health = heroStatusBar.transform.FindChild ("HealthBar/Health").GetComponent<Image>();
		healthText = heroStatusBar.transform.FindChild ("HealthBar/HealthText").GetComponent<TextMeshProUGUI>();

		targetStatusBar = transform.FindChild ("HeroHud/TargetStatusBar").gameObject;
		targetStatusBar.SetActive (false);
		targetName = targetStatusBar.transform.FindChild ("TargetName").GetComponent<TextMeshProUGUI>();
		targetHealth = targetStatusBar.transform.FindChild ("HealthBar/Health").GetComponent<Image>();
		targetHealthText = targetStatusBar.transform.FindChild ("HealthBar/HealthText").GetComponent<TextMeshProUGUI>();

		hero = GetComponent<HeroManager> ();
		heroCombat = hero.heroCombat;

		// setup events
		heroCombat.OnTargetChanged += UpdateTarget;
		heroCombat.OnHealthChanged += UpdateHealth;

		heroName.text = netId.ToString();
		health.fillAmount = (float) heroCombat.health / (float) heroCombat.maxHealth;
		healthText.text = "Health: " + heroCombat.health + " / " + heroCombat.maxHealth;
	}

	public void UpdateTarget(HeroManager oldTarget, HeroManager newTarget) {
		HeroManager target = heroCombat.target;
		HeroCombat targetCombat = target.heroCombat;
		targetStatusBar.SetActive (true);
		targetName.text = target.netId.ToString();
		targetHealth.fillAmount = (float) targetCombat.health / (float) targetCombat.maxHealth;
		targetHealthText.text = "Health: " + targetCombat.health + " / " + targetCombat.maxHealth;
		targetCombat.OnHealthChanged += UpdateTargetHealth;
		if (oldTarget != null) oldTarget.heroCombat.OnHealthChanged -= UpdateTargetHealth;
	}

	public void UpdateTargetHealth(int oldHealth, int newHealth) {
		HeroCombat targetCombat = heroCombat.target.heroCombat;
		targetHealth.fillAmount = (float) targetCombat.health / (float) targetCombat.maxHealth;
		targetHealthText.text = "Health: " + targetCombat.health + " / " + targetCombat.maxHealth;
	}

	public void UpdateHealth(int oldHealth, int newHealth) {
		health.fillAmount = (float) heroCombat.health / (float) heroCombat.maxHealth;
		healthText.text = "Health: " + heroCombat.health + " / " + heroCombat.maxHealth;

        if (newHealth == 0) deathPanel.SetActive(true);
        else if (deathPanel.activeInHierarchy) deathPanel.SetActive(false);
	}

	void OnGUI() {

	}

	// msgs are usually sent by the Controller
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

