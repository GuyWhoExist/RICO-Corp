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
        DontDestroyOnLoad(transform.gameObject);
        used = false;
        DuplicateRemoval();
        //MusicClass[] duplicates = FindObjectsByType<MusicClass>(FindObjectsSortMode.None);
        //if (duplicates.Length > 1)
        //{
        //    foreach (MusicClass l in duplicates)
        //    {
        //        if (l.used == false)
        //        {
        //            Debug.Log("tset++");
        //            test++;
        //        }
        //    }
        //    if (test == duplicates.Length)
        //    {
        //        duplicates[0].used = true;
        //        Debug.Log("music set to used");
        //    }
        //    foreach (MusicClass l in duplicates)
        //    {
        //        if (l.used == false)
        //        {
        //            Debug.Log("More than 1 MusicClass found, killing the unused ones"); //same process we use for the tracker we use for the music class - Nova
        //            Destroy(l.gameObject);
        //        }
        //    }
        //}
        //else
        //{
        //    DontDestroyOnLoad(transform.gameObject);
        //}
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

    private void DuplicateRemoval()
    {
        MusicClass[] duplicates = FindObjectsByType<MusicClass>(FindObjectsSortMode.None);
        if (duplicates.Length > 1) //checks for duplicates and destroys them. - Nova
        {
            foreach (MusicClass m in duplicates)
            {
                if (m.used == false && duplicates.Length - 1 != 0)
                {
                    Debug.Log("More than 1 musicClass found, killing the unused ones");
                    Destroy(m.gameObject);
                }
            }
        }
        else if (duplicates.Length == 1)
        {
            used = true;
        }
    }

}