using src.kr.kro.minestar.utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace src.kr.kro.minestar.player.effect
{
    public sealed class Effects : MonoBehaviour
    {
        /// ##### Field #####
        private Player Player { get; set; }

        private Dictionary<Type, Effect> EffectMap { get; set; }


        /// ##### Value Field #####
        public float ValueMoveSpeed { get; private set; }

        public float ValueJumpForce { get; private set; }
        public int ValueBonusAirJump { get; private set; }

        public float ValueCoolTime { get; private set; }

        public bool ValueBondage { get; private set; }
        public bool ValueDisorder { get; private set; }

        /// ##### Unity Functions #####
        private void Start()
        {
            Player = GetComponent<Player>();
            EffectMap = new Dictionary<Type, Effect>();

            StartTimer();
        }

        /// ##### Coroutine Functions #####
        private void StartTimer()
        {
            StartCoroutine(Timer());

            IEnumerator Timer()
            {
                while (true)
                {
                    float valueMoveSpeed = 0;
                    float valueJumpForce = 0;
                    int valueBonusAirJump = 0;
                    float valueCoolTime = 0;
                    bool valueBondage = false;
                    bool valueDisorder = false;
                    foreach (Effect effect in EffectMap.Values.ToArray())
                    {
                        (effect as IEffectLimitTimer)?.DoPassesTime();

                        if (!(effect as IEffectFunction)?.IsActivate() ?? false) continue;

                        switch (effect.EffectType)
                        {
                            case EffectType.MoveSpeed:
                                valueMoveSpeed += effect.Value;
                                continue;
                            case EffectType.JumpForce:
                                valueJumpForce += effect.Value;
                                continue;
                            case EffectType.BonusAirJump:
                                valueBonusAirJump += Convert.ToInt32(effect.Value);
                                continue;
                            case EffectType.CoolTime:
                                valueCoolTime += effect.Value;
                                continue;
                            case EffectType.Bondage:
                                valueBondage = true;
                                continue;
                            case EffectType.Disorder:
                                valueDisorder = true;
                                continue;
                            default:
                                continue;
                        }
                    }

                    ValueMoveSpeed = valueMoveSpeed;
                    ValueJumpForce = valueJumpForce;
                    ValueBonusAirJump = valueBonusAirJump;
                    ValueCoolTime = valueCoolTime;
                    ValueBondage = valueBondage;
                    ValueDisorder = valueDisorder;

                    yield return new WaitForSeconds(0.01F);
                }
                // ReSharper disable once IteratorNeverReturns
            }
        }


        public void StopTimer() => StopAllCoroutines();


        /// ##### Add & Remove Functions #####
        public void AddEffect(Effect effect)
        {
            if (EffectMap.ContainsKey(effect.GetType())) RemoveEffect(effect.GetType());
            EffectMap.Add(effect.GetType(), effect);
        }

        public void RemoveEffect(Effect effect) => RemoveEffect(effect.GetType());

        public void RemoveEffect(Type type)
        {
            EffectMap.Remove(type);
        }
    }
}