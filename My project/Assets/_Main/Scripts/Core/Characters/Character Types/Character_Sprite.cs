using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



namespace CHARACTERS
{
public class Character_Sprite : Character
{
    private const string SPRITE_RENDERER_PARENT_NAME = "Renderers";
    private const string SPRITESHEET_DEFAULT_SHEETNAME = "Default";
    private const char SPRITESHEET_TEXT_SPRITE_DELIMITER = '-';
    private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();
    public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();

        public override bool isVisible {get {return isRevealing || rootCG.alpha == 1;} set{rootCG.alpha = value ? 1 : 0; }}

        private string artAssetDirectory = "";
public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetFolder) : base(name, config, prefab)
{
    rootCG.alpha = 0;
    artAssetDirectory = rootAssetFolder+ "/Images";
    GetLayers();
    Debug.Log($"Created Sprite Character: '{name}'");
}

private void GetLayers()
{
    Transform rendererRoot = animator.transform.Find(SPRITE_RENDERER_PARENT_NAME);

    if(rendererRoot == null)
        return;

    for(int i=0; i<rendererRoot.transform.childCount; i++)
    {
        Transform child = rendererRoot.transform.GetChild(i);

        Image rendererImage = child.GetComponentInChildren<Image>();

        if(rendererImage != null)
        {
            CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage,i);
            layers.Add(layer);
            child.name = $"Layer: {i}";
        }
    }
}

public void SetSprite(Sprite sprite, int layer = 0)
{
    layers[layer].SetSprite(sprite);
}

public Sprite GetSprite(string spriteName)
{
    if(config.characterType == CharacterType.SpriteSheet)
    {
        string [] data = spriteName.Split(SPRITESHEET_TEXT_SPRITE_DELIMITER);
        Sprite[] spriteArray = new Sprite[0];

        if(data.Length == 2)
        {
            string textureName = data[0];
            spriteName = data[1];
            spriteArray = Resources.LoadAll<Sprite>($"{artAssetDirectory}/{textureName}");
        }
        else
        {
            spriteArray = Resources.LoadAll<Sprite>($"{artAssetDirectory}/{SPRITESHEET_DEFAULT_SHEETNAME}");
        }
            if(spriteArray.Length == 0)
                Debug.LogWarning($"Character '{name}' does not have a default art asset called '{SPRITESHEET_DEFAULT_SHEETNAME}'");

            return Array.Find(spriteArray, sprite => sprite.name == spriteName);
    }
    else
    {
        return Resources.Load<Sprite>($"{artAssetDirectory}/{spriteName}");
    }
}

public Coroutine TransitionSprite(Sprite sprite, int layer = 0, float speed = 1)
{
    CharacterSpriteLayer spriteLayer = layers[layer];
    return spriteLayer.TransiotionSprite(sprite, speed);
}

        public override void SetColor(Color color)
        {
            base.SetColor(color);
            color = displayColor;

            foreach(CharacterSpriteLayer layer in layers)
            {
                layer.StopChangingColor();
                layer.SetColor(color);
            }
        }

        public override IEnumerator ChangingColor(Color color, float speed)
        {
            foreach(CharacterSpriteLayer layer in layers)
            {
                layer.TransitionColor(color, speed);
            }
            yield return null;

            while(layers.Any(l => l.isChangingColor))
            {
                yield return null;
            }

            co_changingColor = null;
        }

        public override IEnumerator ShowingOrHiding(bool show)
        {
            float targetAlpha = show ? 1f : 0;
            CanvasGroup self = rootCG;

            while(self.alpha != targetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime);
                yield return null;
            }

            co_revealing = null;
            co_hiding = null;
        }

        public override IEnumerator Highlighting(bool highlight, float speedMultiplier)
        {
            Color targetColor = displayColor;
            foreach(CharacterSpriteLayer layer in layers)
            {
                layer.TransitionColor(targetColor, speedMultiplier);
            }
            yield return null;

            while(layers.Any(l => l.isChangingColor))
            {
                yield return null;
            }
            co_highlighting = null;
        }

        public override IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            foreach(CharacterSpriteLayer layer in layers)
            {
                if(faceLeft)
                    layer.FaceLeft(speedMultiplier, immediate);
                else
                    layer.FaceRight(speedMultiplier, immediate);
            }
            yield return null;

            while(layers.Any(l => l.isFlippling))
                yield return null;

            co_flipping = null;
        }


    }
}