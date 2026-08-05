using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Experimental.GlobalIllumination;

public class RandomSFX : MonoBehaviour
{
    public List<AudioClip> birdClipList;
    public List<AudioClip> windClipList;
    public List<AudioClip> rustleClipList;

    public float minTimeInbetween;
    public float maxTimeInbetween;

    public float nextBirdSound;
    public float nextWindSound;
    public float nextRustleSound;

    public float maxSFXDist = 20;
    public Transform playerPos;
    public AudioSource source;

    public ParticleSystem rain;
    public float minTimeInbetweenRain;
    public float maxTimeInbetweenRain;
    public float nextRainSwap;
    bool raining = false;

    public Animator anim;
    public int numDaysBeforeMoon;
    public int moonDaySeperation;
    public bool isMoonDay;
    public LakeInteractable lake;
    public GameObject moon;

    void Start()
    {
        nextBirdSound = Time.time + Random.Range(minTimeInbetween, maxTimeInbetween);
        nextWindSound = Time.time + Random.Range(minTimeInbetween, maxTimeInbetween);
        nextRustleSound = Time.time + Random.Range(minTimeInbetween, maxTimeInbetween);
        nextRainSwap = Time.time + Random.Range(minTimeInbetweenRain, maxTimeInbetweenRain);
    }

    void Update()
    {
        if (Time.time > nextBirdSound) {
            PlayClip(birdClipList);
            nextBirdSound = Time.time + Random.Range(minTimeInbetween, maxTimeInbetween);
        }
        if (Time.time > nextWindSound) {
            PlayClip(windClipList);
            nextWindSound = Time.time + Random.Range(minTimeInbetween, maxTimeInbetween);
        }
        if (Time.time > nextRustleSound) {
            PlayClip(rustleClipList);
            nextRustleSound = Time.time + Random.Range(minTimeInbetween, maxTimeInbetween);
        }
        if (Time.time > nextRainSwap)
        {
            if (raining)
                rain.Stop();
            else
                rain.Play();
            raining = !raining;
            if (raining && !isMoonDay)
                lake.water = LakeInteractable.waterType.Rain;
            else if (!raining && !isMoonDay)
                lake.water = LakeInteractable.waterType.Clear;
            nextRainSwap = Time.time + Random.Range(minTimeInbetweenRain, maxTimeInbetweenRain);
        }
    }

    public void NewDay()
    {
        if (lake.water == LakeInteractable.waterType.Moon)
            lake.water = raining ? LakeInteractable.waterType.Rain : LakeInteractable.waterType.Clear;
        numDaysBeforeMoon--;
        moon.SetActive(false);
        if (numDaysBeforeMoon <= 0)
        {
            moon.SetActive(true);
            lake.water = LakeInteractable.waterType.Moon;
            numDaysBeforeMoon = 5;
            isMoonDay = true;
        }
    }

    void PlayClip(List<AudioClip> clipList) {
        AudioClip clip = clipList[Random.Range(0, clipList.Count)];
        transform.position = Random.insideUnitSphere * maxSFXDist + playerPos.position;
        source.PlayOneShot(clip);
    }
}
