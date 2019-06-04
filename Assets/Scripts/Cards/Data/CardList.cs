using UnityEngine;

namespace Database
{
    public static class CardList
    {
        static Card[] _cards;

        public static Card[] Cards{
            get{
                if(_cards == null){
                    _cards = Resources.LoadAll<Database.Card>("Cards");
                }
                return _cards;
            }
        }
        
    }
}



