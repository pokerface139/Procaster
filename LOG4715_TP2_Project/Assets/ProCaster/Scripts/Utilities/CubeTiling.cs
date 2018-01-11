using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class CubeTiling : MonoBehaviour {

    private Material texture1;
    private Material texture2;
    private Mesh mesh;
    private Vector2[] uvs;
    private Vector3 lastScale;

	// Use this for initialization
	void Start () {

   
        texture1 = new Material(GetComponent<MeshRenderer>().sharedMaterials[0]);
        texture2 = new Material(GetComponent<MeshRenderer>().sharedMaterials[1]);
        lastScale = transform.lossyScale;

        uvs = GetComponent<MeshFilter>().sharedMesh.uv;
        ShowUV(uvs);

        mesh = Mesh.Instantiate(GetComponent<MeshFilter>().sharedMesh);
        GetComponent<MeshFilter>().mesh = mesh;
        
        UpdateTiling();

    }

    public void ShowUV(Vector2[] uvs)
    {
        foreach (Vector2 uv in uvs)
        {
            Debug.Log(uv);
        }
    }
   
    public void UpdateTiling()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
       
        // Update top
        Vector2 scale = new Vector2(transform.lossyScale.z, transform.lossyScale.x);
        texture2.mainTextureScale = scale;
        renderer.materials[1] = texture2;

        // Update other faces
        Vector2[] temp = CalculateUvs(transform.lossyScale);

        mesh.uv = temp;
        lastScale = transform.lossyScale;
    }

    public Vector2[] CalculateUvs(Vector3 scale)
    {
        Vector2[] temp = new Vector2[mesh.vertexCount];

        float xScale = scale.x;
        float yScale = scale.y;
        float zScale = scale.z;

        //Bottom
        temp[0] = new Vector2(zScale * uvs[0].x, xScale * uvs[0].y);
        temp[1] = new Vector2(zScale * uvs[1].x, xScale * uvs[1].y);
        temp[2] = new Vector2(zScale * uvs[2].x, xScale * uvs[2].y);
        temp[3] = new Vector2(zScale * uvs[3].x, xScale * uvs[3].y);

        // Front
        temp[4] = new Vector2(zScale * uvs[4].x, yScale * uvs[4].y);
        temp[5] = new Vector2(zScale * uvs[5].x, yScale * uvs[5].y);
        temp[6] = new Vector2(zScale * uvs[6].x, yScale * uvs[6].y);
        temp[7] = new Vector2(zScale * uvs[7].x, yScale * uvs[7].y);

        // Left
        temp[8] = new Vector2(xScale * uvs[8].x, yScale * uvs[8].y);
        temp[9] = new Vector2(xScale * uvs[9].x, yScale * uvs[9].y);
        temp[10] = new Vector2(xScale * uvs[10].x, yScale * uvs[10].y);
        temp[11] = new Vector2(xScale * uvs[11].x, yScale * uvs[11].y);

        // Back
        temp[12] = new Vector2(zScale * uvs[12].x, yScale * uvs[12].y);
        temp[13] = new Vector2(zScale * uvs[13].x, yScale * uvs[13].y);
        temp[14] = new Vector2(zScale * uvs[14].x, yScale * uvs[14].y);
        temp[15] = new Vector2(zScale * uvs[15].x, yScale * uvs[15].y);

        // Right
        temp[16] = new Vector2(xScale * uvs[16].x, yScale * uvs[16].y);
        temp[17] = new Vector2(xScale * uvs[17].x, yScale * uvs[17].y);
        temp[18] = new Vector2(xScale * uvs[18].x, yScale * uvs[18].y);
        temp[19] = new Vector2(xScale * uvs[19].x, yScale * uvs[19].y);

        // Top
        temp[20] = uvs[20];
        temp[21] = uvs[21];
        temp[22] = uvs[22];
        temp[23] = uvs[23];

        return temp;
    }

    public void Revert()
    {
        mesh.uv = uvs;

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Vector2 scale  = new Vector2(1, 1);
        renderer.sharedMaterials[1].mainTextureScale = scale;
    }
}
