using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Camera MainCamera;
    public List<GameObject> Players;
    void Awake()
    {
        MakePlayerMoveVissebole transparencycamera = MainCamera.GetComponent<MakePlayerMoveVissebole>();
        NewMultiTargetCamera FolowCamera = MainCamera.GetComponent<NewMultiTargetCamera>();

        foreach (GameObject Player in Players)
        {
            if (Player == null) continue;

            transparencycamera.players.Add(Player.transform);
            //FolowCamera.Targets.Add(Player.transform);
        }
    }
}
