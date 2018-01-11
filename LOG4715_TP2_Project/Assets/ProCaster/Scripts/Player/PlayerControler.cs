#define Release

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : ILivingController
{
    enum Lane { Far, Middle, Near }

    // Déclaration des constantes
    private static readonly Vector3 FlipRotation = new Vector3(0, 180, 0);
    private static readonly Vector3 CameraPosition = new Vector3(10, 1, 0);
    private static readonly Vector3 InverseCameraPosition = new Vector3(-10, 1, 0);
    private const float LaneDistance = 1f;
    private float PlayerHeight;

    // Déclaration des composants
    Animator _Anim { get; set; }
    Rigidbody _Rb { get; set; }
    Camera _MainCamera { get; set; }
    CapsuleCollider _Collider { get; set; }
    Transform _CeilingCheck { get; set; }
    Transform _FarCheck { get; set; }
    Transform _NearCheck { get; set; }

    // Variable d'états
    bool _HeadBlocked { get; set; }
	bool _Dead {get; set;}
    public bool _Grounded { get; set; }
    public bool _Crouched { get; set; }
    public bool _Flipped { get; set; }
    Lane _Lane { get; set; }
    public float LanePosition {
        get {
            switch (_Lane)
            {
                case Lane.Far:
                    return -LaneDistance;
                case Lane.Near:
                    return LaneDistance;
                default:
                    return 0f;
            }
        }
    }


    // Valeurs exposées
    [SerializeField]
    public float MoveSpeed = 5.0f;

    [SerializeField]
    float JumpForce = 10f;

    [SerializeField]
    LayerMask WhatIsGround;


	private GameObject box;

    // Awake se produit avait le Start. Il peut être bien de régler les références dans cette section.
    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        _MainCamera = Camera.main;
        _Collider = GetComponent<CapsuleCollider>();

        _CeilingCheck = transform.Find("CeilingCheck");
        _FarCheck = transform.Find("FarCheck");
        _NearCheck = transform.Find("NearCheck");
    }

    // Utile pour régler des valeurs aux objets
    void Start()
    {
        _Lane = Lane.Middle;
        _Grounded = false;
        _Flipped = false;
		_Dead = false;
        PlayerHeight = _Collider.bounds.size.y;


		#if Debug
		box = GameObject.CreatePrimitive (PrimitiveType.Cube);
		box.GetComponent<Collider> ().enabled = false;
		#endif
    }

    // Vérifie les entrées de commandes du joueur
    void Update()
    {
		if (!_Dead) {
			var horizontal = Input.GetAxis("Horizontal") * MoveSpeed;
			HorizontalMove(horizontal);
			FlipCharacter(horizontal);
			CheckCrouch();
            if (!_Crouched || !_HeadBlocked)
            {
                CheckJump();
            }
			MoveLane();
		}        
    }

    // Gère le mouvement horizontal
    void HorizontalMove(float horizontal)
    {
        _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, horizontal);
        _Anim.SetFloat("MoveSpeed", Mathf.Abs(horizontal));
    }

    // Gère le saut du personnage, ainsi que son animation de saut
    void CheckJump()
    {
        if (_Grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _Rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
                _Grounded = false;
                _Anim.SetBool("Grounded", false);
                _Anim.SetBool("Jump", true);
            }
        }
    }

    // Vérifie si le joueur est accroupi
    void CheckCrouch()
    {
        if (_Grounded)
        {
            bool CPressed = Input.GetKey(KeyCode.C);

            // Vérifier si on peut se lever
            if (_Crouched && !CPressed)
            {
                // Define a box
                Vector3 halfDim = new Vector3(_Collider.radius / 2.0f, 0.2f, _Collider.radius / 2.0f);
                Vector3 center = _CeilingCheck.position - Vector3.down * halfDim.y;

                _HeadBlocked = Physics.CheckBox(center, halfDim, Quaternion.identity);
                if (!_HeadBlocked)
                {
                    _Crouched = false;
                }
            }
            else
            {
                _Crouched = CPressed;
            }

            // Animation
            _Anim.SetBool("Crouch", _Crouched);

            // Changer la hauteur du collider
            if (_Crouched)
            {
                _Collider.center = new Vector3(0, 0.65f, 0);
                _Collider.height = 1.3f;
            }
            else
            {
                _Collider.center = new Vector3(0, 0.8f, 0);
                _Collider.height = 1.6f;
            }
        }
    }

    // Gère le changement de voie
    void MoveLane()
    {
        // Entrée du joueur en profondeur
        short yMovement = (short)Input.GetAxisRaw("Vertical");

        // En regarde si on est en transition
        float laneX = LanePosition;
        bool inTransition = Mathf.Abs(laneX - transform.position.x) > 0.1f;


        // Évaluation de la voie
        short lane = (short)_Lane;
        if (!inTransition)
        {
            lane -= yMovement;
            lane = (short)Mathf.Clamp(lane, 0, 2);
        }

        // On vérifie s'il y a obstruction dans la direction du mouvement
        bool blocked = IsLaneBlocked(lane);

        // Bouger le joueur vers la voie si elle n'est pas bloqué
        if (!blocked) {
            _Lane = (Lane)lane;

            // Position visée sur la voie
            Vector3 lanePosition = transform.position;
            lanePosition.x = LanePosition;

            // Direction
            Vector3 direction = lanePosition - transform.position;

            // Déplacement
            _Rb.MovePosition(Vector3.MoveTowards(transform.position, lanePosition, 5 * Time.deltaTime));
        }

    }

    // Regarde si le côté du joueur est bloqué par un objet
    private bool IsLaneBlocked(short lane)
    {


		// Trouver la largeur de la boite de collision
		float laneX = (lane - 1) * LaneDistance;;
		float width = Mathf.Max(Mathf.Abs (laneX - transform.position.x), 0.1f);

		// Définition des dimensions de la boite de collision 
		Vector3 halfDim = new Vector3(_Collider.radius,_Collider.height,_Collider.radius) / 2.0f;
		halfDim.x = width / 2f;
        halfDim -= new Vector3(0.05f, 0.05f, 0.05f);

		// Définition du centre de la boite de collision
		float direction = laneX - transform.position.x;
		direction = direction / Mathf.Abs (direction);
        Vector3 center = transform.position + _Collider.center;
		center.x = transform.position.x + direction * (_Collider.radius + width) / 2f ;

		#if Debug
		box.transform.localScale = halfDim * 2f;
		box.transform.position = center;

		#endif


		// On regarde s'il y a collision
        Collider[] collisions = Physics.OverlapBox(center,halfDim);

        LayerMask wall = LayerMask.GetMask("Wall");
        LayerMask transparent = LayerMask.GetMask("Transparent");

        foreach (Collider collision in collisions)
        {
            bool isPlayer = collision.gameObject.tag == "Player";
            bool isTrigger = collision.isTrigger;
            bool isWall = 1 << collision.gameObject.layer == wall.value;
            bool isTransparent = 1 << collision.gameObject.layer == transparent.value;

            if (!isPlayer && !isTrigger && !isWall && !isTransparent)
            {
                return true;
            }
        }

        return false;
    }

	override public void Die()
	{
		_Dead = true;
		_Anim.SetBool ("Dead", _Dead);
	}

    // Gère l'orientation du joueur et les ajustements de la camera
    void FlipCharacter(float horizontal)
    {
        if (horizontal < 0 && !_Flipped)
        {
            _Flipped = true;
            transform.Rotate(FlipRotation);
        }
        else if (horizontal > 0 && _Flipped)
        {
            _Flipped = false;
            transform.Rotate(-FlipRotation);
        }
    }

    // Collision avec le sol
    void OnCollisionEnter(Collision coll)
    {        
        // On s'assure de bien être en contact avec le sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) == 0)
            return;

        // Évite une collision avec le plafond
        if (coll.relativeVelocity.y > 0)
        {
            _Grounded = true;
            _Anim.SetBool("Grounded", _Grounded);
        }
    }

    void OnCollisionStay(Collision coll)
    {
        // On s'assure de bien être en contact avec le sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) == 0)
            return;

        // Évite une collision avec le plafond
        if (!_Grounded && coll.relativeVelocity.y > 0 && _Rb.velocity.y < 0.01f)
        {
            _Grounded = true;
            Debug.Log("Grounded true");
            _Anim.SetBool("Grounded", _Grounded);
        }
    }
}
