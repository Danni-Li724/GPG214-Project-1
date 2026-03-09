using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/Reaction Keys", fileName = "ReactionKeysSO")]
    public class ReactionKeysSO : ScriptableObject
    {
        public string SmileKey = "reaction/smile";
        public string SadKey = "reaction/sad";
    }
}