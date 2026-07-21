using UnityEngine;

[CreateAssetMenu(fileName = "New building", menuName = "Scriptable Objects/Building")]
public class Building : ScriptableObject
{
    public GameObject BuildingObj;
    public int Priority;
    [Space]
    public int woodCost;
    public int stoneCost;
    public int mudCost;
}
