using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPLAY : MonoBehaviour
{
    public string LevelName;
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to the GameObject
        audioSource = GetComponent<AudioSource>();
    }

    public void LoadLevel()
    {
        // Play the audio clip
        audioSource.Play();

        // Start a coroutine to load the level after the sound has finished
        StartCoroutine(WaitForSound());
    }

    IEnumerator WaitForSound()
    {
        // Wait until the clip finishes playing
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // Load the scene
        SceneManager.LoadScene(LevelName);
    }
}