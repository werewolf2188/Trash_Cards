using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NPCFace))]
public class NPCMenu : MonoBehaviour
{
    public void SelectNPC()
    {
        NPCFace face = GetComponent<NPCFace>();
        Debug.Log(face.Character);
        NPCInfo.Default.SetFace(face.Character);
        SceneManager.LoadScene((int)TrashCardsScenes.Main);
    }
}
