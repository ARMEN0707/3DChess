using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static float voluemSound = 1.0f;
    public static float voluemMusic = 1.0f;
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
        if(sound != null)
            sound.volume = voluemSound;
        
        music.volume = voluemMusic;
        sliderSound.value = voluemSound;
        sliderMusic.value = voluemMusic;
    }
}
