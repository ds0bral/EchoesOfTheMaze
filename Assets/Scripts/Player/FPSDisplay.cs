using UnityEngine;

public class FPSDisplay : MonoBehaviour
{

    private float fps;
    public TMPro.TextMeshProUGUI FPSCounterText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("GetFPS", 1, 1);
    }


    void GetFPS()
    {
        fps =(int)(1f / Time.unscaledDeltaTime);
        FPSCounterText.text = "FPS: "+ fps.ToString();
    }

}
