using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// For Particle Trigger to work. Add Collision to Trigger List.
/// This passes "OnParticleEnter" to collsion object.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class ParticleTriggerManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _system;
    [SerializeField, Tooltip("Time Threshold to Stop Particle"), Min(0)] private float _endTimeThreshold = 1;
    [SerializeField, Tooltip("Count Threshold to Stop Particle"), Min(0)] private int _particleCountThreshold = 0;

    [SerializeField] private List<string> playerTag = new() { "Player", "DamageCollider" };

    [SerializeField] private float damage = 1;

    public UnityEvent OnParticleEnd;

    private readonly List<ParticleSystem.Particle> _enter = new();
    private float _time = 0;

    public float MaxParticleCount { get; private set; }
    public float ParticleCount { get { return _system.particleCount; } }

    private void Awake()
    {
        if (!_system)
            _system = GetComponent<ParticleSystem>();

        List<GameObject> list = new();

        //Grabs colliders by tag
        playerTag.ForEach(tag => list.AddRange(GameObject.FindGameObjectsWithTag(tag)));

        foreach (GameObject go in list)
        {
            _system.trigger.AddCollider(go.GetComponent<Collider>());
        }

        MaxParticleCount = _system.startLifetime * _system.emission.rateOverTime.constant;
    }

    private void FixedUpdate()
    {
        if (_time > _endTimeThreshold)
        {
            EndParticle();
        }

        if (_system.particleCount <= _particleCountThreshold)
        {
            _time += Time.fixedDeltaTime;
        }
        else
        {
            _time = 0;
        }
    }

    /// <summary>
    /// Dispatch OnParticleTrigger to GameObject
    /// </summary>
    private void OnParticleTrigger()
    {
        int count = _system.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enter, out ParticleSystem.ColliderData enterData);

        for (int i = 0; i < count; i++)
        {
            if (enterData.GetColliderCount(i) > 0)
            {
                //Paritcle = _enter[i]

                Component data = enterData.GetCollider(i, 0);
                if (data.TryGetComponent<IDamageable>(out IDamageable damagable))
                {
                    damagable.Damage(damage);
                }

                if (data.TryGetComponent<IParticleTrigger>(out IParticleTrigger trigger))
                {
                    trigger.OnParticleEnter();
                }
            }
        }
    }

    private void EndParticle()
    {
        _system.Stop();
        enabled = false;
        OnParticleEnd.Invoke();
    }
}
