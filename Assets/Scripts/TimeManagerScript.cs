using System.Collections;
using UnityEngine;

public class TimeManagerScript : MonoBehaviour {
    public float slowDownFactor=0.00001f;
    public float slowdownLength = 1f;



    void Start()
    {
  
    }
    void Update()
    {
        Time.timeScale += (1 / slowdownLength)*Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale,0f, 1f);
        if (Time.timeScale < 0)
        {
            Debug.Log(Time.timeScale);
        }
    }
	public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    public IEnumerator Pause(int p)
    {
        Debug.Log("Started");
        Time.timeScale = 0.1f;
        float pauseEndTime = Time.realtimeSinceStartup + 1;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1;
    }

}
