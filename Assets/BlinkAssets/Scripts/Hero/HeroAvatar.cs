using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UMA;

public class HeroAvatar : NetworkBehaviour {

	private SlotLibrary slotLibrary;
	private OverlayLibrary overlayLibrary;
	private RaceLibrary raceLibrary;
	private UMAGeneratorBase generator;
	private HeroAnimator heroAnimator;

	private UMADynamicAvatar avatar;
	private UMAData data;
	private UMADnaHumanoid dna;
	private UMADnaTutorial tutorialDna;

	private int numSlots = 20;

	public RuntimeAnimatorController animController;
	[Range (0.0f,1.0f)] public float bodyMass = 0.5f;

	void Start() {
		GameObject umaLib = GameObject.FindWithTag ("UmaLib");
		if (umaLib == null) 
			throw new MissingComponentException ("Ensure an UmaLib is placed inside the scene with tag=UmaLib");
		slotLibrary = umaLib.GetComponentInChildren<SlotLibrary>();
		overlayLibrary = umaLib.GetComponentInChildren<OverlayLibrary>();
		raceLibrary = umaLib.GetComponentInChildren<RaceLibrary>();
		generator = umaLib.GetComponentInChildren<UMAGenerator>();

		InitializeUma ();
	}

	void Update() {
		if (!hasAuthority) return;
		if (bodyMass != dna.upperMuscle) {
			SetBodyMass (bodyMass);
			data.isShapeDirty = true;
			data.Dirty ();
		}
	}

	void InitializeUma() {
		GameObject go = transform.FindChild ("AvatarContainer").gameObject;
		avatar = go.AddComponent<UMADynamicAvatar> ();
		avatar.Initialize ();
		avatar.umaGenerator = generator;
		avatar.animationController = animController;
		// TODO make this a server command UMAGeneratorBase.OnUmaAnimatorCreated += heroAnimator.OnUmaAnimatorCreated;
		avatar.umaData.OnCharacterCreated += OnCharacterCreated;

		dna = new UMADnaHumanoid ();
		tutorialDna = new UMADnaTutorial ();

		data = avatar.umaData;
		data.umaGenerator = generator;
		data.umaRecipe.slotDataList = new SlotData[numSlots];
		data.umaRecipe.AddDna (dna);
		data.umaRecipe.AddDna (tutorialDna);
		ApplyHumanMaleRecipe ();

		avatar.UpdateNewRace ();
	}
		
	void OnCharacterCreated (UMAData obj) {
		heroAnimator = GetComponent<HeroAnimator> ();
		heroAnimator.OnAvatarCreated ();
	}

	void ApplyHumanMaleRecipe() {
		var umaRecipe = avatar.umaData.umaRecipe;
		umaRecipe.SetRace(raceLibrary.GetRace ("HumanMale"));

		SetSlot (0, "MaleEyes");
		SetSlot (1, "MaleInnerMouth");
		SetSlot (2, "MaleFace");
		SetSlot (3, "MaleTorso");
		SetSlot (4, "MaleHands");
		//SetSlot (5, "MaleLegs");
		SetSlot (5, "MaleJeans01");
		SetSlot (6, "FR_MC_Boots");
		SetSlot (7, "M_Hair_Shaggy");
		SetSlot (8, "FR_MC_ShoulderPads");
		SetSlot (9, "FR_MC_TorsoArmor");
		SetSlot (10, "FR_MC_Gloves");

		AddOverlay (0, "EyeOverlay");
		AddOverlay (1, "InnerMouth");
		AddOverlay (2, "MaleHead02");
		AddOverlay (2, "MaleEyebrow01", Color.gray);
		//AddOverlay (2, "MaleBeard03", Color.gray);
		AddOverlay (3, "MaleBody02");
		AddOverlay (3, "MaleUnderwear01");
		AddOverlay (3, "SA_Tee", Color.white);
		LinkOverlay (4, 3);
		//LinkOverlay (5, 3); // MaleLegs
		AddOverlay (5, "MaleJeans01", Color.gray);
		AddOverlay (6, "FR_MC_Boots");
		AddOverlay (7, "M_Hair_Shaggy");
		AddOverlay (8, "FR_MC_ShoulderPads");
		AddOverlay (9, "FR_MC_TorsoArmor");
		AddOverlay (10, "FR_MC_Gloves");
	}

	public void AnimateAbility(HeroManager h, string animationName) {
		if (heroAnimator != null)
			heroAnimator.AnimateAbility (h, animationName);
	}

	public void AnimateMovement(HeroManager h) {
		if (heroAnimator != null)
			heroAnimator.AnimateMovement (h);
	}

	void SetBodyMass(float mass) {
		dna.upperMuscle = mass;
		dna.upperWeight = mass;
		dna.lowerMuscle = mass;
		dna.lowerWeight = mass;
		dna.forearmWidth = mass;
	}

	private void LinkOverlay(int slotNum, int otherSlotNum) {
		data.umaRecipe.slotDataList [slotNum].SetOverlayList (
			data.umaRecipe.slotDataList [otherSlotNum].GetOverlayList ());
	}

	private void AddOverlay(int slotNum, string overlayName) {
		data.umaRecipe.slotDataList [slotNum].AddOverlay (overlayLibrary.InstantiateOverlay (overlayName));
	}

	private void AddOverlay(int slotNum, string overlayName, Color color) {
		data.umaRecipe.slotDataList [slotNum].AddOverlay (overlayLibrary.InstantiateOverlay (overlayName, color));
	}

	private void RemoveOverlay(int slotNum, string overlayName) {
		data.umaRecipe.slotDataList [slotNum].RemoveOverlay (overlayName);
	}

	private void SetSlot(int slotNum, string slotName) {
		data.umaRecipe.slotDataList [slotNum] = slotLibrary.InstantiateSlot (slotName);
	}

	private void RemoveSlot(int slotNum) {
		data.umaRecipe.slotDataList [slotNum] = null;
	}

}
