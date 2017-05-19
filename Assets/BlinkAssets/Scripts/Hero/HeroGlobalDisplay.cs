using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class HeroGlobalDisplay : NetworkBehaviour {

    private HeroManager hero;
    private HeroCombat heroCombat;
    private Camera camera;
    private Transform heroGlobalDisplayTransform;

    private Transform heroStatusBar;
    private TextMeshProUGUI heroName;
    private Image health;

    [SerializeField]
    private Sprite selfHealthSprite;

	// Use this for initialization
	void Start () {
        hero = GetComponent<HeroManager>();
        heroCombat = hero.heroCombat;
        heroGlobalDisplayTransform = transform.FindChild("HeroGlobalDisplay");

        // find all the hud objects on the screen
        heroStatusBar = heroGlobalDisplayTransform.FindChild("HeroStatusBar").transform;
        heroName = heroStatusBar.FindChild("HeroName").GetComponent<TextMeshProUGUI>();
        health = heroStatusBar.FindChild("HealthBar/Health").GetComponent<Image>();

        if (isLocalPlayer) {
            heroName.fontSharedMaterial = Resources.Load("FontsMaterials/LiberationSans SDF - Green Glow") as Material;
            health.sprite = selfHealthSprite;
            health.color = Color.white;
        }

        heroName.text = netId.ToString();
        health.fillAmount = (float) heroCombat.health / (float) heroCombat.maxHealth;

        heroCombat.OnHealthChanged += UpdateHealth;
	}

    void Update() {
        if (camera == null) {
            GameObject c = GameObject.FindWithTag("LocalHeroCamera");
            if (c != null) camera = c.GetComponent<Camera>();
        }
        if (camera == null) return;
        
        heroGlobalDisplayTransform.LookAt(camera.transform);
        heroGlobalDisplayTransform.Rotate(Vector3.up, 180.0f);
    }

    public void UpdateHealth(int oldHealth, int newHealth) {
        health.fillAmount = (float) newHealth / (float) heroCombat.maxHealth;
    }
}
