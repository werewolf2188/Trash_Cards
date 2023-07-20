using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class NPCSpeech : MonoBehaviour, INPCReaction
{
    [SerializeField]
    private TextMeshProUGUI textSpeech;
    [SerializeField]
    [Range(1, 60)]
    private int numberOfFramesToWait = 1;

    private Dictionary<NPCPlayer.Emotions, List<string>> phrases;
    // Start is called before the first frame update
    void Awake()
    {
        textSpeech.text = "";
        phrases = new Dictionary<NPCPlayer.Emotions, List<string>>();
        phrases.Add(NPCPlayer.Emotions.Angry, new List<string>()
        {
            "Why you!!",
            "I hate this game!",
            "$@%#@!^!@%",
            "Oh you know what! Go #!#$ yourself",
            "Ok, enough is enough!"
        });
        phrases.Add(NPCPlayer.Emotions.Generic, new List<string>()
        {
            "Let's get this over with.",
            "I'm bored",
            "Don't you have anything else to do?",
            "Why me?!",
            "You should consider something else to do."
        });
        phrases.Add(NPCPlayer.Emotions.Sad, new List<string>()
        {
            "This sucks!",
            "Oh man, what am I going to do now?",
            "My lunch money!",
            "Why are you so mean?",
            "I thought we were going to play nice."
        });
        phrases.Add(NPCPlayer.Emotions.Smile, new List<string>()
        {
            "Haha! This is the best day of my life!",
            "Nothing can stop me now!",
            "I'm the best!!!",
            "I'm sooo going to buy that TV now!",
            "Nobody can stop me!"
        });
        phrases.Add(NPCPlayer.Emotions.Smirk, new List<string>()
        {
            "Well, well, well!",
            "All the way to the bank.",
            "Gotcha!",
            "Hehe... cool, cool...",
            "It must be my day!"
        });
        phrases.Add(NPCPlayer.Emotions.ZZ, new List<string>()
        {
            "Zzzzz"
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Talk(string sentence)
    {
        string text = "";

        foreach(char character in sentence)
        {
            text += character;
            for(int x = 0; x < numberOfFramesToWait; x++)
                yield return null;
            textSpeech.text = text;
        }
    }

    public void Initialize(NPCPlayer.Characters character, NPCPlayer.Emotions initialEmotion)
    {
        StartCoroutine(Talk(phrases[initialEmotion].GetRandom()));
    }

    public void React(NPCPlayer.Emotions emotion)
    {
        StartCoroutine(Talk(phrases[emotion].GetRandom()));
    }
}
