using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The GameManager keeps track of global information for the game, such as the players and the location of the gaol.
// The GameManager is a Singleton (see Nystrom, chapter II.6), there is only single instance of it.

public class GameManager : MonoBehaviour
{
    public float planeRadius = 20f;
    public Transform planeCenter;
    public Transform UiPanel;
    // We store all obstacles
    public List<GameObject> obstacles;
    // A string to identify objects tagged as obstacles
    const string ObstacleTag = "Obstacle";

    // This is the single instance of the class
    private static GameManager instance = null;

    // Keep track of all the players
    private const int numGreenPlayers = 5;
    private List<Player> greenPlayers = new List<Player>(numGreenPlayers);

    private const int numPurplePlayers = 2;
    private List<Player> purplePlayers = new List<Player>(numPurplePlayers);

    private bool purpleTeamHasWon = false;

    [SerializeField]
    GameObject greenPlayerPrefab;
    [SerializeField]
    GameObject purplePlayerPrefab;
    [SerializeField]
    GameObject gaol;

    // Start is called before the first frame update
    void Start()
    {
        // If there already is an official instance, this instance deletes itself.
        if (instance == null)
        { instance = this; }
        else
        {
            Destroy(this);
            return;
        }

        // Create all the players.
        for (int i = 0; i < numGreenPlayers; i++)
        {
            GreenPlayer gp = Instantiate(greenPlayerPrefab).GetComponent<GreenPlayer>();
            greenPlayers.Add(gp);
        }

        for (int i = 0; i < numPurplePlayers; i++)
        {
            purplePlayers.Add(Instantiate(purplePlayerPrefab).GetComponent<PurplePlayer>());
        }

        // Find the obstacles
        obstacles = new List<GameObject>(GameObject.FindGameObjectsWithTag(ObstacleTag));

    }

    // Update is called once per frame
    void Update()
    {
        if (CheckPurpleTeamWinningCondition()) UiPanel.gameObject.SetActive(true);
    }

    public void RemoveGreenPlayerFromList(GreenPlayer player)
    {
        greenPlayers.Remove(player);
    }

    // Find the nearest green player to a given purple player
    public GreenPlayer FindClosestTarget(PurplePlayer player)
    {
        GreenPlayer target = null;
        float closestDistance = float.MaxValue;
        foreach (GreenPlayer greenPlayer in greenPlayers)
        {
            float distance = Vector2.Distance(greenPlayer.position, player.position);
            if (distance < closestDistance && greenPlayer.isIdle)
            {
                closestDistance = distance;
                target = greenPlayer;
            }
        }
        return target;
    }

    public bool CheckPurpleTeamWinningCondition()
    {
        return greenPlayers.Count == 0;
    }

    // Find the nearest purple player to a given green player

    public PurplePlayer FindClosestTarget(GreenPlayer player)
    {
        PurplePlayer target = null;
        float closestDistance = float.MaxValue;

        foreach (PurplePlayer purplePlayer in purplePlayers)
        {
            float distance = Vector2.Distance(purplePlayer.position, player.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = purplePlayer;
            }
        }
        return target;
    }

    // Return the gaol object
    public GameObject GetGaol()
    {
        return gaol;
    }

    // Return the single instance of the class
    public static GameManager Instance()
    {
        return instance;
    }
    public List<Player> GreenPlayers()
    {
        return greenPlayers;
    }

    public List<Player> PurplePlayers()
    {
        return purplePlayers;
    }
}
