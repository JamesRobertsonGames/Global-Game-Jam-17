using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

    private float timer;
    public GameObject activeObject;
    public GameObject inactiveObject;

    public bool active;

    // Use this for initialization
    void Start()
    {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update()
    {
        timer += Time.deltaTime * 2;

        float scale = 0.9f + Mathf.Sin(timer) * 0.1f;
        transform.localScale = new Vector3(scale, scale, scale);
	}

    public void SetActive(bool b)
    {
        active = b;
        activeObject.SetActive(b);
        inactiveObject.SetActive(!b);
    }
}
