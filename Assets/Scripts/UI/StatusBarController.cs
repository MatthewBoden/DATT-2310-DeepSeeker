using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatusBarController : MonoBehaviour
    {
        [SerializeField] private float maxAmount;

        private Image _filler;
        public float Amount { get; private set; }

        private void Awake()
        {
            foreach (var image in GetComponentsInChildren<Image>())
            {
                if (image.gameObject.name != "Filler") continue;
                _filler = image;
                break;
            }

            if (_filler == null)
                throw new MissingComponentException($"{gameObject.name} is missing a Filler component.");

            Amount = maxAmount;
        }

        public void Increase(float change)
        {
            ValidateChange(change);
            Change(change);
        }

        public void Decrease(float change)
        {
            ValidateChange(change);
            Change(-change);
        }

        private void Change(float change)
        {
            Amount = Math.Clamp(Amount + change, 0, maxAmount);
            _filler.fillAmount = Amount / maxAmount;
        }

        private static void ValidateChange(float change)
        {
            if (change <= 0) throw new ArgumentOutOfRangeException(nameof(change), "Value must be a positive number.");
        }
    }
}