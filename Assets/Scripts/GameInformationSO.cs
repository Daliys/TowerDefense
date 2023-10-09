using Items;
using UnityEngine;

/// <summary>
/// Scriptable object for game information.
/// </summary>
[CreateAssetMenu(fileName = "GameInformationSO", menuName = "Tower Defense/GameInformationSO")]
public class GameInformationSO : ScriptableObject
{
    public LevelItem currentLevel;
}