using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class PlaySubtitles : MonoBehaviour
{
    public AudioSource audioSource;
    private ScriptManager scriptManager;
    private SubtitleGuiManager guiManager;
    bool isPlaying = false;
    ALTPlayerController controller;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        scriptManager = FindObjectOfType<ScriptManager>();  //expensive call
        guiManager = FindObjectOfType<SubtitleGuiManager>();
        controller = FindObjectOfType<ALTPlayerController>();
    }

    void Update()
    {
        if (controller == null)
        {
            controller = FindObjectOfType<ALTPlayerController>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindObjectOfType<InteractableText>().b_inInteractCollider = true;
            if (controller.CheckForInteract())
            {
                StartCoroutine(DisplaySubtitles());
                GetComponent<AudioManager_VoiceOver>().PlayVoiceOver();
            }

        }
    }

    private IEnumerator DisplaySubtitles()
    {
        var script = scriptManager.GetText(audioSource.clip.name);  //get text
        var lineDuration = audioSource.clip.length / script.Length; //script size cannot equal 0, need to have a script in the lines array in the text file

        foreach (var line in script)
        {
            guiManager.SetText(line); //set text
            yield return new WaitForSeconds(lineDuration);
        }

        guiManager.SetText(string.Empty);   //clear text to empty
        isPlaying = false;
    }
}