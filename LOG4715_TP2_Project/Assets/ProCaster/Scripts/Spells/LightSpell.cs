using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightSpell : BaseSpell
{
	// Variables exposées
	public float Intensity = 2f;
	public float Range  = 4f;

	// Propriétés
	private Transform _Player { get; set; }
    private Light _Light { get; set; }

    private bool _IsOpen { get; set; }
    private float _DefaultRange { get; set; }
    private float _DefaultIntensity { get; set; }
    

    public LightSpell(Transform player, Transform wand)
    {
        _Player = player;
        _Light = wand.Find("Light").GetComponent<Light>();
        _DefaultIntensity = _Light.intensity;
        _DefaultRange = _Light.range;
        SpriteImage = Resources.Load<Sprite>("Lumos");
    }

    public override void Cast()
    {
        _IsOpen = !_IsOpen;

        if (!_IsOpen)
        {
            _Light.intensity = _DefaultIntensity;
            _Light.range = _DefaultRange;
        }
        else
        {
			_Light.intensity = Intensity;
			_Light.range = Range;
        }

    }

    public override void Cancel()
    {
        if (_IsOpen)
        {
            Cast();
        }
    }
}
