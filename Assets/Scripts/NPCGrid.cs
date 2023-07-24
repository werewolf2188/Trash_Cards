using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGrid : MonoBehaviour
{
    [SerializeField]
    private NPCFace facePrefab; 
    // Start is called before the first frame update
    void Start()
    {
        System.Array characters = System.Enum.GetValues(typeof(NPCPlayer.Characters));
        for (int i = 0; i < characters.Length; i++)
        {
            NPCFace face = Instantiate<NPCFace>(facePrefab, this.GetComponent<RectTransform>(), false);
            face.Initialize((NPCPlayer.Characters)characters.GetValue(i), NPCPlayer.Emotions.Generic);
        }
    }
}
