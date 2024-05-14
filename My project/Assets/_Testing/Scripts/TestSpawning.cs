using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CHARACTERS;

namespace TESTING
{
public class TestSpawning : MonoBehaviour
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
          

            Character Mage1 = Character_Manager.instance.CreateCharacter("Mage1 as Generic");
            //Character Mage2 = Character_Manager.instance.CreateCharacter("Mage2 as Generic");
           // Character Mage3 = Character_Manager.instance.CreateCharacter("Mage3 as Generic");

            Mage1.SetPosition(Vector2.zero);
           // Mage2.SetPosition(new Vector2(0.5f,0.5f));
            //Mage3.SetPosition(Vector2.one);

            //Mage1.Show();
           // Mage2.Show();
            //Mage3.Show();
           
            yield return Mage1.MoveToPosition(Vector2.one);
            yield return Mage1.MoveToPosition(Vector2.zero);
            yield return null;

        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
