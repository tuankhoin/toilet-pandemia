using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionController : MonoBehaviour
{
    public void GoBack() {
        SceneManager.LoadScene("MenuScreen");
    }
}
