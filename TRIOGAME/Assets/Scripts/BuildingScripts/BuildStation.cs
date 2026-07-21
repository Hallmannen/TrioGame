using UnityEngine;

public class BuildStation : MonoBehaviour
{
    public Building[] allBuildingStages;
    [Space]
    public BuildManager buildManager;
    public float MaterialDistans = 3;

    private int buildProgres = 0;
    public GameObject currentBuildObj;
    [Space]
    public int[] currentMaterials = new int[] { 0, 0, 0 };
    public int[] neededMaterials = new int[] { 0, 0, 0 };
    [Space]
    public GameObject CompletPartical;
    [Space]
    public bool buildingComplet = false;
    private void Awake()
    {
        //buildProgres++;
        buildManager = FindAnyObjectByType<BuildManager>();
        calculateBuildingCost();
    }

    void Update()
    {
        int L = buildManager.allAvailibleMaterial.Count;

        for (int i = 0; i < L; i++)
        {
            
            Transform MatTrans = buildManager.allAvailibleMaterial[i]; //<-- needed and availible material transforms
            float dis = Vector3.Distance(MatTrans.position, transform.position);
            if (dis <= MaterialDistans)
            {
                MaterialScript MScript = buildManager.allAvailibleMaterial[i].GetComponent<MaterialScript>();
                for (int x = 0; x < currentMaterials.Length; x++)
                {
                    int matNumber = MScript.ArrayIdx;
                    if (matNumber == x) currentMaterials[x] += MScript.Amount;
                }

                buildManager.allAvailibleMaterial.RemoveAt(i);
                Destroy(MScript.gameObject);
                Build();
                
            }
        }
    }

    private void calculateBuildingCost()
    {
        for (int i = 0; i < currentMaterials.Length; i++)
        {
            if (i == 0) neededMaterials[i] = allBuildingStages[buildProgres].woodCost;
            if (i == 1) neededMaterials[i] = allBuildingStages[buildProgres].stoneCost;
            if (i == 2) neededMaterials[i] = allBuildingStages[buildProgres].mudCost;
        }
    }

    private void Build()
    {
        int MaterialCheck = 0;
        for (int i = 0; i < currentMaterials.Length; i++)
        {
            if (currentMaterials[i] >= neededMaterials[i])
            {
                MaterialCheck++;
            }
        }
        if(MaterialCheck == currentMaterials.Length)
        {

            
            if(currentBuildObj != null) Destroy(currentBuildObj);
            currentBuildObj = Instantiate(allBuildingStages[buildProgres].BuildingObj, transform.position, Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z));

            for (int i = 0; i < currentMaterials.Length; i++)
            {
                currentMaterials[i] = 0; // all materials set to 0
            }

            buildProgres++;
            if (buildProgres == allBuildingStages.Length)
            {
                GameObject newPartical = Instantiate(CompletPartical, transform.position, Quaternion.identity);
                Destroy(newPartical, 6);
                Destroy(this.GetComponentInChildren<Transform>().gameObject);
                buildingComplet = true;
                buildManager.checkAllBuildingStatus();
                this.enabled = false;
            }
            else
            {
                calculateBuildingCost();
            }
        }

    }
}
