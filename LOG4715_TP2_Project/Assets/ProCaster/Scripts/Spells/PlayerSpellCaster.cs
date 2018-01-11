using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ProCaster.Scripts.Spells
{
    public class PlayerSpellCaster : MonoBehaviour {

        // Constantes
        private KeyCode[] KeyCodeNumber = {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9
        };
        [SerializeField]

        // Propriétés
        private Animator _Anim { get; set; }
		private Rigidbody rigidbody;
        private Vector3 _Target { get; set; } = new Vector3();
        public List<BaseSpell> Spells { get; private set; }
        public BaseSpell CurrentSpell { get; private set; }

        // Variables Exposées
        [SerializeField]
        private Transform Wand;

        [SerializeField]
        private Image SpellImage;

        [SerializeField]
        [Range(1, 3)]
        private int DefaultSpell = 1;

		[SerializeField]
        public SleepSpell sleepSpell;

		[SerializeField]
		public LightSpell lightSpell;

		[SerializeField]
        public LevitationSpell levitationSpell;

		[SerializeField]
        public ElementalShield elementalShield;

        // Use this for initialization
        void Start () {

            _Anim = GetComponent<Animator>();

            // Création du sort d'endormissement
            sleepSpell = new SleepSpell(transform, Wand);
			lightSpell = new LightSpell(transform, Wand);
            levitationSpell = new LevitationSpell(transform, Wand);
            elementalShield = new ElementalShield(transform);

            CurrentSpell = sleepSpell;

            // Ajout à la liste des sorts
            Spells = new List<BaseSpell>
            {
                CurrentSpell,
				lightSpell,
                levitationSpell
            };

            SelectSpell(DefaultSpell - 1);


            rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update () {
            elementalShield.UpdateSpell();
            CurrentSpell.UpdateSpell();

            if (Input.GetButtonDown("Fire2")) {
                elementalShield.ImmediateCast();
            }

            // Détecte le click de la souris si l'animation est finie
            if (!_Anim.GetCurrentAnimatorStateInfo(2).IsName("Cast Spell"))
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    // Trouver le point du click
                    // Démarrer l'animation
                    _Anim.SetTrigger("CastSpell");
                    CurrentSpell.ImmediateCast();
                }
            }

            // Détecte si les chiffres sont touchés
            for (int i = 0; i < KeyCodeNumber.Length; ++i) {
                if (Input.GetKeyDown (KeyCodeNumber [i])) {
                    SelectSpell (i);
                    break; // On veut juste en prendre un
                }
            }
        }

        // Appeler au bon moment de l'animation à l'aide d'un évènement
        void CastSpell()
        {
            CurrentSpell.Cast();
            Wand.GetComponent<AudioSource>().Play();
        }

        // Permet au joueur de changer de sort
        void SelectSpell(int index)
        {
            if (index < Spells.Count)
            {
                CurrentSpell.Cancel();
                CurrentSpell = Spells[index];
                if (SpellImage != null)
                {
                    SpellImage.sprite = CurrentSpell.SpriteImage;
                }
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision);

        }
        public void OnCollisionStay(Collision collision)
        {
            HandleCollision(collision);
        }

        void HandleCollision(Collision collision)
        {
            if (collision.transform.tag.Equals("Levitable")
                && CurrentSpell.GetType() == typeof(LevitationSpell)
                && levitationSpell._CurrentState == LevitationSpell.State.Selected
                && levitationSpell._selectedObject == collision.gameObject)
            {
                collision.rigidbody.velocity = Vector3.zero;
                collision.rigidbody.angularVelocity = Vector3.zero;
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;

                Debug.Log("OnCollisionEnter enter!" + collision.relativeVelocity);
                foreach (ContactPoint contact in collision.contacts)
                {
                    Vector3 normal = -contact.normal;
                    if (normal.y < 0)
                    {
                        normal.y = 0;
                    }
                    collision.rigidbody.AddForce(normal);
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                }
            }
        }

    }
}
