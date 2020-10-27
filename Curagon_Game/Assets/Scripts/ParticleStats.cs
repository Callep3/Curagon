using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStats : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private Renderer _renderer;
    [SerializeField] private Material[] materials;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _renderer = _particleSystem.GetComponent<Renderer>();
    }
    
    public void SetMaterial (Particle_Material particleMat)
    {
        _renderer.material = materials[(int)particleMat];
    }

    public void Play()
    {
        SetMaterial(Particle_Material.Plus);
        _particleSystem.Play();
    }
    
    public void Play(Particle_Material statMat)
    {
        SetMaterial(statMat);
        _particleSystem.Play();
    }
}

public enum Particle_Material : int
{
    Plus,
    Minus
}
