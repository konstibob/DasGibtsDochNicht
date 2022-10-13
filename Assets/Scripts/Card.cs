using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string name;
    public string description;
    public Sprite artwork;
    public Sprite cardBack;
}
    