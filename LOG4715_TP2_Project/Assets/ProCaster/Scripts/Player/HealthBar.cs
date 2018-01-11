using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    // Propriétés
    private AudioSource audioSource;
    public AudioClip painAudioClip;
    private float healthPoint = 50;
	public bool Dead {get; set;}

    public float HealthPoint
    {
        get { return this.healthPoint; }
        set {
            this.healthPoint = value;
            this.UpdateHealthBar();
        }
    }

    public bool Invincible { get; set; } = false;


    // Variables accessibles dans l'éditeur
    [SerializeField]
    Image Bar;

    [SerializeField]
    float StartHealthPoint = 100.0f;

    [SerializeField]
    float MaxHealthPoint = 100.0f;


    void Start()
    {
        HealthPoint = StartHealthPoint;
        this.audioSource = GetComponent<AudioSource>();
    }

    // Ajuste la taille de la bar de vie
    private void UpdateHealthBar()
    {
        if (Bar != null)
        {
            float ratio = HealthPoint / MaxHealthPoint;
            Bar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        }
    }

    // Enlève de la vie au joueur
    public void TakeDamage(float damage)
    {
        if (Invincible)
        {
            return;
        }

        HealthPoint -= damage;
        StartCoroutine(PlayPainSound());

        if (!Dead && HealthPoint <= 0)
        {
			Dead = true;
            HealthPoint = 0;
            GetComponent<ILivingController>().Die();
        }
    }

    // Ajoute de la vie au joueur
    public void Heal(float heal)
    {
        HealthPoint += heal;
        if (HealthPoint > MaxHealthPoint)
        {
            HealthPoint = MaxHealthPoint;
        }
    }
    IEnumerator PlayPainSound()
    {
        if (this.painAudioClip && !this.Dead )
        {
            AudioClip oldAudioClip = this.audioSource.clip;
            this.audioSource.clip = this.painAudioClip;
            this.audioSource.Play();
            yield return new WaitForSeconds(0.5f);
            this.audioSource.clip = oldAudioClip;
        }
    }
}
