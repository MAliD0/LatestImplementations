using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataBase")]
public class CharacterDataBase : ScriptableObject
{
    public List<Character> characters = new List<Character>();

    public Character FindCharacterByName(string name)
    {
        int index = characters.FindIndex(x => x.name == name);

        if(index != -1)
        {
            return characters[index];
        }

        return null;
    }


}
[Serializable]
public class Character
{
    public string name;
    public int id;

    public AudioClip[] vowelVoice;
    public AudioClip punctuationVoice;

    [SerializedDictionary("EmotionName", "Sprites/Animations")]
    public SerializedDictionary<string, Sprite> data;

}