using UnityEngine;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    public bool AllBuildingsBuilt = false;

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

    public void checkAllBuildingStatus()
    {
        bool BuildCheck = true;
        for (int i = 0; i < allBuildStation.Length; i++)
        {
            if (allBuildStation[i].buildingComplet == false)
            {
                BuildCheck = false;
            }
        }

        if (BuildCheck) AllBuildingsBuilt = true;
    }
}
