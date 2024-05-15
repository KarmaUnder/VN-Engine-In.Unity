using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace CHARACTERS
{
public class CharacterSpriteLayer
{
    private Character_Manager character_Manager => Character_Manager.instance;

    private const float DEFAULT_TRANSITION_SPEED = 3F;
    private float transitionSpeedMultiplier = 1;
    public int layer {get; private set;} = 0;

    public Image renderer {get; private set;} = null;

    public CanvasGroup rendererCG => renderer.GetComponent<CanvasGroup>(); 

    private List <CanvasGroup> oldRenderers = new List<CanvasGroup>();
    private Coroutine co_LevelingAlpha=null;
    public bool isLevelingAlpha => co_LevelingAlpha != null;

    private Coroutine co_TransitioningLayer = null;
    public bool isTransitioningLayer =>co_TransitioningLayer !=null;

    public CharacterSpriteLayer (Image defaultRenderer, int layer = 0)
    {
        this.layer = layer;
        renderer = defaultRenderer;
    }

    public void SetSprite(Sprite sprite)
    {
        renderer.sprite = sprite;
    }

    public Coroutine TransiotionSprite(Sprite sprite, float speed = 1)
    {
        if(sprite == renderer.sprite)
            return null;
        
        if(isTransitioningLayer)
            character_Manager.StopCoroutine(co_TransitioningLayer);

        co_TransitioningLayer = character_Manager.StartCoroutine(TransitioningSprite(sprite, speed));

        return co_TransitioningLayer;
    }

private IEnumerator TransitioningSprite(Sprite sprite, float speedMultiplier = 1)
{
    transitionSpeedMultiplier = speedMultiplier;
    Image newRenderer = CreateRenderer(renderer.transform.parent);

    newRenderer.sprite = sprite;

    yield return TryStartLevelingAlphas();

    co_TransitioningLayer = null;
}

private Image CreateRenderer(Transform parent)
{
    Image newRenderer = Object.Instantiate(renderer, parent);
    oldRenderers.Add(rendererCG);

    newRenderer.name = renderer.name;
    renderer = newRenderer;
    renderer.gameObject.SetActive(true);
    rendererCG.alpha = 0;


    return newRenderer;
}

private Coroutine TryStartLevelingAlphas()
{
    if(isLevelingAlpha)
        return co_LevelingAlpha;

    co_LevelingAlpha = character_Manager.StartCoroutine(RunAlphaLeveling());

    return co_LevelingAlpha;
}


private IEnumerator RunAlphaLeveling()
{
    while(rendererCG.alpha < 1 || oldRenderers.Any(oldCG => oldCG.alpha > 0))
    {
        float speed = DEFAULT_TRANSITION_SPEED * transitionSpeedMultiplier * Time.deltaTime;
        rendererCG.alpha = Mathf.MoveTowards(rendererCG.alpha, 1, speed);

        for(int i = oldRenderers.Count - 1; i >= 0; i--)
        {
            CanvasGroup oldCG = oldRenderers[i];
            oldCG.alpha = Mathf.MoveTowards(oldCG.alpha, 0, speed);

            if(oldCG.alpha <= 0)
            {
                oldRenderers.RemoveAt(i);
                Object.Destroy(oldCG.gameObject);
            }
        }
    yield return null;
    }
    co_LevelingAlpha=null;
}

}

}
