using UnityEngine;
using UnityEngine.InputSystem;

public class Player_movment : MonoBehaviour
{
    public float Mspeed = 3;
    public Rigidbody RB;
    public CharacterController CC;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 speed = new Vector3(0, 0, 0);
        if (Input.GetKey("s")) speed.z = -1;
        if (Input.GetKey("w")) speed.z = 1;
        if (Input.GetKey("a")) speed.x = -1;
        if (Input.GetKey("d")) speed.x = 1;


        CC.SimpleMove(speed * Mspeed);
    }
}
