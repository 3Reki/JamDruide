using UnityEngine;

public class MusicInstance : MonoBehaviour
{

    public static MusicInstance instance;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SwapMusicClip(AudioClip clip, float volume)
    {
        if (musicSource.clip == clip)
        {
            return;
        }
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }
}
