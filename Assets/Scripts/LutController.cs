using Pixelplacement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LutController : Singleton<LutController>
{
    [SerializeField] private Button _maskBtn;
    [SerializeField] private Button _medicineKitBtn;
    [SerializeField] private Button _vaccineBtn;

    [SerializeField] private TextMeshProUGUI _maskCount;
    [SerializeField] private TextMeshProUGUI _medicineKitCount;
    [SerializeField] private TextMeshProUGUI _vaccineCount;
    [SerializeField] private TextMeshProUGUI _paperCount;

    private static Data _data;

    public void Awake()
    {
        if (LevelManager.IsEndless)
        {
            _data = DataManager.Endless;

            _data.Mask = 5;
            _data.Vaccine = 5;
            _data.MedicineKit = 5;

            LevelController.Score = 0;
        }
        else
            _data = DataManager.Data;
        

        UpdateView();

        _maskBtn.onClick.AddListener(UseMask);
        _medicineKitBtn.onClick.AddListener(UseMedicineKit);
        _vaccineBtn.onClick.AddListener(UseVaccine);
    }

    public static void AddMask()
    {
        _data.Mask++;
        UpdateView();
    }

    public static void AddPaper()
    {
        LevelController.Score++;
        UpdateView();
    }

    public static void AddVaccine()
    {
        _data.Vaccine++;
        UpdateView();
    }

    private static void UseMask()
    {
        if (_data.Mask <= 0)
            return;

        Character.Instance.OnHeal(30);
        _data.Mask--;

        UpdateView();
    }

    private static void UseMedicineKit()
    {
        if (_data.MedicineKit <= 0)
            return;

        Character.Instance.OnHeal(100);
        _data.MedicineKit--;

        UpdateView();
    }

    private static void UseVaccine()
    {
        if (_data.Vaccine <= 0)
            return;

        Character.Instance.OnHeal(10);
        _data.Vaccine--;

        UpdateView();
    }

    public static void UpdateView()
    {
        Instance._maskCount.SetText(_data.Mask.ToString());
        Instance._medicineKitCount.SetText(_data.MedicineKit.ToString());
        Instance._vaccineCount.SetText(_data.Vaccine.ToString());
        Instance._paperCount.SetText(LevelController.Score.ToString());
    }
}
