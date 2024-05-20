using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CHARACTERS;
using JetBrains.Annotations;

namespace TESTING
{
public class TestingPriority : MonoBehaviour
    {
        public TMP_FontAsset tempFont;
        // Start is called before the first frame update
        void Start()
        {
            //Character FemaleStudent2 = Character_Manager.instance.CreateCharacter("Female Student 2");
            //Character Raelin = Character_Manager.instance.CreateCharacter("Raelin");

            StartCoroutine(Test());

        }

        IEnumerator Test()
        {
          
            Character_Sprite Raelin = Character_Manager.instance.CreateCharacter("Raelin") as Character_Sprite;
            Character_Sprite Mage = Character_Manager.instance.CreateCharacter("Generic") as Character_Sprite;

            Raelin.Show();
            Mage.Show();
    

            yield return new WaitForSeconds(1);
      
            Raelin.SetPriority(1);

           yield return null;
    

        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
