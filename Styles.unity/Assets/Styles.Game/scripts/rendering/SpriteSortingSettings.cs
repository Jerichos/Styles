using UnityEngine;
using UnityEngine.Serialization;

namespace Styles.Game.rendering
{
[CreateAssetMenu(fileName = "SpriteSorting", menuName = "data/SpriteSorting", order = 0)]
public class SpriteSortingSettings : ScriptableObject
{
    [SerializeField] public int Step = 10;
    [SerializeField] public int StepsPerUnit = 5;
}
}