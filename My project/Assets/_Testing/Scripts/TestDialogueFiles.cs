using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TestDialogueFiles : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextAsset fileToRead = null;
   void Start()
    {
        StartConversation();
    }

   
    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset(fileToRead);

        DialogueSystem.instance.Say(lines);

    }
}
