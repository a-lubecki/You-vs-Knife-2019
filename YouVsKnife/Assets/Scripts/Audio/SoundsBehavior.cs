/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class SoundsBehavior : MonoBehaviour {


    [SerializeField] private AudioClip soundButtonAppear = null;
    [SerializeField] private AudioClip soundCut = null;
    [SerializeField] private AudioClip soundDing = null;
    [SerializeField] private AudioClip soundGameOver = null;
    [SerializeField] private AudioClip soundKnifeAppear = null;
    [SerializeField] private AudioClip[] soundsSplat = null;
    [SerializeField] private AudioClip[] soundsSwoosh = null;
    [SerializeField] private AudioClip soundTargetHide = null;
    [SerializeField] private AudioClip soundTargetShow = null;

    private AudioSource audioSourceDefaut;
    private AudioSource audioSourceSwoosh;


    protected void Awake() {

        audioSourceDefaut = GetComponents<AudioSource>()[0];
        audioSourceSwoosh = GetComponents<AudioSource>()[1];
    }

    private void playClip(AudioSource source, AudioClip clip) {
        source.PlayOneShot(clip);
    }

    private void playRandomClip(AudioSource source, AudioClip[] clips) {
        playClip(source, clips[Random.Range(0, clips.Length)]);
    }

    public void playButtonAppear() {
        playClip(audioSourceDefaut, soundButtonAppear);
    }

    public void playCut() {
        playClip(audioSourceDefaut, soundCut);
    }

    public void playDing() {
        playClip(audioSourceDefaut, soundDing);
    }

    public void playGameOver() {
        playClip(audioSourceDefaut, soundGameOver);
    }

    public void playKnifeAppear() {
        playClip(audioSourceDefaut, soundKnifeAppear);
    }

    public void playSplat() {
        playRandomClip(audioSourceDefaut, soundsSplat);
    }

    public void playSwoosh(float speedPercentage) {

        if (speedPercentage < 0) {
            speedPercentage = 0;
        } else if (speedPercentage > 1) {
            speedPercentage = 1;
        }

        audioSourceSwoosh.volume = speedPercentage * 0.5f;
        audioSourceSwoosh.pitch = 1 + speedPercentage * 0.75f;

        playRandomClip(audioSourceSwoosh, soundsSwoosh);
    }

    public void playTargetHide() {
        playClip(audioSourceDefaut, soundTargetHide);
    }

    public void playTargetShow() {
        playClip(audioSourceDefaut, soundTargetShow);
    }

}
