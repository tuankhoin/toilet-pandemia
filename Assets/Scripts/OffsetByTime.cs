using UnityEngine;

// C# method to make the desired object rotate its texture in cycles, imitating a cyclone effect
// The equivalent method using shader only is demonstrated in FogShader.shader
public class OffsetByTime : MonoBehaviour
{
    [Range(-50.0f,50.0f)] public float xFlow = 5.0f;
    [Range(-50.0f,50.0f)] public float yFlow = 0.0f;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        rend.material.mainTextureOffset -= new Vector2(xFlow, yFlow)*Time.deltaTime/10.0f;
    }
}
