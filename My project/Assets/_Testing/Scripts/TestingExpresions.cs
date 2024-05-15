using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CHARACTERS;

namespace TESTING
{
public class TestExpresions : MonoBehaviour
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

            Raelin.Show();

            yield return new WaitForSeconds(1);

            Raelin.TransitionSprite(Raelin.GetSprite("Raelin_7"),1);
            Raelin.TransitionSprite(Raelin.GetSprite("Raelin_2"));

           yield return null;
    

        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
