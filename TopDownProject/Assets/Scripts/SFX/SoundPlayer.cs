using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{

    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    private AudioSource audioSource;

    private void Awake() => audioSource = GetComponent<AudioSource>();

    public void PlaySound()
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }

}