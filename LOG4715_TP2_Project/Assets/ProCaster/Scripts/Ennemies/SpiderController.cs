using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderController : ILivingController
{

    // Définition d'un pointeur de fonction
    private delegate void Behaviour();

    // Définition des états de l'araignée
    enum EnnemyState
    {
        Idle,
        Roaming,
        Following,
        Attack,
        Shoot,
        Dead
    };

    // Propriétés
    private Transform _Player;
    private Rigidbody _Rb;
    private Animator _Anim;

    private EnnemyState _State;
    private Vector3 _Position;     // Position initiale
    private Quaternion _Rotation;     // Rotation initiale
    private BoxCollider _Collider;
    private Behaviour _CurrentBehaviour;

    private float NextAttackTime;
    private float DisapearTime;
    private bool IsDisapearing = false;
    private float RoamingEndTime;
    private Vector3 zoneStart;
    private Vector3 zoneEnd;

    // Propriétés exposés
    [SerializeField]
    Transform ZoneStart;

    [SerializeField]
    Transform ZoneEnd;

    [SerializeField]
    Transform ProjectileEmitter;

    [SerializeField]
    GameObject Projectile;

    [SerializeField]
    float VisionAngle = 90.0f;

    [SerializeField]
    float VisionDistance = 6.0f;

    [SerializeField]
    float Speed = 1.75f;

    [SerializeField]
    float AttackCooldown = 1.0f;

    [SerializeField]
    float MeleeAttackDamage = 10.0f;

    [SerializeField]
    float ShootAttackDamage = 5f;

    [SerializeField]
    float AttackDistance = 1.5f;

    [SerializeField]
    float ProjectileSpeed = 2.0f;

    [SerializeField]
    float ShootCooldown = 2.5f;

    [SerializeField]
    float ShootingAttackProbability = 0.5f;

    [SerializeField]
    float TimeWhileDead = 2.0f;

    [SerializeField]
    float RoamTime = 2.0f;

    
    // Use this for initialization
    void Start () {
		_Player = GameObject.FindGameObjectWithTag ("Player").transform;
		_Rb = GetComponent<Rigidbody> ();
		_Anim = GetComponent<Animator> ();

		_State = EnnemyState.Idle;
		_Position = transform.position;
        _Rotation = transform.rotation;
		_Collider = GetComponent<BoxCollider> ();
		_CurrentBehaviour = this.IdleBehaviour;

        NextAttackTime = Time.time;
        DisapearTime = Time.time;
        RoamingEndTime = Time.time;

        ZoneEnd.gameObject.SetActive(false);
        ZoneStart.gameObject.SetActive(false);
        zoneStart = ZoneStart.position;
        zoneEnd = ZoneEnd.position;

    }
	
	// Update is called once per frame
	void Update () {
		EvaluateState ();
		ExecuteBehaviour();
	}

	// Exécute le comportement de l'état
	private void ExecuteBehaviour()
	{
		_CurrentBehaviour();
	}

	// Évalue l'état
	private void EvaluateState()
	{
		bool targetVisible = IsPlayerVisible ();
		bool targetInDetectionArea = IsPlayerInDetectionArea ();
        bool targetReachable = IsPlayerInMovementZone();
        bool targetIsSlowed = _Player.GetComponent<SlowedDown>() != null;

		switch (_State) {
		    case EnnemyState.Idle:

			    // Change State
			    if (targetVisible && targetInDetectionArea) {

                    if (targetReachable &&  Random.value >= ShootingAttackProbability  )
                    {
                        _State = EnnemyState.Following;
                        _CurrentBehaviour = this.FollowBehaviour;
                    }
                    else
                    {
                        _State = EnnemyState.Shoot;
                        _CurrentBehaviour = this.ShootBehaviour;
                    }
			    }
			    break;

		    case EnnemyState.Following:
                if (!targetReachable && targetVisible && targetInDetectionArea)
                {
                    _State = EnnemyState.Shoot;
                    _CurrentBehaviour = this.ShootBehaviour;
                }
                else if (!targetInDetectionArea || !targetVisible) {
                    RoamingEndTime = Time.time + RoamTime;
                    _State = EnnemyState.Roaming;
                    _CurrentBehaviour = this.RoamingBehaviour;
                }
                else if (DistanceFromPlayer().magnitude < AttackDistance)
                {
                    _State = EnnemyState.Attack;
                    _CurrentBehaviour = this.AttackBehaviour;
                }
			    break;
		    case EnnemyState.Attack:
			    if (DistanceFromPlayer ().magnitude >= AttackDistance) {
				    _State = EnnemyState.Following;
				    _CurrentBehaviour = this.FollowBehaviour;
			    }
			    break;

            case EnnemyState.Shoot:
                if (DistanceFromPlayer().magnitude > VisionDistance || !targetVisible)
                {
                    RoamingEndTime = Time.time + RoamTime;
                    _State = EnnemyState.Roaming;
                    _CurrentBehaviour = this.RoamingBehaviour;
                }
                else if (DistanceFromPlayer().magnitude < AttackDistance)
                {
                    _State = EnnemyState.Attack;
                    _CurrentBehaviour = this.AttackBehaviour;
                }
                else if (targetIsSlowed && targetReachable)
                {
                    _State = EnnemyState.Following;
                    _CurrentBehaviour = this.FollowBehaviour;
                }
                break;

            case EnnemyState.Roaming:
                if (targetVisible && targetInDetectionArea)
                {
                    if ( targetReachable && Random.value >= ShootingAttackProbability )
                    {
                        _State = EnnemyState.Following;
                        _CurrentBehaviour = this.FollowBehaviour;
                    }
                    else
                    {
                        _State = EnnemyState.Shoot;
                        _CurrentBehaviour = this.ShootBehaviour;
                    }
                }
                else if (Time.time >= RoamingEndTime)
                {
                    _State = EnnemyState.Idle;
                    _CurrentBehaviour = this.IdleBehaviour;
                }
                break;
		}

	}

    private void Move(Vector3 direction)
    {
        direction.x = 0; direction.y = 0;
        _Rb.velocity = Speed * direction.normalized;
        _Anim.SetFloat("MoveSpeed", direction.magnitude);

        if (direction.magnitude > 0.1)
        {
            Flip(direction.z);
        }
    }

    private void Flip(float horizontal)
    {
        if (Mathf.Sign(transform.forward.z)  != Mathf.Sign(horizontal) )
        {
            Vector3 forward = new Vector3(0, 0, horizontal);
            transform.forward = forward;
        }
    }

	private Vector3 DistanceFromPlayer()
	{
		// Vecteur entre l'araignée et le joueur
		float playerHeight = _Player.GetComponent<CapsuleCollider>().height;
		Vector3 playerPosition = _Player.position + Vector3.up * playerHeight / 2;
		Vector3 spiderPosition = transform.position + Vector3.up * _Collider.size.y / 2;
		return playerPosition - spiderPosition;
	}

	// Vérifie si le joueur est visible
	private bool IsPlayerInDetectionArea()
	{
		// Vecteur entre l'araignée et le joueur
		Vector3 ennemy2Player = DistanceFromPlayer();

		// Distance et position dans l'angle de vision
		float distance = ennemy2Player.magnitude;
		float angle = Vector3.Angle (transform.forward, ennemy2Player);

		return  distance <= VisionDistance && angle <= VisionAngle;
	}

	private bool IsPlayerVisible()
	{
		// Vecteur entre l'araignée et le joueur
		Vector3 ennemy2Player = DistanceFromPlayer();

		// Vérifie s'il y a un objet entre l'araignée et le joueur
		RaycastHit[] hits = Physics.RaycastAll(transform.position, ennemy2Player, ennemy2Player.magnitude);
			
		bool isVisible = true;
		foreach (RaycastHit hit in hits) {
			if(hit.transform != _Player && hit.transform != transform)
			{
				isVisible = false;
				break;
			}
		}

		return isVisible;
	}
		
	private void FollowBehaviour()
	{
       
        if (IsPlayerInMovementZone()) {

			// Direction au prochain waypoint du checmin
			Vector3 direction = _Player.transform.position - transform.position;
            direction.y = 0;

            // Rotation de l'araignée dans le sens de la direction en z
            transform.LookAt (transform.position + direction.normalized);

			float angle = Vector3.Angle (Vector3.forward, direction);
			if (angle > 90) {
				angle = 180 - angle;
			}
		
			// Détermine si on avance ou un changement de voie
			if (angle < 45) {
                Move(direction);
			} 
			else {
				// Change lane
			}
		}
	}

    private bool IsPlayerInMovementZone()
    {
        float d = Mathf.Abs(Vector3.Dot(zoneEnd - zoneStart,Vector3.forward));
        float d1 = Mathf.Abs(Vector3.Dot(zoneStart - _Player.transform.position,Vector3.forward));
        float d2 = Mathf.Abs(Vector3.Dot(zoneEnd - _Player.transform.position,Vector3.forward));
        return d1 <= d && d2 < d;

    }

	private void IdleBehaviour()
	{
        Vector3 direction = _Position - transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.25)
        {
            Move(direction);
        }
        else {
            if (Quaternion.Angle(transform.rotation , _Rotation) > 0.25)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _Rotation, 0.05f);
            }
        }
	}


	private void AttackBehaviour()
	{
		_Anim.SetFloat("MoveSpeed",0);

		if (!_Anim.GetCurrentAnimatorStateInfo(0).IsName ("Attack") && NextAttackTime <= Time.time) {
			_Anim.SetTrigger ("Attack");
            NextAttackTime = Time.time + AttackCooldown;
		}

        Vector3 target = _Player.position;
        target.y = transform.position.y;
		transform.LookAt (target);
	}


    private void DeadBehaviour()
    {
        if (DisapearTime <= Time.time)
        {
            if (!IsDisapearing)
            {
                IsDisapearing = true;
                StartCoroutine("Shrink");
            }
        }
    }


    private void AttackPlayer()
    {
        if (DistanceFromPlayer().magnitude < AttackDistance)
        {
            _Player.GetComponent<HealthBar>().TakeDamage(MeleeAttackDamage);
        }
    }

    private void RoamingBehaviour()
    {
        // Random rotation
        _Anim.SetFloat("MoveSpeed", 0f);
    }


    private void ShootBehaviour()
    {
        // Faire regarder l'araignée dans le sens du joueur
        Vector3 look = _Player.position;
        look.y = transform.position.y;
        transform.LookAt(look);


        if (NextAttackTime <= Time.time)
        {
            _Anim.SetTrigger("Attack");
            float playerHeight = _Player.GetComponent<CapsuleCollider>().height / 2;
            Vector3 target = _Player.position + playerHeight * Vector3.up;
            Vector3 direction = target - ProjectileEmitter.position;


            GameObject projectile = Instantiate(Projectile);
            projectile.transform.position = ProjectileEmitter.position;
            projectile.transform.LookAt(transform.position + direction);
            projectile.GetComponent<Rigidbody>().velocity = direction.normalized * ProjectileSpeed;
            projectile.GetComponent<WebSphere>().Damage = ShootAttackDamage;

            Physics.IgnoreCollision(_Collider, projectile.GetComponent<Collider>());

            NextAttackTime = Time.time + ShootCooldown;
        }
        
    }

    override public void Die()
    {
        _Anim.SetBool("Dead", true);
        _State = EnnemyState.Dead;
        _CurrentBehaviour = this.DeadBehaviour;
        DisapearTime = Time.time + TimeWhileDead;
        Physics.IgnoreCollision(_Collider, _Player.GetComponent<Collider>());
    }


    IEnumerator Shrink()
    {
        for (float s = 1f; s >= 0.2; s -= 0.1f)
        {
            transform.localScale = s * transform.localScale;
            yield return new WaitForSeconds(0.05f);
        }

        Destroy(this.gameObject);
    }
}
