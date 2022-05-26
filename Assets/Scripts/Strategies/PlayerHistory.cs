using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerHistory stores the last n positions of a player
public class PlayerHistory
{
    // The dictionary is indexed by a Player, and its values are queues of PlayerData
    private Dictionary<Player, LimitedQueue<PlayerData>> players = new Dictionary<Player, LimitedQueue<PlayerData>>();

    // Add a player to the dictionary
    public void Add(Player player)
    {
        players.Add(player, new LimitedQueue<PlayerData>());
    }

    // Update the queue of position values for a player
    public void Update(Player player)
    {
        players[player].Enqueue(new PlayerData(player.position, player.Rotation(), Time.frameCount));
    }

    // Get a list of the most recent data for all players
    public List<PlayerData> GetLatestPlayerData()
    {
        List<PlayerData> outData = new List<PlayerData>();
        foreach (KeyValuePair<Player, LimitedQueue<PlayerData>> p in players)
        {
            outData.Add(p.Value.First.Value);
        }
        return outData;
    }

    // A test to decide if potentialChaser is approaching target
    public bool IsApproaching(Player target, Player potentialChaser)
    {
        LimitedQueue<PlayerData>.Enumerator targetEnumerator = target.myHistory.players[target].GetEnumerator();
        LimitedQueue<PlayerData>.Enumerator chaserEnumerator = target.myHistory.players[potentialChaser].GetEnumerator();

        List<float> distances = new List<float>();

        if (!targetEnumerator.MoveNext())
            return false;
        if (!chaserEnumerator.MoveNext())
            return false;

        // We want to check values with the same timestamps, so we move through the lists to try to synchronise them
        while (true)
        {
            PlayerData targetData = targetEnumerator.Current;
            PlayerData chaserData = chaserEnumerator.Current;
            if (targetData.timestamp > chaserData.timestamp)
            {
                if (targetEnumerator.MoveNext())
                    continue;
                else break;
            }
            else if (targetData.timestamp < chaserData.timestamp)
            {
                if (chaserEnumerator.MoveNext())
                    continue;
                else break;
            }
            else
            {
                distances.Add((targetData.position - chaserData.position).magnitude);
            }
        }

        if (distances.Count == 3)  // We have three values, can compute acceleration
        {
            return (distances[0] > distances[1] && distances[1] > distances[2]);
            // if (distances[0] - distances[1]).magnitude > (distances[1] - distances[2]).magnitude
            // accelerating
        }
        else if (distances.Count == 2)  // We have two values, can compute approach
        {
            return (distances[0] > distances[1]);
        }
        else if (distances.Count == 1)  // We have one value, can only compute distance
        {
            return false;
        }
        else  // We have no values, can say nothing
        {
            return false;
        }
    }

}
