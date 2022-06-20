using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner
{
    public class Coin : MonoBehaviour
    {
        void Update()
        {
            transform.Rotate(35 * Time.deltaTime * Vector3.right);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<AudioManager>().Play("CoinPickUp");
                PlayerManager.collectedCoins++;
                PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins", 0) + 1);
                Destroy(gameObject);
            }
        }
    }
}