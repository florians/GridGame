using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private GameObject emptyTile;
    [SerializeField]
    private GameObject levelContainer;
    [SerializeField]
    private GameObject[] groundTiles;
    [SerializeField]
    private GameObject[] blockTiles;

    private int rows = 5;
    private int cols = 5;
    private float tileSize = (float)1;

    //public UserInterface userInterface;
    public Player player;

    public int level = 1;
    private int emptyAmount = 1;
    private int trapAmount = 2;
    private int upgradeAmount = 1;
    private int tunnelAmount = 1;


    private List<int> allRandomNumbers = new List<int>();
    private int[] emtpyFields;
    private int[] trapFields;
    private int[] hpField;
    private int[] upgradeField;
    private int[] goDownField;

    private Sprite defaultGroundSprite;

    void Start()
    {
        GenerateGrid();
        player.Init();
    }

    public void GenerateGrid()
    {
        allRandomNumbers.Clear();
        UpdateVariables();
        SetRandomNumbers();
        ClearOld();
        transform.position = new Vector2(0, 0);
        int gridCounter = 0;
        GameObject container = (GameObject)Instantiate(levelContainer, transform);
        int groundRandomNr = UnityEngine.Random.Range(0, groundTiles.Length);
        defaultGroundSprite = groundTiles[groundRandomNr].GetComponent<SpriteRenderer>().sprite;
        container.name = "Level_" + level;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // positions
                float posX = col * tileSize;
                float posY = row * -tileSize;

                container.transform.SetParent(transform);
                CreateTile(gridCounter, container, posX, posY);
                gridCounter++;
            }
        }
        float gridW = cols * tileSize;
        float gridH = rows * tileSize;

        transform.position = new Vector2(-gridW / 2 + tileSize / 2, gridH / 2 - tileSize / 2);
    }

    private void CreateTile(int gridCounter, GameObject container, float x, float y, int order = 1)
    {
        int blockRandomNr = UnityEngine.Random.Range(0, blockTiles.Length);
        GameObject tile = (GameObject)Instantiate(emptyTile, transform);
        tile.GetComponent<SpriteRenderer>().sprite = defaultGroundSprite;
        tile.name = "default";
        tile.AddComponent<ClickEvent>();
        ClickEvent tileEvent = tile.GetComponent<ClickEvent>();
        // fallback sprite
        GameObject fallback = tile.transform.GetChild(0).gameObject;
        fallback.GetComponent<SpriteRenderer>().sprite = defaultGroundSprite;
        // event sprite
        GameObject eventSprite = tile.transform.GetChild(2).gameObject;

        if (order != 0)
        {
            if (Array.IndexOf(trapFields, gridCounter) >= 0)
            {
                tile.name = "trap";
                tileEvent.e = Events.Trap;
                eventSprite.AddComponent<Animator>();
                eventSprite.AddComponent<SpriteMask>();
                eventSprite.GetComponent<Animator>().runtimeAnimatorController = GetResource("events/" + tile.name).GetComponent<Animator>().runtimeAnimatorController;
                eventSprite.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
                eventSprite.GetComponent<SpriteMask>().sprite = GetResource("events/" + tile.name).GetComponent<SpriteMask>().sprite;
                eventSprite.GetComponent<SpriteMask>().alphaCutoff = 0.3f;
                eventSprite.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
            // upgrade field
            else if (Array.IndexOf(upgradeField, gridCounter) >= 0)
            {
                tile.name = "upgrade";
                tileEvent.e = Events.Upgrade;
            }
            // hp field
            else if (Array.IndexOf(hpField, gridCounter) >= 0)
            {
                tile.name = "hp";
                tileEvent.e = Events.Hp;
            }
            // down field
            else if (Array.IndexOf(goDownField, gridCounter) >= 0)
            {
                tile.name = "goDown";
                tileEvent.e = Events.GoDown;
            }
            if (tile.name != "default")
            {
                eventSprite.GetComponent<SpriteRenderer>().sprite = GetResource("events/" + tile.name).GetComponent<SpriteRenderer>().sprite;
            }
            int pos = Array.IndexOf(emtpyFields, gridCounter);
            if (pos == -1)
            {
                tile.GetComponent<SpriteRenderer>().sprite = blockTiles[blockRandomNr].GetComponent<SpriteRenderer>().sprite;
                tile.name += "_" + blockTiles[blockRandomNr].name;
                if (
                    tileEvent.e != Events.Trap &&
                    tileEvent.e != Events.Upgrade &&
                    tileEvent.e != Events.Hp &&
                    tileEvent.e != Events.GoDown
                    )
                {
                    tileEvent.e = Events.Destroy;
                }
            }
        }
        else
        {
            Destroy(tile.GetComponent<BoxCollider2D>());
        }
        tile.transform.position = new Vector2(x, y);
        tile.transform.SetParent(container.transform);
        tile.GetComponent<SpriteRenderer>().sortingOrder = order;
    }

    public void Replay()
    {
        ClearOld();
        player.Init();
        GenerateGrid();
    }

    private void SetRandomNumbers()
    {
        emtpyFields = NrGenerator(0, (rows * cols), emptyAmount);
        trapFields = NrGenerator(0, (rows * cols), trapAmount);
        upgradeField = NrGenerator(0, (rows * cols), upgradeAmount);
        hpField = NrGenerator(0, (rows * cols), upgradeAmount);
        goDownField = NrGenerator(0, (rows * cols), tunnelAmount);
    }
    private void UpdateVariables()
    {
        level = player.currentLevel;
        emptyAmount = player.currentUpgrade;
    }
    private void ClearOld()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    private GameObject GetResource(string name)
    {
        return (GameObject)Resources.Load(name);
    }

    public void OnClickEvent(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().enabled = false;
        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        obj.transform.GetChild(1).GetComponent<Animator>().enabled = true;
        obj.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
    }

    private int[] NrGenerator(int start, int end, int amount)
    {
        if (amount > (int)(end / 2))
        {
            amount = (int)(end / 2);
        }
        int[] numbers = new int[amount];
        int i = 0;
        int stopCounter = 0;
        while (i < numbers.Length)
        {
            int rndNr = UnityEngine.Random.Range(start, end - 1);
            if (!allRandomNumbers.Contains(rndNr))
            {
                numbers[i] = rndNr;
                allRandomNumbers.Add(rndNr);
                i++;
            }
            stopCounter++;
            if (stopCounter >= 400)
            {
                Debug.Log("overflow error");
                return numbers;
            }
        }
        return numbers;
    }
}
