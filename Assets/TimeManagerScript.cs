using UnityEngine;

public class TimeManagerScript : MonoBehaviour {
    public float slowDownFactor=0.05f;
    public float slowdownLength = 1f;
    public float pauseLength=0.5f;
    private float x;

    void Start()
    {
        x = pauseLength;
    }
    void Update()
    {
        Time.timeScale += 1 / slowdownLength*Time.unscaledDeltaTime;
       Time.timeScale = Mathf.Clamp(Time.timeScale,0f, 1f);
    }
	public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

}
