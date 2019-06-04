using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterVoice", menuName = "Dialogue System/CharacterVoice", order = 1)]
public class CharacterVoice : ScriptableObject {
    public AudioClip voice;
    [Range(-2,2)]public float maxPitch = 1;
    [Range(-2,2)]public float minPitch = 1;
    [Range(0,1)] public float speed = 0.9f;

}