using UnityEngine;

public class MaterialScript : MonoBehaviour
{
    //[HideInInspector]
    public int ArrayIdx = 0;
    [HideInInspector]
    public string[] MaterialArray = new string[] { "wood", "stone", "mud" };
    public int Amount = 1;

    [HideInInspector]
    private BuildManager BM;

    private void Awake()
    {
        Debug.Log("working!!!");
        BM = FindAnyObjectByType<BuildManager>();
        BM.AddToMaterialList(this.transform);
    }

}
