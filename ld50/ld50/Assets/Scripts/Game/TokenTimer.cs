using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenTimer : MonoBehaviour
{
    public Token token;

    public SpriteRenderer timerSprite;
    private MaterialPropertyBlock timerPropertyBlock;

    private void Start() {
        timerPropertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        var newProgress = Mathf.Clamp(token.spawner.SpawnProgress, 0, 1);
        
        var tex = timerSprite.sprite.texture;
        timerSprite.GetPropertyBlock(timerPropertyBlock);
        timerPropertyBlock.SetFloat("_WipeProgress", 1 - newProgress);
        timerSprite.SetPropertyBlock(timerPropertyBlock);
    }
}
