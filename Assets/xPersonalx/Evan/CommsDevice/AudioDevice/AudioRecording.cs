using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class AudioRecording : MonoBehaviour
{
    public AudioSource audioSource;
    bool isPlaying = false;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.isPressed)
        {
            isPlaying = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!audioSource.isPlaying && isPlaying == true)
        {
            audioSource.Play();
        }
    }
}