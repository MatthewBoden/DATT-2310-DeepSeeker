using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatusBarController : MonoBehaviour
    {
        [SerializeField] private Image bar;
        [SerializeField] private float maxAmount;
        
        private float _amount;

        private void Start()
        {
            _amount = maxAmount;
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
            _amount = Math.Clamp(_amount + change, 0, maxAmount);
            Debug.Log("Fill amount: " + _amount / maxAmount);
            bar.fillAmount = _amount / maxAmount;
        }
        
        private static void ValidateChange(float change)
        {
            if (change <= 0) throw new ArgumentOutOfRangeException(nameof(change), "Value must be a positive number.");
        }
    }
}