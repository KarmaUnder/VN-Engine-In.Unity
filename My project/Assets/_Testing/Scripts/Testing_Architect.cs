using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using DIALOGUE;


namespace TESTING
{

public class Testing_Architect : MonoBehaviour
{
    DialogueSystem ds;
    TextArchitect architect;

    public TextArchitect.BuildMethod bm = TextArchitect.BuildMethod.instant;

    string [] lines = new string [5]{
        "Este es un dialogo de puebas",
        "Hola mundo! como van esas pruebas?",
        "Supongo que bien si llegaste a la mitad",
        "Ya casi se terminan las pruebas, solo una mas!",
        "Felicidades, terminaste las pruebas, ahora a seguir adelante"
    };



    // Start is called before the first frame update
    void Start()
    {
        ds = DialogueSystem.instance;
        architect = new TextArchitect(ds.dialogueContainer.dialogueText);
        architect.buildMethod = TextArchitect.BuildMethod.typewriter;
    }

    // Update is called once per frame
    void Update()
    {
        if(bm!= architect.buildMethod){
            architect.buildMethod=bm;
            architect.Stop();
        }

        if(Input.GetKeyDown(KeyCode.S)){
            architect.Stop();
        }

        if(Input.GetKeyDown(KeyCode.Space)){

            if(architect.isBuilding){
                if(!architect.hurryUp){
                    architect.hurryUp = true;
                }
                else
                    architect.ForceComplete();
            }
            else
            architect.Build(lines[Random.Range(0,lines.Length)]);
        }
        else if(Input.GetKeyDown(KeyCode.A)){
            architect.Append(lines[Random.Range(0,lines.Length)]);
        }
    }
}
}