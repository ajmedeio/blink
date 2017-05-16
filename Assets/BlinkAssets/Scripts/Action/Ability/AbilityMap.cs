using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMap {

	public static readonly Ability blink = new Blink();
	public static readonly Ability firebolt = new Firebolt();

	public static readonly Dictionary<string, Ability> masterAbilityMap = new Dictionary<string, Ability>()
	{
		{blink.name, blink},
		{firebolt.name, firebolt}
	};

	private Dictionary<string, Ability> backingMap;
	
	public AbilityMap() {
		backingMap = new Dictionary<string, Ability> ();
	}

	public void Add(string name, Ability ability) {
		backingMap.Add (name, ability);
	}

}
