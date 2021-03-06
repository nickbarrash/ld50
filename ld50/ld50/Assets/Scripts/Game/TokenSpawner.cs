using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TokenSpawnInfo {
    public int value;
    public int spawnTick;
    //public Vector2 impulseVector;
}

public class TokenSpawner : MonoBehaviour
{
    public GameObject tokenPrefab;
    public SpriteRenderer Affordance;

    private int spawnIndex = 0;

    public int loopAfterTicks;
    public int loopOffset;
    public List<TokenSpawnInfo> tokenSpawns;

    public float SpawnProgress => (float) ((Simulation.Instance.Ticks + loopOffset) % loopAfterTicks) / (float)loopAfterTicks;

    private void Start() {
        Destroy(Affordance);

        if (loopAfterTicks <= 0)
            Debug.LogWarning("Token spawner has loopAfter ticks of: " + loopAfterTicks);

        if (tokenSpawns == null || tokenSpawns.Count == 0)
            Debug.LogWarning("Token spawner empty token spawn list");

        if (loopOffset == 0 && tokenSpawns[0].spawnTick == 0)
            SpawnToken();
    }

    private void FixedUpdate() {
        if (!Simulation.Instance.Simulating || Simulation.Instance.Ticks == 0)
            return;

        var loopedTick = (Simulation.Instance.Ticks + loopOffset) % loopAfterTicks;
        if (loopedTick == 0)
            spawnIndex = 0;

        if (spawnIndex >= tokenSpawns.Count)
            return;

        if (loopedTick == tokenSpawns[spawnIndex].spawnTick) {
            SpawnToken();
        }
    }

    private void SpawnToken() {
        var tokenInfo = tokenSpawns[spawnIndex++];

        GameObject newTokenGO = Instantiate(tokenPrefab, Tokens.Instance.transform);
        newTokenGO.transform.position = transform.position;
        var newToken = newTokenGO.GetComponent<Token>();
        newToken.SetInfo(tokenInfo);
        newToken.spawner = this;
    }
}
