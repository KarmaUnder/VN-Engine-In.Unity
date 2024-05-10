using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DIALOGUE;
using UnityEngine;

namespace TESTING
{
public class TestParser : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        SendFileToParse();
    }

   
    void SendFileToParse()
    {
        List<string> lines = FileManager.ReadTextAsset("textFile");

        foreach(string line in lines){
            if(line == string.Empty)
            continue;
            DIALOGUE_LINE dl = Dialogue_Parser.Parse(line);
        }

    }
}

}
