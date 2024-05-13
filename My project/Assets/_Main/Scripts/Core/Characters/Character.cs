using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace CHARACTERS
{
    public abstract class Character
    {
        public string name = "";
        public string displayName= "";
        public RectTransform root = null;
        public CharacterConfigData config;
        public DialogueSystem dialogueSystem => DialogueSystem.instance;
        public Character_Manager manager => Character_Manager.instance;

        protected Coroutine co_revealing, co_hiding;
        public bool isRevealing => co_revealing!=null;
        public bool isHiding => co_hiding !=null;
        public virtual bool isVisible => false;

        public Animator animator;


        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayName = name;
            this.config = config;

            if(prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, manager.characterPanel);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();
            }
        }

        public Coroutine Say(string dialogue) => Say(new List<string> {dialogue});
    
        public Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            UpdateTextCustomizationOnScreen();
            return dialogueSystem.Say(dialogue);
        }

        public virtual Coroutine Show()
        {
            if(isRevealing)
            return co_revealing;

            if(isHiding)
                manager.StopCoroutine(co_hiding);
            
            co_revealing = manager.StartCoroutine(ShowingOrHiding(true));

            return co_revealing;
        }

        public virtual Coroutine Hide()
        {
            if(isHiding)
                return co_hiding;

            if(isRevealing)
                manager.StopCoroutine(co_revealing);

            co_hiding = manager.StartCoroutine(ShowingOrHiding(false));

            return co_hiding;
        }

        public virtual IEnumerator ShowingOrHiding(bool show)
        {
            Debug.Log("Show/Hide cannot be called from a base type");
            yield return null;
        }

        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void ResetConfigurationData() => config = Character_Manager.instance.GetCharacterConfig(name);

        public void UpdateTextCustomizationOnScreen() => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);

        public enum CharacterType
        {
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3d
        }
    }
}