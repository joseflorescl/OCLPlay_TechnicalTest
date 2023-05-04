using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UIElements;
using System;
using Random = UnityEngine.Random;
using Mono.Cecil;

public class SimpleHorizontalBackgroundManager : MonoBehaviour
{
    // It is preferred to create a centralized component instead of having to add a component on each of the sprites
    //   that will move in the background.
    // The idea is to create a component that is easy to configure in the inspector (single point of configuration)
    //   and relatively reusable for other projects.

    private Vector2 direction = Vector2.left; // Only left moving sprite change validation is implemented.


    [Serializable]
    struct BackgroundConfiguration
    {
        public SpriteRenderer spriteRenderer;
        public float speed;
    }

    
    [SerializeField] private BackgroundConfiguration[] backgroundConfigurations;
    [SerializeField] private int size = 2; // Number of repeating sprites that will move horizontally
    [SerializeField] private Transform[] artifactsPrefabs;
    [SerializeField] private int backgroundIndexForArtifacts;
    [SerializeField] private int backgroundIndexForTreasures;
    

    struct SpritesInBackground
    {
        public SpriteRenderer[] spriteRenderers; // The most common is that each background has 2 sprites moving horizontally
    }

    // A background element corresponds to an array of sprites, arranged horizontally next to each other.
    SpritesInBackground[] backgrounds;
    float xCameraLeft;
    float speedFactor; // depends on the level of difficulty
    bool paused;

    private void Awake()
    {

        backgrounds = new SpritesInBackground[backgroundConfigurations.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].spriteRenderers = new SpriteRenderer[size];
        }
        paused = true;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += GameStartHandler;
        GameManager.Instance.OnTreasureCreated += TreasureCreatedHandler;
    }


    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= GameStartHandler;
        GameManager.Instance.OnTreasureCreated -= TreasureCreatedHandler;
    }

    private void TreasureCreatedHandler(TreasureController treasure)
    {
        // Note: It does not work to leave the treasure as a child of a sprite, because the sprite can be reset in its position
        //  the treasure will be able to disappear BEFORE reaching the left end of the screen

        // We set the speed and direction. The position on the Y axis is configured in the prefab
        treasure.Direction = direction;
        treasure.Speed = speedFactor * backgroundConfigurations[backgroundIndexForTreasures].speed;        
    }

    private void GameStartHandler(int currentLevel)
    {
        paused = false;        
        speedFactor = GameManager.Instance.GetSpeedFactorByLevel(currentLevel); 
    }


    private void Start()
    {
        xCameraLeft = Camera.main.ViewportToWorldPoint(Vector3.zero).x;

        InstantiateSpritesForBackground();
        StartPositionForSpritesInBackground();
        PutArtifactsInBackgroundLevel(backgroundIndexForArtifacts);

        GameManager.Instance.BackgroundCreated();
    }

    void PutArtifactsInBackgroundLevel(int backgroundIndexForArtifacts)
    {
        var sprites = backgrounds[backgroundIndexForArtifacts].spriteRenderers;

        // For each background sprite of the backgroundIndexForArtifacts level
        for (int i = 0; i < sprites.Length; i++)
        {
            int count = RandomArtifactsCountPerSprite();

            float deltaX = sprites[i].bounds.size.x / count;
            float xMin = sprites[i].bounds.min.x;

            // The sprite is segmented into "count" pieces, inside each one an artifact is placed
            for (int n = 0; n < count; n++)
            {
                // The exact position within the segment is random
                // Boundary condition: "xOffsetArtifact": to ensure that the entire sprite of the artifact falls within the segment
                float xOffsetArtifact = 3;
                float xPos = Random.Range(xMin + n * deltaX + xOffsetArtifact, xMin + (n + 1) * deltaX - xOffsetArtifact);

                print("Delta random = " + ((xMin + n * deltaX + xOffsetArtifact) - (xMin + (n + 1) * deltaX - xOffsetArtifact)));

                // The selected artifact is also random
                int idx = Random.Range(0, artifactsPrefabs.Length);

                Transform parent = sprites[i].transform;
                Vector3 position = parent.position;
                position.x = xPos;
                // With this, the artifact will move along with the sprite.
                Transform artifact = Instantiate(artifactsPrefabs[idx], position, parent.rotation, parent);

                // Horizontal orientation is also random
                if (Random.value > 0.5f)
                {
                    artifact.localScale = new Vector3(-artifact.localScale.x, artifact.localScale.y, artifact.localScale.z);
                }
            }
        }
    }

    int RandomArtifactsCountPerSprite()
    {
        int minArtifactsPerSprite = 2;
        int maxArtifactsPerSprite = 3;
        return Random.Range(minArtifactsPerSprite, maxArtifactsPerSprite + 1);
    }

    private void Update()
    {
        if (paused) return;
        
        TranslateBackgrounds();
        BackgroundsPositionValidation();
    }

    

    void TranslateBackgrounds()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            for (int j = 0; j < backgrounds[i].spriteRenderers.Length; j++)
            {
                float speed = speedFactor * backgroundConfigurations[i].speed;
                backgrounds[i].spriteRenderers[j].transform.Translate(speed * Time.deltaTime * direction);
            }
        }
    }

    void BackgroundsPositionValidation()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            var firstSprite = backgrounds[i].spriteRenderers[0];
            float xRight = firstSprite.bounds.max.x;
            if (xRight < xCameraLeft)
            {
                SwapSprites(backgrounds[i].spriteRenderers);
            }
        }
    }

    void SwapSprites(SpriteRenderer[] spriteRenderers)
    {
        // The sprite array is rearranged (it's like a stack)
        var first = spriteRenderers[0];
        var length = spriteRenderers.Length;

        for (int i = 1; i < length; i++)
        {
            spriteRenderers[i - 1] = spriteRenderers[i];
        }

        spriteRenderers[length - 1] = first;

        float xLeft = spriteRenderers[length - 2].bounds.max.x;
        RepositionSpriteAtXPos(first, xLeft);
    }

    void InstantiateSpritesForBackground()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // The first sprite is the one configured in BackgroundConfiguration
            backgrounds[i].spriteRenderers[0] = backgroundConfigurations[i].spriteRenderer;

            // The rest of the sprites will be a clone of the first
            for (int j = 1; j < size; j++)
            {
                backgrounds[i].spriteRenderers[j] = Instantiate(backgrounds[i].spriteRenderers[0], transform);
            }

        }
    }

    void StartPositionForSpritesInBackground()
    {
        // For each background, its sprites are positioned horizontally, one next to the other
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float xLeft = xCameraLeft;
            for (int j = 0; j < backgrounds[i].spriteRenderers.Length; j++)
            {
                xLeft = RepositionSpriteAtXPos(backgrounds[i].spriteRenderers[j], xLeft);
            }
        }
    }

    float RepositionSpriteAtXPos(SpriteRenderer spriteRenderer, float xLeft)
    {
        // The function returns the value of xRight of the repositioned sprite.
        //  It is only repositioned on the X axis, the Y axis is not touched
        float extentX = spriteRenderer.bounds.extents.x;
        var newPos = spriteRenderer.transform.position;
        newPos.x = xLeft + extentX;
        spriteRenderer.transform.position = newPos;

        return xLeft + spriteRenderer.bounds.size.x;
    }

}
