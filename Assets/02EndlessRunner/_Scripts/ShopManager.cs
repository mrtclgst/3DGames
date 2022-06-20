using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace EndlessRunner
{
    public class ShopManager : MonoBehaviour
    {
        public ShopElement[] characters;
        public GameObject[] shopCharacters;
        [SerializeField] Button buyButton;


        int characterIndex;//0: granny, 1:ticky, 2:comfy

        private void Start()
        {
            foreach (ShopElement c in characters)
            {
                if (c.price != 0)
                    c.isLocked = PlayerPrefs.GetInt(c.name, 1) == 1 ? true : false;

            }
            characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

            foreach (GameObject ch in shopCharacters)
            {
                ch.SetActive(false);
            }
            shopCharacters[characterIndex].SetActive(true);

            UpdateUI();
        }
        public void ChangeNextCharacter()
        {
            shopCharacters[characterIndex].SetActive(false);
            characterIndex++;
            if (characterIndex == characters.Length)
                characterIndex = 0;
            shopCharacters[characterIndex].SetActive(true);
            UpdateUI();
            bool isLocked = characters[characterIndex].isLocked;
            if (isLocked)
                return;
            PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        }
        public void ChangePreviousCharacter()
        {
            shopCharacters[characterIndex].SetActive(false);
            characterIndex--;
            if (characterIndex == -1)
                characterIndex = characters.Length - 1;
            shopCharacters[characterIndex].SetActive(true);
            UpdateUI();
            bool isLocked = characters[characterIndex].isLocked;
            if (isLocked)
                return;
            PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        }
        public void UnlockCharacter()
        {
            ShopElement c = characters[characterIndex];

            if (PlayerPrefs.GetInt("TotalCoins", 0) < c.price)
                return;

            int newCoinNumber = PlayerPrefs.GetInt("TotalCoins", 0) - characters[characterIndex].price;
            PlayerPrefs.SetFloat("TotalCoins", newCoinNumber);
            PlayerPrefs.SetInt(c.name, 0);
            PlayerPrefs.SetInt("SelectedCharacter", characterIndex);

            UpdateUI();
        }
        private void UpdateUI()
        {
            ShopElement c = characters[characterIndex];

            if (c.isLocked)
            {
                buyButton.gameObject.SetActive(true);
                buyButton.GetComponentInChildren<TextMeshProUGUI>().text = c.price.ToString();

                if (PlayerPrefs.GetInt("TotalCoins", 0) < c.price)
                    buyButton.interactable = true;
                else
                    buyButton.interactable = true;
            }
            else
            {
                buyButton.gameObject.SetActive(false);
            }
        }
    }
}