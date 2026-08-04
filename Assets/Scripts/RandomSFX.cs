using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

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

    void Start()
    {
        nextBirdSound = Time.time + Random.Range(minTimeInbetween, maxTimeInbetween);
        nextWindSound = Time.time + Random.Range(minTimeInbetween, maxTimeInbetween);
        nextRustleSound = Time.time + Random.Range(minTimeInbetween, maxTimeInbetween);
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
    }

    void PlayClip(List<AudioClip> clipList) {
        AudioClip clip = clipList[Random.Range(0, clipList.Count)];
        transform.position = Random.insideUnitSphere * maxSFXDist + playerPos.position;
        source.PlayOneShot(clip);
    }
}
