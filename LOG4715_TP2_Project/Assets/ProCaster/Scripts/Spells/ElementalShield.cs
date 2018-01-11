using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ElementalShield : BaseSpell {

	// Propriétés
    private GameObject shield;
    private Transform player;
	private float shieldDuration;
	private float timer = 0f;

	// Variables exposées
    public float cooldown = 0.5f;

    public ElementalShield(Transform player)
    {
        this.player = player;
        shield = Resources.Load<GameObject>("ElementalShield");
        shieldDuration = shield.transform.Find("Particles").GetComponent<ParticleSystem>().main.duration;
        timer = shieldDuration + cooldown;
    }

    public override void UpdateSpell()
    {
        timer += Time.deltaTime;

        if (timer > shieldDuration)
        {
            player.GetComponent<HealthBar>().Invincible = false;
        }
    }

    public override void ImmediateCast()
    {
        if (timer > cooldown + shieldDuration)
        {
            timer = 0;
            GameObject s =GameObject.Instantiate(shield,player.transform);
            GameObject.Destroy(s, shieldDuration);
            player.GetComponent<HealthBar>().Invincible = true;
        }
    }

}
