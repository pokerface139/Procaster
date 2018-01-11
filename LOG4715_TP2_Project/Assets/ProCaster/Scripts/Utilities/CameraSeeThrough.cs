using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSeeThrough : MonoBehaviour {

	// Variables exposées
	public Transform Target;
	public float Alpha = 0.25f;
	public LayerMask Mask;
	public float fadeTime = 0.1f;

	// Properties
	private List<GameObject> fadeObjects;
	private List<GameObject> temp;

	// Use this for initialization
	void Start () {
		fadeObjects = new List<GameObject> ();
		temp = new List<GameObject> ();
	}

	// Update is called once per frame
	void Update () {
		
		temp.Clear ();

		// Trouver les objets qui cachent la cible
		Vector3 direction = Target.position - transform.position;
		RaycastHit[] hits = Physics.RaycastAll (transform.position, direction.normalized, direction.magnitude, Mask); 

		// Diminuer l'opacité des objets qui cachent
		foreach (RaycastHit hit in hits) {
			GameObject o = hit.collider.gameObject;
			if (fadeObjects.Contains (o)) {
				FadeOut (o);
				fadeObjects.Remove (o);
			} 

			temp.Add(o);
			FadeOut (o);
		}

		// Remettre l'opacité des objets qui ne cachent plus.
		foreach (GameObject o in fadeObjects)
		{
			FadeIn (o);
		}

		// Mettre a jour la liste des objets cachés
		fadeObjects = new List<GameObject>(temp); 
	}

	void FadeOut(GameObject o)
	{
        if (o.GetComponent<MeshRenderer>() == null)
        {
            return;
        }

		Material material = o.GetComponent<MeshRenderer> ().material;
        if (material.shader == Shader.Find("Standard"))
        {
            StandardShaderUtils.ChangeRenderMode(material, StandardShaderUtils.BlendMode.Transparent);
            Color color = material.color;
            color.a = Alpha;
            material.color = color;
        }
		
	}

	void FadeIn(GameObject o)
	{
        if (o.GetComponent<MeshRenderer>() == null)
        {
            return;
        }

        Material material = o.GetComponent<MeshRenderer> ().material;
        if (material.shader == Shader.Find("Standard"))
        {
            StandardShaderUtils.ChangeRenderMode(material, StandardShaderUtils.BlendMode.Opaque);
            Color color = material.color;
            color.a = 1f;
            material.color = color;
        }
	}
}
