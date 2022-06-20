using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] GameObject[] tilePrefabs;
        float tileLenght = 30f;
        float zSpawn = 10f;
        int randomTilePicker;
        Transform player;
        Queue<GameObject> gameObjects = new Queue<GameObject>();
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            for (int i = 0; i < 5; i++)
            {
                SpawnRandomTile();
            }
        }
        private void Update()
        {
            if (player.transform.position.z > zSpawn - tileLenght * 3)
            {
                SpawnRandomTile();
                Destroy(gameObjects.Dequeue());
            }
        }
        void SpawnRandomTile()
        {
            randomTilePicker = Random.Range(0, tilePrefabs.Length);
            GameObject go = Instantiate(tilePrefabs[randomTilePicker], transform.forward * zSpawn, transform.rotation); ;
            zSpawn += tileLenght;

            gameObjects.Enqueue(go);
        }
    }
}