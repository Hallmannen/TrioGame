using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{
    public int treeHP = 4;
    public int maxTreeHP;
    public float ChoppBarValue;
    public Transform treeStump;
    public GameObject treeStumpObj;
    public GameObject falingTreeObj;
    private readonly float TreeSpawnYoffset = 2.9669368f; // this offset is so the tree is not in the ground when i spawnds a new tree
    [HideInInspector]
    void Start()
    {
        transform.position += Vector3.up * TreeSpawnYoffset;
        maxTreeHP = treeHP;
    }
    public void choopTree()
    {
        treeHP--;

        if (treeHP <= 0)
        {
            Destroy(gameObject);
            SpawnTreeparts();
            Debug.Log("Chooping down tree! ");
        }
    }
    void SpawnTreeparts()
    {
        Instantiate(treeStumpObj, treeStump.position, Quaternion.Euler(0, 45, 0));
        Instantiate(falingTreeObj, transform.position + transform.up, Quaternion.Euler(0, 45, 0));
    }
}
