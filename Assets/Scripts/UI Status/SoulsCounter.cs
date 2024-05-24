using Player.Managers;
using TMPro;
using UnityEngine;

namespace UIStatus
{
    public class SoulsCounter : MonoBehaviour
    {
        #region Variables / Properties

        [SerializeField] private TextMeshProUGUI soulsText;
        PlayerManager PManager => PlayerManager.Instance;

        #endregion

        #region Setup

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            if (PManager)
            {
                PManager.onPlayerSoulsChanged.AddListener(UpdateSoulsText);
                UpdateSoulsText(PManager.PlayerSouls);
            }
        }

        #endregion

        #region UI

        private void UpdateSoulsText(int souls)
        {
            soulsText.text = $"Souls: {souls}";
        }

        #endregion
    }
}
