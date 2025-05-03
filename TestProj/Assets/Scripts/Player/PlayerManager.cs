using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    void Awake()
    {
        instance = this;
        input = GetComponent<PlayerInputControls>();
        health = player.GetComponent<PlayerHealthStats>();
    }

    public GameObject player;
    public PlayerInputControls input { get; private set; }
    public PlayerHealthStats health { get; private set; }
}
