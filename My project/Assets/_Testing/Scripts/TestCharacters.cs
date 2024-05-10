using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
using TMPro;


namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        public TMP_FontAsset tempFont;
        // Start is called before the first frame update
        void Start()
        {
            //Character stella = Character_Manager.instance.CreateCharacter("Stella");
            //Character bob = Character_Manager.instance.CreateCharacter("Bob");

            StartCoroutine(Test());

        }

        IEnumerator Test()
        {
            Character ellen = Character_Manager.instance.CreateCharacter("Ellen");
            Character ben = Character_Manager.instance.CreateCharacter("Benjamin");
            List<string> lines = new List<string>()
            {
                "Hi, how are you?",
                "My name is Ellen.",
                "What's your name?",
                "Oh, that's a beauty name.",

            };
            yield return ellen.Say(lines);

            ellen.SetNameColor(Color.red);
            ellen.SetDialogueColor(Color.red);
            ellen.SetNameFont(tempFont);
            ellen.SetDialogueFont(tempFont);

            yield return ellen.Say("HA HA HA HA HA HA");

            ellen.ResetConfigurationData();

            yield return ellen.Say("Its a joke");


            yield return ben.Say("Thats just a simply string line test");

            Debug.Log("Finished");
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}