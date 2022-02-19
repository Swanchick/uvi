using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FootStepsSystem : MonoBehaviour
{   
    [SerializeField] private Player Player;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private FootStepsCollection[] FootStepsCollections;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private float DelaySpeed = 0.9f;
    private bool AudioClipIsChanged = false;
    private bool IsSteping = true;
    private bool ChangeLeg = false;

    private FootStepsCollection collection;

    private void Update()
    {
        GetRay();
    }

    private void GetRay()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, GroundLayer))
        {
            string textureName = string.Empty;
            
            if (hit.collider.GetComponent<Terrain>())
            {
                textureName = GetTextureNameTerrain(hit, hit.collider.GetComponent<Terrain>());
            }
            else
            {
                textureName = GetMeshMaterialAtPoint(transform.position, hit);
            }

            if (textureName == null) return;

            ChangeAudioClip(textureName);
            FootStepsSound();
        }
    }

    private string LastTextureName;

    private void ChangeAudioClip(string textureName)
    {
        if (LastTextureName == textureName) return;

        LastTextureName = textureName;
        
        if (GetCollection(textureName) == null)
        {
            AudioSource.clip = null;
            AudioClipIsChanged = true;
            return;
        }

        collection = GetCollection(textureName);
        AudioClipIsChanged = true;

        if (ChangeLeg)
            AudioSource.clip = collection.RightFootStep;
        else
            AudioSource.clip = collection.LeftFootStep;

        ChangeLeg = !ChangeLeg;
    }

    private FootStepsCollection GetCollection(string name)
    {
        foreach (FootStepsCollection _collection in FootStepsCollections)
        {
            if (_collection.name == name)
                return _collection;
        }
        return null;
    }

    private void FootStepsSound()
    {
        if (!Player.IsMoving() || !Player.PlayerGrounded)
        {
            AudioSource.Stop();
            return;
        }


        if (AudioSource.isPlaying) return;
        if (!IsSteping || collection == null) return;

        StartCoroutine(DelaySteps());
    }

    private float[] GetTextureMixTerrain(RaycastHit hit, Terrain t)
    {
        Vector3 tPos = t.transform.position;
        TerrainData tData = t.terrainData;

        int posX = Mathf.RoundToInt((hit.point.x - tPos.x) / tData.size.x * tData.alphamapWidth);
        int posZ = Mathf.RoundToInt((hit.point.z - tPos.z) / tData.size.z * tData.alphamapHeight);

        float[,,] splatMapData = tData.GetAlphamaps(posX, posZ, 1, 1);
        float[] cellmix = new float[splatMapData.GetUpperBound(2) + 1];

        for (int i=0; i < cellmix.Length; i++)
        {
            cellmix[i] = splatMapData[0, 0, i];
        }

        return cellmix;
    }

    private string GetTextureNameTerrain(RaycastHit hit, Terrain t)
    {
        float[] cellmix = GetTextureMixTerrain(hit, t);
        float strongest = 0;
        int maxIndex = 0;

        for (int i = 0; i < cellmix.Length; i++)
        {
            if (cellmix[i] > strongest)
            {
                maxIndex = i;
                strongest = cellmix[i];
            }
        }
        
        return t.terrainData.terrainLayers[maxIndex].diffuseTexture.name;
    }

    private string GetMeshMaterialAtPoint(Vector3 worldPosition, RaycastHit hit) {
        Renderer r = hit.collider.GetComponent<Renderer>();
        MeshCollider mc = hit.collider as MeshCollider;

        if (r == null || r.sharedMaterial == null || r.sharedMaterial.mainTexture == null || r == null) 
        {
            return "";
        }
        else if(!mc || mc.convex) 
        {
            return r.material.name;
        }

        int materialIndex = -1;
        Mesh m = mc.sharedMesh;
        int triangleIdx = hit.triangleIndex;
        int lookupIdx1 = m.triangles[triangleIdx * 3];
        int lookupIdx2 = m.triangles[triangleIdx * 3 + 1];
        int lookupIdx3 = m.triangles[triangleIdx * 3 + 2];
        int subMeshesNr = m.subMeshCount;

        for(int i = 0;i < subMeshesNr;i ++) 
        {
            int[] tr = m.GetTriangles(i);

            for(int j = 0;j < tr.Length;j += 3) 
            {
                if (tr[j] == lookupIdx1 && tr[j+1] == lookupIdx2 && tr[j+2] == lookupIdx3) 
                {
                    materialIndex = i;

                    break;
                }
            }

            if (materialIndex != -1) break;
        }

        string textureName = r.materials[materialIndex].name;

        return textureName.Split()[0];
    }

    private int a = 0;

    private IEnumerator DelaySteps()
    {
        while (Player.IsMoving() && !Player.IsPaused && !Player.IsDead)
        {
            AudioSource.Play();
            IsSteping = false;
            

            yield return new WaitForSeconds(DelaySpeed);

            IsSteping = true;
            if (ChangeLeg)
                AudioSource.clip = collection.RightFootStep;
            else
                AudioSource.clip = collection.LeftFootStep;
            
            ChangeLeg = !ChangeLeg;
        }
    }
}
