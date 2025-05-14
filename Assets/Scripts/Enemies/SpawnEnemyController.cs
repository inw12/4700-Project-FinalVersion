using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyController : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyList;

    private Vector3 spawnOffset = new(0f, -1f, 0f);
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio() {
        if (audioSource) audioSource.Play();
    }

    public void SpawnEnemy() {
        // pick a random enemy from the list
        int i = Random.Range(0, enemyList.Count);
        // instantiate @ given position
        Instantiate(enemyList[i], transform.position + spawnOffset, Quaternion.identity);
    }

    public void SetInvisible() {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}