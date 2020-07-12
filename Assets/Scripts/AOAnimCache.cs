using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct AnimIdlePair
{
    public AnimationClip Anim;
    public AnimationClip IdleAnim;
}

public class AOAnimCache
{
    private Dictionary<int, AnimIdlePair> _bodyWeaponsCache = new Dictionary<int, AnimIdlePair>();
    private Dictionary<int, AnimationClip> _headCache = new Dictionary<int, AnimationClip>();
    private Dictionary<int, AnimationClip> _helmetCache = new Dictionary<int, AnimationClip>();

    public void BuildCache()
    {
        Object[] animations = Resources.LoadAll("Animations", typeof(AnimationClip));

        foreach (var animation in animations)
        {
            AnimationClip tempAnim = (AnimationClip)animation;

            if (tempAnim.name.Contains("IDLE_"))
            {
                SaveIdleAnim(tempAnim);
            }
            else if (tempAnim.name.Contains("IDLEWEAP_"))
            {
                SaveIdleAnim(tempAnim);
            }
            else if (tempAnim.name.Contains("HEAD_"))
            {
                int animIndex = int.Parse(tempAnim.name.Substring(tempAnim.name.IndexOf('_') + 1));

                if (_headCache.ContainsKey(animIndex))
                {
                    AnimationClip tempAnimClip = _headCache[animIndex];
                    tempAnimClip = tempAnim;
                    _headCache[animIndex] = tempAnimClip;
                }
                else
                {
                    _headCache.Add(animIndex, tempAnim);
                }
            }
            else if (tempAnim.name.Contains("HELMET_"))
            {
                int animIndex = int.Parse(tempAnim.name.Substring(tempAnim.name.IndexOf('_') + 1));

                if (_helmetCache.ContainsKey(animIndex))
                {
                    AnimationClip tempAnimClip = _helmetCache[animIndex];
                    tempAnimClip = tempAnim;
                    _helmetCache[animIndex] = tempAnimClip;
                }
                else
                {
                    _helmetCache.Add(animIndex, tempAnim);
                }
            }
            else //body or weapon anim
            {
                int animIndex = int.Parse(tempAnim.name);

                if (_bodyWeaponsCache.ContainsKey(animIndex))
                {
                    AnimIdlePair tempAnimIdlePair = _bodyWeaponsCache[animIndex];
                    tempAnimIdlePair.Anim = tempAnim;
                    _bodyWeaponsCache[animIndex] = tempAnimIdlePair;
                }
                else
                {
                    AnimIdlePair tempAnimIdlePair = new AnimIdlePair();
                    tempAnimIdlePair.Anim = tempAnim;
                    _bodyWeaponsCache.Add(animIndex, tempAnimIdlePair);
                }
            }
        }
    }

    public AnimationClip GetAnim(int Index)
    {
        if (_bodyWeaponsCache.ContainsKey(Index))
        {
            return _bodyWeaponsCache[Index].Anim;
        }
        else
        {
            Debug.LogError("GetAnim: index was not found on Anim cache: " + Index);
            return null;
        }

    }

    public AnimationClip GetIdleAnim(int Index)
    {
        if (_bodyWeaponsCache.ContainsKey(Index))
        {
            return _bodyWeaponsCache[Index].IdleAnim;
        }
        else
        {
            Debug.LogError("GetIdleAnim: index was not found on Anim cache: " + Index);
            return null;
        }

    }

    public AnimationClip GetHeadAnim(int Index)
    {
        if (_headCache.ContainsKey(Index))
        {
            return _headCache[Index];
        }
        else
        {
            Debug.LogError("GetHeadAnim: index was not found on Anim cache: " + Index);
            return null;
        }

    }

    public AnimationClip GetHelmetAnim(int Index)
    {
        if (_helmetCache.ContainsKey(Index))
        {
            return _helmetCache[Index];
        }
        else
        {
            Debug.LogError("GetHelmetAnim: index was not found on Anim cache: " + Index);
            return null;
        }

    }

    private void SaveIdleAnim(AnimationClip tempAnim)
    {
        int animIndex = int.Parse(tempAnim.name.Substring(tempAnim.name.IndexOf('_') + 1));

        if (_bodyWeaponsCache.ContainsKey(animIndex))
        {
            AnimIdlePair tempAnimIdlePair = _bodyWeaponsCache[animIndex];
            tempAnimIdlePair.IdleAnim = tempAnim;
            _bodyWeaponsCache[animIndex] = tempAnimIdlePair;
        }
        else
        {
            AnimIdlePair tempAnimIdlePair = new AnimIdlePair();
            tempAnimIdlePair.IdleAnim = tempAnim;
            _bodyWeaponsCache.Add(animIndex, tempAnimIdlePair);
        }
    }
}
