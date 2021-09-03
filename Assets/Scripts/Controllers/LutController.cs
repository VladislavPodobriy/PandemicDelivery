using Pixelplacement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LutController : Singleton<LutController>
    {
        [SerializeField] private Button _maskBtn;
        [SerializeField] private Button _medicineKitBtn;
        [SerializeField] private Button _vaccineBtn;

        [SerializeField] private TextMeshProUGUI _maskCount;
        [SerializeField] private TextMeshProUGUI _medicineKitCount;
        [SerializeField] private TextMeshProUGUI _vaccineCount;
        [SerializeField] private TextMeshProUGUI _paperCount;

        private static PlayerData _playerData;
        
        private int _masks;
        private int _vaccines;
        private int _medicineKits;
    
        public void Awake()
        {
            if (LevelDataManager.IsEndless())
            {
                _playerData = new PlayerData
                {
                    Mask = 5,
                    Vaccine = 5,
                    MedicineKit = 5
                };
            }
            else
            {
                _playerData = PlayerDataManager.PlayerData;
            }

            _maskBtn.onClick.AddListener(UseMask);
            _medicineKitBtn.onClick.AddListener(UseMedicineKit);
            _vaccineBtn.onClick.AddListener(UseVaccine);
        
            Instance._vaccineCount.SetText(_playerData.Vaccine.ToString());
            Instance._paperCount.SetText(LevelController.GetScore().ToString());
            Instance._maskCount.SetText(_playerData.Mask.ToString());
            Instance._medicineKitCount.SetText(_playerData.MedicineKit.ToString());
        }

        public static void AddMask()
        {
            _playerData.Mask++;
            Instance._maskCount.SetText(_playerData.Mask.ToString());
        }

        public static void AddPaper()
        {
            LevelController.AddScore(1);
            Instance._paperCount.SetText(LevelController.GetScore().ToString());
        }

        public static void AddVaccine()
        {
            _playerData.Vaccine++;
            Instance._vaccineCount.SetText(_playerData.Vaccine.ToString());
        }

        private static void UseMask()
        {
            if (_playerData.Mask <= 0)
                return;

            CharacterController.Instance.ChangeHP(30);
            _playerData.Mask--;

            Instance._maskCount.SetText(_playerData.Mask.ToString());
        }

        private static void UseMedicineKit()
        {
            if (_playerData.MedicineKit <= 0)
                return;

            CharacterController.Instance.ChangeHP(100);
            _playerData.MedicineKit--;

            Instance._medicineKitCount.SetText(_playerData.MedicineKit.ToString());
        }

        private static void UseVaccine()
        {
            if (_playerData.Vaccine <= 0)
                return;

            CharacterController.Instance.ChangeHP(10);
            _playerData.Vaccine--;

            Instance._vaccineCount.SetText(_playerData.Vaccine.ToString());
        }
    }
}
