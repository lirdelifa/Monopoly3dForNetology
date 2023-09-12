using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimate : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void EnableRunAnimation(bool enable) => _animator.SetBool("Run", enable);

}
