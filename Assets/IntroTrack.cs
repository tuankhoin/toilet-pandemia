using UnityEngine;

public class IntroTrack : MonoBehaviour
{
    public static IntroTrack currentTrack = null;
    private AudioSource _audioSource;
    private void Awake()
    {
        //GameObject.FindGameObjectWithTag("Soundtrack").GetComponent<IntroTrack>().StopMusic();
        if (currentTrack != null) {
            Destroy(gameObject);
            return;
        } else {
            currentTrack = this;
        }
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
