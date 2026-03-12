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
            option1.onClick.AddListener(On1);
            option2.onClick.AddListener(On2);
            option3.onClick.AddListener(On3);
            option4.onClick.AddListener(On4);
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
        private async void On1() { Debug.Log("PACK BUTTON: heavy"); if (station != null) await station.SelectPackagingAsync(PackagingType.Heavy); }
        private async void On2() { Debug.Log("PACK BUTTON: fragile"); if (station != null) await station.SelectPackagingAsync(PackagingType.Fragile); }
        private async void On3() { Debug.Log("PACK BUTTON: fresh"); if (station != null) await station.SelectPackagingAsync(PackagingType.Fresh); }
        private async void On4() { Debug.Log("PACK BUTTON: normal"); if (station != null) await station.SelectPackagingAsync(PackagingType.Normal); }
    }
}