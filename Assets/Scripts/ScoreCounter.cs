using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ScoreCounter : MonoBehaviour
{
    public Text scoreText;
    public AudioClip ScoringSound;

    int score = 0;
    public int Score
    {
        get {return score;}
        set 
        { 
            score = value; 
            scoreText.text = score.ToString();
            audioSource.PlayOneShot(ScoringSound);
        }
    }

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        score = 0;
        scoreText.text = "0"; 
    }
}
