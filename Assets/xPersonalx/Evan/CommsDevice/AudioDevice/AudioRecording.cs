using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class AudioRecording : MonoBehaviour
{
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
        if (isPlaying == true)
        {
            //GetComponent<AudioManager_VoiceOver>().PlayVoiceOver();
        }
    }
}