using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSound : MonoBehaviour
{
    AudioSource audioSource;
    private NPCPlayer.Characters character;
    private readonly List<NPCPlayer.Characters> women = new List<NPCPlayer.Characters>()
    {
        NPCPlayer.Characters.HeroBlonde,
        NPCPlayer.Characters.HeroGreen,
        NPCPlayer.Characters.HeroPurple,
        NPCPlayer.Characters.Pirate,
        NPCPlayer.Characters.Punk,
        NPCPlayer.Characters.PurpleHat,
        NPCPlayer.Characters.Rapper,
        NPCPlayer.Characters.RedHatBoy
    };

    private readonly string[] numbers = new string[]
    {
        "One", "Two", "Three", "Four"
    };
    private int sound = 0;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(NPCPlayer.Characters character, NPCPlayer.Emotions initialEmotion)
    {
        this.character = character;
        //audioSource.pitch = Mathf.Clamp(((int)character) / 3, 0.5f, 3);
        React(initialEmotion);
    }

    public void React(NPCPlayer.Emotions emotions)
    {
        //Debug.Log("I'll play the sound");
        audioSource.clip = GetClip(emotions);
        //Debug.Log("The sound I found is: " + audioSource.clip);
        audioSource.Play();
    }

    private AudioClip GetClip(NPCPlayer.Emotions emotions)
    {
        string name = $"Sounds/{emotions}_";
        name += $"{(women.Contains(character) ? "Woman" : "Man")}_";
        name += numbers[sound];
        sound++;
        if (sound > 3) sound = 0;

        //Debug.Log("The sound I found is: " + name);
        return Resources.Load<AudioClip>(name);
    }
}
