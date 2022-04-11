using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The GameManager keeps track of global information for the game, such as the players and the location of the gaol.
// The GameManager is a Singleton (see Nystrom, chapter II.6), there is only single instance of it.

public class GameManager : MonoBehaviour
{
    // This is the single instance of the class
    private static GameManager instance = null;

    // Keep track of all the players
    private const int numGreenPlayers = 6;
    private List<GreenPlayer> greenPlayers = new List<GreenPlayer>(numGreenPlayers);

    private const int numPurplePlayers = 3;
    private List<PurplePlayer> purplePlayers = new List<PurplePlayer>(numPurplePlayers);

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
            gp.OpponnentCaptured += gp.TransportToGaol;
            greenPlayers.Add(gp);
        }

        for (int i = 0; i < numPurplePlayers; i++)
        {
            purplePlayers.Add(Instantiate(purplePlayerPrefab).GetComponent<PurplePlayer>());

        }

    }

    // Update is called once per frame
    void Update()
    {

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

        if (greenPlayers.Count == 0)
        {
            print("Purple team has won");
            purpleTeamHasWon = true;
        }

        foreach (GreenPlayer greenPlayer in greenPlayers)
        {
            float distance = Vector2.Distance(greenPlayer.Position(), player.Position());
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = greenPlayer;
            }
        }

        return target;
    }

    public bool CheckPurpleTeamWinningCondition()
    {
        return purpleTeamHasWon;
    }

    // Find the nearest purple player to a given green player

    public PurplePlayer FindClosestTarget(GreenPlayer player)
    {
        PurplePlayer target = null;
        float closestDistance = float.MaxValue;

        foreach (PurplePlayer purplePlayer in purplePlayers)
        {
            float distance = Vector2.Distance(purplePlayer.Position(), player.Position());
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
}
