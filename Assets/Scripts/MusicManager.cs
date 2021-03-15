using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public float voluemSound;
    public float voluemMusic;
    public AudioSource sound;
    public AudioSource music;
    public Slider sliderSound;
    public Slider sliderMusic;

    public void SetVoluemSound()
    {
        voluemSound = sliderSound.value;
        if(sound != null)
            sound.volume = voluemSound;
    }
    public void SetVoluemMusic()
    {
        voluemMusic = sliderMusic.value;
        music.volume = voluemMusic;
    }

    // Start is called before the first frame update
    void Start()
    {
        voluemSound = 1.0f;
        voluemMusic = 1.0f;
        sound = GameObject.Find("Sound")?.GetComponent<AudioSource>();
        music = GameObject.Find("Music").GetComponent<AudioSource>();

        if(sound != null)
            sound.volume = voluemSound;
        
        music.volume = voluemMusic;
        sliderSound.value = voluemSound;
        sliderMusic.value = voluemMusic;
    }
}
