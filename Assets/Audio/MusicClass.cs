using UnityEngine;

public class MusicClass : MonoBehaviour
{
    //used to start and stop music - Nova
    //Coded by Nova

    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Stop();
        used = false;
        MusicClass[] duplicates = FindObjectsByType<MusicClass>(FindObjectsSortMode.None);
        if (duplicates.Length > 1)
        {
            foreach (MusicClass l in duplicates)
            {
                if (l.used == false)
                {
                    Debug.Log("More than 1 MusicClass found, killing the unused ones"); //same process we use for the tracker we use for the music class - Nova
                    Destroy(l.gameObject);
                }
            }
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    public bool used;

    public void PlayMusic()
    {
        used = true;
        if (_audioSource != null )
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
                Debug.Log("Music is now playing (music class)");
            }
            else
            {
                Debug.Log("Music is already playing (music class)");
                return;
            }
        }  
    }

    public void StopMusic()
    {
        if (_audioSource != null )
        {
            _audioSource.Stop();
            Debug.Log("Music is stopped");
        }   
    }

}