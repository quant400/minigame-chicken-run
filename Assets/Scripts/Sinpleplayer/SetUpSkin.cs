using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SetUpSkin : MonoBehaviour
{

   Material mat;

    public void SetUpChar(string n)
    {
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        GameObject skin = Resources.Load(Path.Combine("SinglePlayerPrefabs/DisplayModels", n)) as GameObject;
        var newMeshRenderer = skin.GetComponentInChildren<SkinnedMeshRenderer>();
        Debug.Log(newMeshRenderer.name);
        mat.mainTexture = newMeshRenderer.sharedMaterial.mainTexture;

        GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = newMeshRenderer.sharedMesh;

        Transform[] childrens = transform.GetComponentsInChildren<Transform>(true);

        // sort bones.
        Transform[] bones = new Transform[newMeshRenderer.bones.Length];
        for (int boneOrder = 0; boneOrder < newMeshRenderer.bones.Length; boneOrder++)
        {
            bones[boneOrder] = System.Array.Find<Transform>(childrens, c => c.name == newMeshRenderer.bones[boneOrder].name);
        }
        GetComponentInChildren<SkinnedMeshRenderer>().bones = bones;
    }
}
