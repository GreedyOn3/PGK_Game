using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class LevelSelectionUi : MonoBehaviour
    {
        [SerializeField] private LevelInfo[] levelInfos;
        [SerializeField] private GameObject levelCardPrefab;

        private void Start()
        {
            foreach (var level in levelInfos)
            {
                var card = Instantiate(levelCardPrefab, transform);
                var levelCardUi = card.GetComponent<LevelCardUi>();
                Assert.IsNotNull(levelCardUi, "Level card should have a LevelCardUi component.");
                levelCardUi.Initialize(level, this);
            }
        }

        public void PickLevel(LevelId levelId)
        {
            Debug.Log($"Picked level {levelId}");
        }
    }
}
