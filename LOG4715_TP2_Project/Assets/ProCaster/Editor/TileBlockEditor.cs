using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileBlockEditor : ScriptableWizard {

    public GameObject Block;
    public Vector3 Tiling = Vector3.one;
    public bool WithCollider = false;
    public string ObjectName = "TiledBlock";

    [MenuItem ("My Tools/Tile Block...")]
    static void CreateWindow()
    {
        ScriptableWizard.DisplayWizard<TileBlockEditor>("Tile Block", "Tile","UpdateTiling");
    }

    private void OnWizardCreate()
    {
        CreateTileObject();
    }

    private void OnWizardOtherButton()
    {
        if (Selection.gameObjects.Length == 1)
        {
            Transform transform = Selection.activeGameObject.transform;
            GameObject tileObject = CreateTileObject();
            tileObject.transform.position = transform.position;
            tileObject.transform.rotation = transform.rotation;
            tileObject.transform.localScale = transform.localScale;
            tileObject.name = transform.gameObject.name;
            tileObject.transform.SetParent(Selection.activeTransform.parent);

            DestroyImmediate(Selection.activeObject);
            
        }
    }


    private GameObject CreateTileObject()
    {
        GameObject parent = new GameObject();
        parent.name = ObjectName;
        parent.layer = LayerMask.NameToLayer("Floor");

        Vector3 size = Block.GetComponent<MeshRenderer>().bounds.size;
        bool hasCollider = Block.GetComponent<Collider>() != null;

        // Tiling
        for (int i = 0; i < Tiling.x; ++i)
        {
            for (int j = 0; j < Tiling.y; ++j)
            {
                for (int k = 0; k < Tiling.z; ++k)
                {
                    Vector3 offset = Vector3.Scale(size / 2f, Vector3.forward);
                    Vector3 position = Vector3.Scale(size, new Vector3(i, j, k)) + offset;
                    GameObject instance = Instantiate(Block, position, Quaternion.identity, parent.transform);
                    instance.isStatic = true;
                    instance.layer = LayerMask.NameToLayer("Floor");

                    if (WithCollider && hasCollider)
                    {
                        DestroyImmediate(instance.GetComponent<Collider>());
                    }
                }
            }
        }

        // Create global collider
        if (WithCollider)
        {
            BoxCollider collider = parent.AddComponent<BoxCollider>();
            collider.size = Vector3.Scale(Tiling, size);
            collider.center = Vector3.Scale(Vector3.Scale(Tiling, size),Vector3.forward) / 2f;
        }

        parent.isStatic = true;

        return parent;
    }


    private void OnWizardUpdate()
    {
        isValid = Block != null;

    }
}
