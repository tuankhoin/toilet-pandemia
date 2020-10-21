
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlay : MonoBehaviour
{
    // Start is called before the first frame update

    public Image overlayImage;

    private float r;
    private float g;
    private float b;
    private float a;
    private bool isDamage = false;
    public bool updateOn = true;

    private void Start()
    {
        r = overlayImage.color.r;
        g = overlayImage.color.g;
        b = overlayImage.color.b;
        a = overlayImage.color.a;

    }

    // Update is called once per frame
    private void Update()
    {
        if (!isDamage)
        {
            a -= 0.01f;
        }else
        {
            a += 0.01f;
        }
        a = Mathf.Clamp(a, 0, 0.5f);
        AdjustColor();


        if (isDamage && a == 0.5f)
        {
            StartCoroutine(updateOff());
        }

        if (updateOn == false)
        {
            this.SetDamage(false);
            updateOn = true;
        }

    }

    private void AdjustColor()
    {
        Color c = new Color(r, g, b, a);
        overlayImage.color = c;
    }

    public void SetDamage(bool value)
    {
        this.isDamage = value;
    }


    IEnumerator updateOff()
    {
        yield return new WaitForSeconds(2.0f);
        updateOn = false;
    }
}
