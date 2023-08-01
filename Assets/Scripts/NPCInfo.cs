using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInfo : MonoBehaviour
{ 
    private static NPCInfo _default;
    public static NPCInfo Default
    {
        get
        {
            return _default;
        }
    }

    private NPCPlayer.Characters character;
    // Start is called before the first frame update
    void Awake()
    {
        if (_default != null)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        _default = this;
    }

    public void SetFace(NPCPlayer.Characters character)
    {
        this.character = character;
    }

    public NPCPlayer.Characters GetFace()
    {
        return character;
    }
}
