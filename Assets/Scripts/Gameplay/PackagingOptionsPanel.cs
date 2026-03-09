using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay
{
    public class PackagingOptionsPanel : MonoBehaviour
    {
        [SerializeField] private Button option1;
        [SerializeField] private Button option2;
        [SerializeField] private Button option3;
        [SerializeField] private Button option4;

        private PackagingStation station;

        private void Awake()
        {
            option1.onClick.AddListener(OnOption1);
            option2.onClick.AddListener(OnOption2);
            option3.onClick.AddListener(OnOption3);
            option4.onClick.AddListener(OnOption4);
        }

        public void Show(PackagingStation s)
        {
            station = s;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            station = null;
            gameObject.SetActive(false);
        }

        private async void OnOption1() { if (station != null) await station.SelectPackagingAsync(PackagingType.Option1); }
        private async void OnOption2() { if (station != null) await station.SelectPackagingAsync(PackagingType.Option2); }
        private async void OnOption3() { if (station != null) await station.SelectPackagingAsync(PackagingType.Option3); }
        private async void OnOption4() { if (station != null) await station.SelectPackagingAsync(PackagingType.Option4); }
    }
}
