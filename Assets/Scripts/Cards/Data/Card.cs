using UnityEngine;


namespace Database
{
    [CreateAssetMenu(fileName = "Card Name", menuName = "Card/New Card", order = 0)]
    public class Card : ScriptableObject {
        public Sprite cover;
        public Sprite front;

        public new string name;
        [TextArea] 
        public string description;

        public enum rarityLevel{
            Common,
            Rare,
            SuperRare,
            SecretRare,
            UltimateRare

        };

        public rarityLevel rarity;
        
    }
}
