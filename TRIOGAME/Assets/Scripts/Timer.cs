using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text UItext;

    public float startTime;
    public float CurrentTime;

    private BuildManager BM;

    private void Start()
    {
        CurrentTime = startTime;
        BM = FindAnyObjectByType<BuildManager>();
    }
    void Update()
    {

        if(CurrentTime > 0)
        {
            if (!BM.AllBuildingsBuilt)
            {
                if (CurrentTime < 10) UItext.color = Color.yellow;
                CurrentTime -= Time.deltaTime;
                UItext.text = CurrentTime.ToString("0");
            }
            else
            {
                UItext.text = "you are the G.O.A.T and built the thing in time with " + CurrentTime.ToString("0") + " second to spare! :D";
                UItext.color = Color.green;
            }

        }
        else
        {
            UItext.text = "you suck and failed to build the thing in time! >:(";
            UItext.color = Color.red;

        }
    }
}
