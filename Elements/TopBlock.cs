using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBlock : Block
{
    [SerializeField]
    public ParticleSystem oneBlockFall;
    [SerializeField] Material particleMaterial;
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;
    public bool isPlaced;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isPlaced = false;
    }
    void Start()
    {
        oneBlockFall.transform.position = transform.position + new Vector3(0, 0, -1);
        oneBlockFall.GetComponent<Renderer>().material = particleMaterial;

        // audioSource = gameObject.AddComponent<AudioSource>();
        // audioSource.clip = audioClip;
    }
    public void BlockFall(){
        DisappearBlock();
        // oneBlockFall.Play();
        // audioSource.Play();
    }
    public void DisappearBlock(){
        spriteRenderer.color = new Color32(255, 255, 255, 0);
        isPlaced = false;
    }
    public void ReappearBlock(){
        spriteRenderer.color = Color.white;
        isPlaced = true;
    }
}
