using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TruchetTilesPreset",menuName = "TruchetTiles/Preset")]
public class TruchetTilesPreset : ScriptableObject
{
    [SerializeField]
    private List<Sprite> truchetTilesSprites = new List<Sprite>();

    public IReadOnlyList<Sprite> TruchetTilesSprites => truchetTilesSprites;

    public int AmountOfSprites => truchetTilesSprites.Count;
}
