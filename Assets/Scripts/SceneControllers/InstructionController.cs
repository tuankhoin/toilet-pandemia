using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionController : MonoBehaviour
{
    public void GoBack() {
        SceneManager.LoadScene("MenuScreen");
    }
}
