using UnityEngine;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    public BuildStation[] allBuildStation;

    public List<Transform> allAvailibleMaterial;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void AddToMaterialList(Transform MaterialTransform)
    {
        allAvailibleMaterial.Add(MaterialTransform);
    }
}
