using UnityEngine;
using UnityEngine.InputSystem;

public class ExplodetionTriger : MonoBehaviour
{
    public ParticleSystem explosion;
    public CameraShake cameraShake;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            //explosion.Play();
            StartCoroutine(cameraShake.Shake(.15f, .1f));
        }
    }
}
