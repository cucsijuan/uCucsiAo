using System.Collections.Generic;
using UnityEngine;

public class AOSpriteCache
{
    private Dictionary<int, Sprite> _cache = new Dictionary<int, Sprite>();

    public void BuildCache()
    {
        Object[] sprites = Resources.LoadAll("Sprites", typeof(Sprite));

        foreach (var sprite in sprites)
        {
            Sprite tempSprite = (Sprite)sprite;

            _cache.Add(System.Int32.Parse(tempSprite.name), tempSprite);
        }
    }

    public Sprite GetSprite(int Index)
    {
        if (_cache.ContainsKey(Index))
        {
            return _cache[Index];
        }
        else
        {
            Debug.LogError("GetSprite: index was not found on sprite cache: " + Index);
            return null;
        }
        
    }
}
