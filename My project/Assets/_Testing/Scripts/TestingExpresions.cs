using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CHARACTERS;
using JetBrains.Annotations;

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
            Character_Sprite Mage = Character_Manager.instance.CreateCharacter("Generic") as Character_Sprite;


            Raelin.SetPosition(Vector2.zero);
            Mage.SetPosition(new Vector2(1,0));

            Mage.UnHighlight();
            Mage.Show();
            Raelin.Show();

            yield return Raelin.Say("Just a conversation example");

            Raelin.UnHighlight();
            Mage.Highlight();
            yield return Mage.Say("A very basic example! {c}Better test another thing...");

            Mage.UnHighlight();
            Raelin.Highlight();

            yield return Raelin.Say("Ok, {a}Im gonna try");


            Raelin.TransitionSprite(Raelin.GetSprite("Raelin_7"),1);
            Raelin.TransitionSprite(Raelin.GetSprite("Raelin_2"));

            yield return Raelin.Say("What do you think about that?");

            yield return new WaitForSeconds(1);

            yield return Raelin.TransitionColor(Color.red, speed: 0.3f);
            yield return Raelin.TransitionColor(Color.blue);
            yield return Raelin.TransitionColor(Color.yellow);
            yield return Raelin.TransitionColor(Color.white);

            yield return Raelin.UnHighlight();

            yield return new WaitForSeconds(1);

            yield return Raelin.TransitionColor(Color.red, speed: 0.3f);

            yield return new WaitForSeconds(1);

            yield return Raelin.Highlight();

            yield return new WaitForSeconds(1);

            yield return Raelin.TransitionColor(Color.white);


           yield return null;
    

        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
