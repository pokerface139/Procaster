using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SleepSpell : BaseSpell
{

    // Variables exposes
    public float LifeTime = 4.0f;
    public float Damage = 15f;
    public float Speed = 15f;

    private Transform _Player { get; set; }
    private Transform _Emitter { get; set; }
    private Rigidbody _Projectile { get; set; }


    public SleepSpell(Transform player, Transform wand)
    {
        _Player = player;
        _Emitter = wand.Find("SpellEmitter");
        SpriteImage = Resources.Load<Sprite>("SleepSpell");
        

        // Mauvaise pratique d'utiliser le Resource folder, mais on est en tp
        GameObject projectile = Resources.Load("SleepBolt") as GameObject;
        _Projectile = projectile.GetComponent<Rigidbody>();
    }

    public override void Cast()
    {
        float lanePosition = _Player.GetComponent<PlayerControler>().LanePosition;
        Rigidbody projectile = GameObject.Instantiate(_Projectile);

        Vector3 projectilePosition = _Emitter.position;
        projectilePosition.x = lanePosition;

        projectile.transform.position = projectilePosition;
        projectile.velocity = _Player.forward.normalized * Speed;

        SleepBolt bolt = projectile.GetComponent<SleepBolt>();
        bolt.LifeTime = LifeTime;
        bolt.Damage = Damage;

        Physics.IgnoreCollision(_Player.GetComponent<Collider>(), projectile.GetComponent<Collider>());
    }
}
