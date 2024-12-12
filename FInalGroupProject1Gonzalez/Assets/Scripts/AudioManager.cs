using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    // Start is called before the first frame update
    public AudioClip background;
    public AudioClip death;
    public AudioClip jump;
    public AudioClip walk;
    public AudioClip TP;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

private void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}