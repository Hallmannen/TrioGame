using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text UItext;

    public float startTime;
    public float CurrentTime;

    private void Start()
    {
        CurrentTime = startTime;
    }
    void Update()
    {
        CurrentTime -= Time.deltaTime;
        UItext.text = CurrentTime.ToString("0");
    }
}
