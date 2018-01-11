using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerControler))]
public class FootStepSource : MonoBehaviour {

    public float PitchVariance = 0.1f;
    public float VolumeVariance = 0.1f;
    public float CrouchOffset = 0.4f;

    private float defaultPitch;
    private float defaultVolume;
    private AudioSource source;
    private PlayerControler player;
    private AudioClip footstepClip;
    private HealthBar healthBar;

    void Start()
    {
        player = GetComponent<PlayerControler>();
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;

        defaultPitch = source.pitch;
        defaultVolume = source.volume;

        this.footstepClip = source.clip;
        this.healthBar = GetComponent<HealthBar>();
    }

    void FootStep()
    {
        if (this.source.clip== this.footstepClip && !this.healthBar.Dead)
        {
            if (player._Grounded)
            {
                source.pitch = (Random.Range(defaultPitch - PitchVariance, defaultPitch + PitchVariance));
                source.volume = Random.Range(defaultVolume - VolumeVariance, defaultVolume + VolumeVariance);

                if (player._Crouched)
                {
                    source.volume = Mathf.Max(0, source.volume - CrouchOffset);
                }
                source.Play();
            }
        }
  
        
    }
}
