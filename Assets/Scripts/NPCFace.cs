using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFace : MonoBehaviour, INPCReaction
{
    public struct LeanTweenInfo
    {
        public enum LeanTweenInfoType
        {
            Nothing, Move, Color
        }
        public LeanTweenInfoType infoType;
        public float time;
        public Vector2 translation;
        public Color color;
        public LeanTweenType easeType;
        public int repetition;
        public bool repeat;

        public LeanTweenInfo(Vector2 translation, float time, LeanTweenType easeType = LeanTweenType.linear, int repetition = -1, bool repeat = false)
        {
            this.infoType = LeanTweenInfoType.Move;
            this.translation = translation;
            this.time = time;
            this.color = Color.black;
            this.easeType = easeType;
            this.repetition = repetition;
            this.repeat = repeat;
        }

        public LeanTweenInfo(Color color, float time, LeanTweenType easeType = LeanTweenType.linear, int repetition = -1, bool repeat = false)
        {
            this.infoType = LeanTweenInfoType.Color;
            this.translation = Vector3.zero;
            this.color = color;
            this.time = time;
            this.easeType = easeType;
            this.repetition = repetition;
            this.repeat = repeat;
        }
    }

    private static string RESOURCES_FOLDER = "Third_Party/Box Toon Characters Pack - Humans/Pngs/";

    private NPCPlayer.Characters character;
    public NPCPlayer.Characters Character
    {
        get
        {
            return character;
        }
    }
    [SerializeField]
    private UnityEngine.UI.Image faceImage;
    private Dictionary<NPCPlayer.Emotions, List<LeanTweenInfo>> infosPerEmotions = new Dictionary<NPCPlayer.Emotions, List<LeanTweenInfo>>();

    void Awake()
    {
        infosPerEmotions.Add(NPCPlayer.Emotions.Generic, new List<LeanTweenInfo>()
        {
            new LeanTweenInfo()
        });
        infosPerEmotions.Add(NPCPlayer.Emotions.Sad, new List<LeanTweenInfo>()
        {
            new LeanTweenInfo(new Vector2(25, 0), 0.5f, LeanTweenType.easeInOutCubic, 3, true)
        });
        infosPerEmotions.Add(NPCPlayer.Emotions.Smirk, new List<LeanTweenInfo>()
        {
            new LeanTweenInfo(new Vector2(0, 25), 0.5f, LeanTweenType.easeInOutElastic, 5, true)
        });
        infosPerEmotions.Add(NPCPlayer.Emotions.Angry, new List<LeanTweenInfo>()
        {
            new LeanTweenInfo(new Vector2(25, 0), 0.08f, LeanTweenType.easeInOutElastic, -1, true),
            new LeanTweenInfo(Color.red, 0.5f, LeanTweenType.easeInOutElastic, -1, true)
        });
        infosPerEmotions.Add(NPCPlayer.Emotions.Smile, new List<LeanTweenInfo>()
        {
            new LeanTweenInfo(new Vector2(0, 25f), 0.1f, LeanTweenType.easeInOutElastic, 5, true),
            new LeanTweenInfo(Color.yellow, 0.5f, LeanTweenType.easeInOutElastic, -1, true)
        });
        infosPerEmotions.Add(NPCPlayer.Emotions.ZZ, new List<LeanTweenInfo>()
        {
        });
    }

    private string GetName(NPCPlayer.Characters character, NPCPlayer.Emotions initialEmotion)
    {
        string spriteName = "";
        switch (character)
        {
            case NPCPlayer.Characters.Afro:
                spriteName += "Afro";
                break;
            case NPCPlayer.Characters.Baldy:
                spriteName += "Baldy";
                break;
            case NPCPlayer.Characters.Clown:
                spriteName += "Clown";
                break;
            case NPCPlayer.Characters.Cop:
                spriteName += "Cop";
                break;
            case NPCPlayer.Characters.FireFighter:
                spriteName += "FireFighter";
                break;
            case NPCPlayer.Characters.GenericLongHair:
                spriteName += "Generic LongHair";
                break;
            case NPCPlayer.Characters.HeroBlonde:
                spriteName += "Hero Blonde";
                break;
            case NPCPlayer.Characters.HeroGreen:
                spriteName += "Hero Green";
                break;
            case NPCPlayer.Characters.HeroPurple:
                spriteName += "Hero Purple";
                break;
            case NPCPlayer.Characters.LongHair:
                spriteName += "LongHair";
                break;
            case NPCPlayer.Characters.OneEyed:
                spriteName += "OneEyed";
                break;
            case NPCPlayer.Characters.Pirate:
                spriteName += "Pirate";
                break;
            case NPCPlayer.Characters.Punk:
                spriteName += "Punk";
                break;
            case NPCPlayer.Characters.PurpleHat:
                spriteName += "Purple Hat Boy";
                break;
            case NPCPlayer.Characters.Rapper:
                spriteName += "Rapper";
                break;
            case NPCPlayer.Characters.RedHatBoy:
                spriteName += "Red Hat Boy";
                break;
            case NPCPlayer.Characters.RedIndian:
                spriteName += "Red Indian";
                break;
            case NPCPlayer.Characters.Soldier:
                spriteName += "Solider";
                break;
            case NPCPlayer.Characters.Thief:
                spriteName += "Thief";
                break;
            case NPCPlayer.Characters.WrestlerA:
                spriteName += "Wrestler A";
                break;
            case NPCPlayer.Characters.WrestlerB:
                spriteName += "Wrestler B";
                break;
        }
        spriteName += " ";
        switch (initialEmotion)
        {
            case NPCPlayer.Emotions.Generic:
                spriteName += "Generic";
                break;
            case NPCPlayer.Emotions.Angry:
                spriteName += "Angry";
                break;
            case NPCPlayer.Emotions.Sad:
                spriteName += "Sad";
                break;
            case NPCPlayer.Emotions.Smile:
                spriteName += "Smile";
                break;
            case NPCPlayer.Emotions.Smirk:
                spriteName += "Smirk";
                break;
            case NPCPlayer.Emotions.ZZ:
                spriteName += "ZZ";
                break;
        }
        return spriteName;
    }

    private void SetFace(NPCPlayer.Characters character, NPCPlayer.Emotions initialEmotion)
    {

        string spriteName = $"{RESOURCES_FOLDER}";
        spriteName += GetName(character, initialEmotion);
        Sprite texture = Resources.Load<Sprite>(spriteName);
        faceImage.sprite = texture;
    }

    public void Initialize(NPCPlayer.Characters character, NPCPlayer.Emotions initialEmotion)
    {
        SetFace(character, initialEmotion);
        SetReaction(initialEmotion);
        this.character = character;
    }

    public void React(NPCPlayer.Emotions emotion)
    {
        SetFace(character, emotion);
        SetReaction(emotion);
    }

    private void SetReaction(NPCPlayer.Emotions emotion)
    {
        LeanTween.cancel(faceImage.gameObject);
        faceImage.color = Color.white;

        LTDescr lTDescr = null;
        foreach(LeanTweenInfo info in infosPerEmotions[emotion])
        {
            if (info.infoType == LeanTweenInfo.LeanTweenInfoType.Nothing) continue;
            switch (info.infoType)
            {
                case LeanTweenInfo.LeanTweenInfoType.Move:
                    if (info.translation.x != 0 && info.translation.y != 0)
                    {
                        lTDescr = LeanTween.move(faceImage.GetComponent<RectTransform>(), info.translation, info.time)
                            .setCanvasMoveX().setCanvasMoveY();
                    }                        
                    else if (info.translation.x != 0)
                        lTDescr = LeanTween.moveX(faceImage.GetComponent<RectTransform>(), info.translation.x, info.time);
                    else if (info.translation.y != 0)
                        lTDescr = LeanTween.moveY(faceImage.GetComponent<RectTransform>(), info.translation.y, info.time);
                    break;
                case LeanTweenInfo.LeanTweenInfoType.Color:
                    lTDescr = LeanTween.color(faceImage.GetComponent<RectTransform>(), info.color, info.time);
                    break;
            }

            if (lTDescr != null)
            {
                lTDescr = lTDescr.setEase(info.easeType);
                if (info.repeat)
                    lTDescr.setLoopPingPong(info.repetition);
            }
        }
        //LeanTween.moveX(faceImage.GetComponent<RectTransform>(), 25, 0.08f).setEase(LeanTweenType.)
    }
}
