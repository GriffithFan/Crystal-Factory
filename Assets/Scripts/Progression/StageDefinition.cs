using System;
using UnityEngine;

[Serializable]
public class StageDefinition
{
    public string id;
    public string displayName;
    [TextArea] public string lore;
    public double unlockAtTotalCoins;
    public string visualMood;
    public string primaryColor;
    public string accentColor;
    public string backgroundResource;
}

[Serializable]
public class StageCatalog
{
    public StageDefinition[] stages;
}
