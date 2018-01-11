using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlaneTextureTiling : MonoBehaviour {

	public float XRatioPerUnit = 1f;
	public float ZRatioPerUnit = 1f;
    public int materialNumber = 0;

	// Use this for initialization
	void Start () {
		UpdateTiling ();
	}

	[ContextMenu("UpdateTiling")]
	public void UpdateTiling()
	{
		MeshRenderer renderer = GetComponent<MeshRenderer> ();
		Vector2 scale = new Vector2 (transform.lossyScale.x * XRatioPerUnit,transform.lossyScale.z * ZRatioPerUnit);
		renderer.sharedMaterials[materialNumber].mainTextureScale = scale;
	}
}
