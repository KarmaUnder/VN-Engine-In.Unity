using System;
using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace CHARACTERS
{
    public abstract class Character
    {
        private const float UNHIGHLIGHTED_DARKEN_STRENGHT = 0.65f;
        public const bool DEFAULT_ORIENTATION_IS_FACING_LEFT = true;
        public const string ANIMATION_REFRESH_TRIGGER = "Refresh";

        protected bool facingLeft = DEFAULT_ORIENTATION_IS_FACING_LEFT;
        public string name = "";
        public string displayName= "";
        public RectTransform root = null;
        public CharacterConfigData config;
        public DialogueSystem dialogueSystem => DialogueSystem.instance;
        public Character_Manager manager => Character_Manager.instance;
        public Color color {get; protected set;} = Color.white;
        protected Color displayColor => highlighted ? highlightedColor : unhighlightedColor;
        protected Color highlightedColor => color;
        protected Color unhighlightedColor => new Color(color.r * UNHIGHLIGHTED_DARKEN_STRENGHT, color.g * UNHIGHLIGHTED_DARKEN_STRENGHT, color.b * UNHIGHLIGHTED_DARKEN_STRENGHT, color.a);
        public bool highlighted {get; protected set;} = true;
        protected Coroutine co_revealing, co_hiding;
        protected Coroutine co_moving;
        protected Coroutine co_changingColor;
        protected Coroutine co_highlighting;
        protected Coroutine co_flipping;

        public bool isHighlighting => (highlighted && co_highlighting !=null);
        public bool isUnHighlighting => (!highlighted && co_highlighting !=null);

        public bool isRevealing => co_revealing!=null;
        public bool isHiding => co_hiding !=null;
        public bool isMoving => co_moving != null;
        public bool isChangingColor => co_changingColor!=null;
        public virtual bool isVisible {get; set;}
        public bool isFacingLeft => facingLeft;
        public bool isFacingRight => !facingLeft;
        public bool isFlippling => co_flipping != null;

        public int priority {get; protected set;}

        public Animator animator;


        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayName = name;
            this.config = config;

            if(prefab != null)
            {
                GameObject ob = UnityEngine.Object.Instantiate(prefab, manager.characterPanel);
                ob.name = manager.FormatCharacterPath(manager.characterPrefabNameFormat, name);
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

        public virtual void SetPosition(Vector2 position)
        {
            if(root == null)
                return;

            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);

            root.anchorMin = minAnchorTarget;
            root.anchorMax = maxAnchorTarget;
        }

        protected (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterAnchorTargets(Vector2 position)
        {
            Vector2 padding = root.anchorMax - root.anchorMin;

            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;

            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y);
            Vector2 maxAnchorTarget = minAnchorTarget + padding;

            return (minAnchorTarget, maxAnchorTarget);
        }

        public virtual void SetColor(Color color)
        {
            this.color = color;
        }

        public Coroutine TransitionColor(Color color, float speed = 1)
        {
            this.color = color;

            if(isChangingColor)
                manager.StopCoroutine(co_changingColor);

            co_changingColor= manager.StartCoroutine(ChangingColor(displayColor, speed));

            return co_changingColor;
        }

        public Coroutine Highlight(float speed = 1f)
        {
            if(isHighlighting)
                return co_highlighting;
            if(isUnHighlighting)
                manager.StopCoroutine(co_highlighting);

            highlighted = true;
            co_highlighting = manager.StartCoroutine(Highlighting(highlighted, speed));

            return co_highlighting;
        }
        public Coroutine UnHighlight(float speed = 1f)
        {
            if(isUnHighlighting)
                return co_highlighting;
            if(isHighlighting)
                manager.StopCoroutine(co_highlighting);

            highlighted = false;
            co_highlighting = manager.StartCoroutine(Highlighting(highlighted,speed));

            return co_highlighting;
        }

        public virtual IEnumerator Highlighting(bool highlight, float speedMultiplier)
        {
            Debug.Log("Highlighting is not avaliable for text characters");
            yield return null;
        }

        public Coroutine Flip(float speed=1, bool immediate = false)
        {
            if(isFacingLeft)
                return FaceRight(speed, immediate);
            else
                return FaceLeft(speed, immediate);
        }
        public Coroutine FaceLeft(float speed=1, bool immediate = false)
        {
            if(isFlippling)
                manager.StopCoroutine(co_flipping);
            facingLeft = true;
            co_flipping = manager.StartCoroutine(FaceDirection(facingLeft, speed, immediate));

            return co_flipping;
        }
        public Coroutine FaceRight(float speed=1, bool immediate = false)
        {
            if(isFlippling)
                manager.StopCoroutine(co_flipping);
            facingLeft = false;
            co_flipping = manager.StartCoroutine(FaceDirection(facingLeft, speed, immediate));

            return co_flipping;
        }

        public virtual IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            Debug.Log("Flip is not posible on this character type");
            yield return null;
        }
        public virtual IEnumerator ChangingColor(Color color, float speed)
        {
            Debug.Log("Color changing is not posible on this character type");
            yield return null;
        }
        public virtual Coroutine MoveToPosition(Vector2 position, float speed = 2f, bool smooth = false)
        {
            if(root == null)
                return null;

            if(isMoving)
                manager.StopCoroutine(co_moving);
            co_moving = manager.StartCoroutine(MovingToPosition(position, speed, smooth));

            return co_moving;

        }

        private IEnumerator MovingToPosition (Vector2 position, float speed, bool smooth)
        {
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);
            Vector2 padding = root.anchorMax - root.anchorMin;

            while(root.anchorMin != minAnchorTarget || root.anchorMax != maxAnchorTarget)
            {
                root.anchorMin = smooth ?
                    Vector2.Lerp(root.anchorMin, minAnchorTarget, speed * Time.deltaTime)
                    : Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed * Time.deltaTime * 0.35f);

                root.anchorMax = root.anchorMin + padding;

                if(smooth && Vector2.Distance(root.anchorMin, minAnchorTarget) <= 0.001f)
                {
                    root.anchorMin = minAnchorTarget;
                    root.anchorMax = maxAnchorTarget;
                    break;
                }
                yield return null;
            }

            Debug.Log("Done Moving");
            co_moving = null;
        }

        public void SetPriority(int priority, bool autoSortCharactersOnUI = true)
        {
            this.priority = priority;
            if(autoSortCharactersOnUI)
                manager.SortCharacters();
        }

        public void Animate(string animation)
        {
            animator.SetTrigger(animation);
        }
        public void Animate(string animation, bool state)
        {
            animator.SetBool(animation, state);


            
            animator.SetTrigger(ANIMATION_REFRESH_TRIGGER);
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