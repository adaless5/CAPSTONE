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

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        scriptManager = FindObjectOfType<ScriptManager>();  //expensive call
        guiManager = FindObjectOfType<SubtitleGuiManager>();
    }

    void Update()
    {
        if (Keyboard.current.eKey.isPressed)
        {
            isPlaying = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isPlaying == true)
        {
            StartCoroutine(DisplaySubtitles());
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