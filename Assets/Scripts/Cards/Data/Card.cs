using UnityEngine;


namespace Database
{
    [CreateAssetMenu(fileName = "Card Name", menuName = "Card/New Card", order = 0)]
    public class Card : ScriptableObject {
        public Sprite background;
        public Sprite front;

        public new string name;

        public int rareLevel;
        
    }
}
