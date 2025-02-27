using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public static class CustomProperty
{
    public const string LOAD = "Load";
    public const string READY = "Ready";
    public const string WINNER = "Winner";
    public const string LIFE = "Life";
    public const string COLOR = "Color";

    private static PhotonHashtable customProperty = new PhotonHashtable();

    public static void SetReady(this Player player, bool ready)
    {
        customProperty.Clear();
        customProperty.Add(READY, ready);
        player.SetCustomProperties(customProperty);
    }

    public static bool GetReady(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(READY))
        {
            return (bool)customProperty[READY];
        }
        else
            return false;
    }

    public static void SetLoad(this Player player, bool load)
    {
        customProperty.Clear();
        customProperty[LOAD] = load;
        player.SetCustomProperties(customProperty);
    }

    public static bool GetLoad(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(LOAD))
        {
            return (bool)customProperty[LOAD];
        }
        else
            return false;
    }

    public static void SetWinner(this Player player, bool number)
    {
        customProperty.Clear();

        customProperty[WINNER] = number;
        player.SetCustomProperties(customProperty);
    }

    public static bool GetWinner(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(WINNER))
        {
            return (bool)customProperty[WINNER];
        }
        else
            return false;
    }

    public static void SetLife(this Player player, bool result)
    {
        customProperty.Clear();
        customProperty[LIFE] = result;
        player.SetCustomProperties(customProperty);
    }

    public static bool GetLife(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(LIFE))
        {
            return (bool)customProperty[LIFE];
        }
        else
            return false;
    }

    public static void SetColor(this Player player, Vector3 color)
    {
        customProperty.Clear();
        customProperty[COLOR] = color;
        player.SetCustomProperties(customProperty);
    }

    public static Vector3 GetColor(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(COLOR))
        {
            return (Vector3)customProperty[COLOR];
        }
        else
            return Vector3.one;
    }

    public static void ResetProperty(this Player player)
    {
        customProperty.Clear();
        Debug.Log(customProperty);
        customProperty[READY] = null;
        customProperty[LOAD]= null;
        customProperty[LIFE]= null;
        customProperty[WINNER]= null;

        player.SetCustomProperties(customProperty);
    }
}