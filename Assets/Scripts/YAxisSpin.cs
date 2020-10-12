using UnityEngine;
using System.Collections;

public class YAxisSpin : MonoBehaviour {
    
    public float spinSpeed;
        	
	// Update is called once per frame
	void Update () {
		this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * spinSpeed, new Vector3(0.0f, 1.0f, 0.0f));
	}
}
