using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonImageChanger : MonoBehaviour
{
    [SerializeField] private Image buttonImage; // Reference to the button image component
    [SerializeField] private Sprite normalSprite; // Default button sprite
    [SerializeField] private Sprite pressedSprite; // Pressed button sprite
    [SerializeField] private float resetDelay = 0.2f; // Time before reverting to normal sprite

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        // Ensure the default sprite is set
        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }

        // Add click listener
        button.onClick.AddListener(ChangeButtonImage);
    }

    private void ChangeButtonImage()
    {
        if (buttonImage != null && pressedSprite != null)
        {
            buttonImage.sprite = pressedSprite;
            StartCoroutine(ResetButtonImage());
        }
    }

    private IEnumerator ResetButtonImage()
    {
        yield return new WaitForSeconds(resetDelay);

        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }
    }
}
