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
    }

    void Update()
    {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            ALTPlayerController.instance._InInteractionVolume = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindObjectOfType<InteractableText>().b_inInteractCollider = true;
            if (ALTPlayerController.instance.CheckForInteract())
            {
                StartCoroutine(DisplaySubtitles());
                GetComponent<AudioManager_VoiceOver>().PlayVoiceOver();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            ALTPlayerController.instance._InInteractionVolume = false;
    }

    private IEnumerator DisplaySubtitles()
    {
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }
        if (guiManager == null)
        {
            guiManager = FindObjectOfType<SubtitleGuiManager>();
        }

        if (scriptManager != null)
        {
            var script = scriptManager.GetText(audioSource.clip.name);  //get text

            var lineDuration = audioSource.clip.length / script.Length; //script size cannot equal 0, need to have a script in the lines array in the text file

            if (guiManager != null)
            {
                foreach (var line in script)
                {
                    guiManager.SetText(line); //set text
                    yield return new WaitForSeconds(lineDuration);
                }

                guiManager.SetText(string.Empty);   //clear text to empty
                isPlaying = false;
            }
        }
    }
}