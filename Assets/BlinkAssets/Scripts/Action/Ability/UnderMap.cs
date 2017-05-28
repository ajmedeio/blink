using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using DigitalRuby.PyroParticles;

public class UnderMap : Ability {

    public UnderMap() : base (
            "UnderMap",
            "Going under the map is usually an indication of suicide, this is generally considered a bad idea.",
            30.0f, 50, "initialAnimation",
            -1.0f, 0, 0, 0, "tickAnimation", 0, "terminalAnimation") { }

    public override bool IsLegal(HeroManager hero) {
        return hero.transform.position.y < -50;
    }

    public override void DoAbility(HeroManager hero) {
        hero.heroCombat.CmdChangeHealthBy(this, -hero.heroCombat.maxHealth);
    }

    public override void OnAbilityHitTarget(GameObject ability) {
        HomingAbility firebolt = ability.GetComponent<HomingAbility>();
        firebolt.target.heroCombat.CmdAbilityHitMe(firebolt.gameObject);
    }

}
