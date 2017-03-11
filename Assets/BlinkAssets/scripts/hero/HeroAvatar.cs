using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;

public class HeroAvatar : MonoBehaviour {

	public SlotLibrary slotLibrary;
	public OverlayLibrary overlayLibrary;
	public RaceLibrary raceLibrary;
	public UMAGeneratorBase generator;
	public HeroAnimator heroAnimator;

	private UMADynamicAvatar umaDynamicAvatar;
	private UMAData umaData;
	private UMADnaHumanoid umaDna;
	private UMADnaTutorial umaDnaTutorial;

	private int numSlots = 20;

	[Range (0.0f,1.0f)]
	public float bodyMass = 0.5f;

	void Start() {
		heroAnimator = GetComponent<HeroAnimator> ();
		GameObject umaLib = GameObject.FindWithTag ("UmaLib");
		if (umaLib == null) 
			throw new MissingComponentException ("Ensure an UmaLib is placed inside the scene with tag=UmaLib");
		slotLibrary = GameObject.FindWithTag ("SlotLibrary").GetComponent<SlotLibrary>();
		overlayLibrary = GameObject.FindWithTag ("OverlayLibrary").GetComponent<OverlayLibrary>();
		raceLibrary = GameObject.FindWithTag ("RaceLibrary").GetComponent<RaceLibrary>();
		generator = GameObject.FindWithTag ("UMAGenerator").GetComponent<UMAGenerator>();

		InitializeUma ();
	}

	void Update() {
		if (bodyMass != umaDna.upperMuscle) {
			SetBodyMass (bodyMass);
			umaData.isShapeDirty = true;
			umaData.Dirty ();
		}
	}

	void InitializeUma() {
		GameObject avatar = new GameObject ("Avatar");
		umaDynamicAvatar = avatar.AddComponent<UMADynamicAvatar> ();

		umaDynamicAvatar.Initialize ();
		umaData = umaDynamicAvatar.umaData;
		umaDna = new UMADnaHumanoid ();
		umaDnaTutorial = new UMADnaTutorial ();

		umaData.umaRecipe.slotDataList = new SlotData[numSlots];

		umaDynamicAvatar.umaGenerator = generator;
		umaData.umaGenerator = generator;

		umaData.umaRecipe.AddDna (umaDna);
		umaData.umaRecipe.AddDna (umaDnaTutorial);

		CreateMale ();
		umaDynamicAvatar.animationController = heroAnimator.runtimeAnimatorController;

		umaDynamicAvatar.UpdateNewRace ();

		avatar.transform.SetParent (this.transform);
		avatar.transform.localPosition = Vector3.zero;
		avatar.transform.localRotation = Quaternion.identity;
	}

	void CreateMale() {
		var umaRecipe = umaDynamicAvatar.umaData.umaRecipe;
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
		AddOverlay (2, "MaleEyebrow01", Color.black);
		AddOverlay (2, "MaleBeard03", Color.gray);
		AddOverlay (3, "MaleBody02");
		AddOverlay (3, "MaleUnderwear01");
		AddOverlay (3, "SA_Tee", Color.white);
		LinkOverlay (4, 3);
		//LinkOverlay (5, 3); // MaleLegs
		AddOverlay (5, "MaleJeans01", Color.gray);
		AddOverlay (6, "FR_MC_Boots");
		AddOverlay (7, "M_Hair_Shaggy", Color.gray);
		AddOverlay (8, "FR_MC_ShoulderPads");
		AddOverlay (9, "FR_MC_TorsoArmor");
		AddOverlay (10, "FR_MC_Gloves");
	}

	public void AnimateAbility(Hero h, string animatorKey) {
		if (heroAnimator.animator == null) {
			if (transform.childCount > 0) {

				heroAnimator.animator = transform.GetChild (0).GetComponent (typeof(Animator)) as Animator;
				if (heroAnimator.animator != null) {
					heroAnimator.animator.applyRootMotion = false;
				}
			}
			return;
		}
		heroAnimator.AnimateAbility (h, this, animatorKey);
	}

	public void AnimateMovement(Hero h) {
		if (heroAnimator.animator == null) {
			if (transform.childCount > 0) {
				
				heroAnimator.animator = transform.GetChild (0).GetComponent (typeof(Animator)) as Animator;
				if (heroAnimator.animator != null) {
					heroAnimator.animator.applyRootMotion = false;
				}
			}
			return;
		}
		heroAnimator.AnimateMovement (h, this);
	}

	void SetBodyMass(float mass) {
		umaDna.upperMuscle = mass;
		umaDna.upperWeight = mass;
		umaDna.lowerMuscle = mass;
		umaDna.lowerWeight = mass;
		umaDna.forearmWidth = mass;
	}

	private void LinkOverlay(int slotNum, int otherSlotNum) {
		umaData.umaRecipe.slotDataList [slotNum].SetOverlayList (
			umaData.umaRecipe.slotDataList [otherSlotNum].GetOverlayList ());
	}

	private void AddOverlay(int slotNum, string overlayName) {
		umaData.umaRecipe.slotDataList [slotNum].AddOverlay (overlayLibrary.InstantiateOverlay (overlayName));
	}

	private void AddOverlay(int slotNum, string overlayName, Color color) {
		umaData.umaRecipe.slotDataList [slotNum].AddOverlay (overlayLibrary.InstantiateOverlay (overlayName, color));
	}

	private void RemoveOverlay(int slotNum, string overlayName) {
		umaData.umaRecipe.slotDataList [slotNum].RemoveOverlay (overlayName);
	}

	private void SetSlot(int slotNum, string slotName) {
		umaData.umaRecipe.slotDataList [slotNum] = slotLibrary.InstantiateSlot (slotName);
	}

	private void RemoveSlot(int slotNum) {
		umaData.umaRecipe.slotDataList [slotNum] = null;
	}

}
