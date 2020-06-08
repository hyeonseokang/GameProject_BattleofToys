using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }
        public ThirdPersonCharacter character { get; private set; }
        private Transform target;
        private bool isEnabled = true;
        private bool isControlled = false;

        public bool isToy;
        Animator npcAnimator;

        public Animator childAnimator;
        public int StateBarNum;
        private void Start()
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            npcAnimator = GetComponent<Animator>();
            agent.updateRotation = false;
            agent.updatePosition = true;
            EnbaleNpc(true);
        }
        IEnumerator MoveSoundOff()
        {
            AudioSource MoveSound = GetComponent<AudioSource>();
            float Timef = 1;
            float MoveVolume = MoveSound.volume;
            Timef = 1;
            MoveVolume = Mathf.Lerp(0f, 1f, Timef);

            while (MoveVolume > 0f)
            {
                Timef -= Time.deltaTime / 0.1f;
                MoveVolume = Mathf.Lerp(0.0f, 1f, Timef);
                MoveSound.volume = MoveVolume;
                yield return null;
            }
        }
        public void KillNpc()
        {
            //StartCoroutine(MoveSoundOff());
            if(GetComponent<AudioSource>()!=null)
            {
                GetComponent<AudioSource>().mute = true;
            }
            GetComponent<PhotonView>().RPC("NpcDie", PhotonTargets.All);
        }
        
        [PunRPC]
        public void NpcDie()
        {
            EnbaleNpc(false);
            gameObject.layer = 2;
            StartCoroutine(DieCor());
        }

        IEnumerator DieCor()
        {
            GetComponent<Animator>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            int rand = Random.Range(0, 2);
            int turnCount = 0;
            while (turnCount < 16)
            {
                turnCount++;
                if (rand == 0)
                    transform.eulerAngles += Vector3.forward * 5f;
                else
                    transform.eulerAngles -= Vector3.forward * 5f;
                yield return false;
            }
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<CapsuleCollider>().isTrigger = true;
            yield return new WaitForSeconds(4f);
            Renderer[] renderers;
            renderers = GetComponentsInChildren<Renderer>();
            float time = 0f;
            float duration = 4f;
            while (time < duration)
            {
                time += Time.fixedDeltaTime;

                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i].material.HasProperty("_Color"))
                    {
                        Color originColor = renderers[i].material.GetColor("_Color");
                        renderers[i].material.SetColor("_Color", Color.Lerp(originColor, Color.clear, time / duration));
                    }
                }
                yield return new WaitForFixedUpdate();
            }
            Destroy(gameObject);
        }

        public void SetNpcDestination(Vector3 pos)
        {
            isControlled = true;
            agent.isStopped = false;
            agent.SetDestination(transform.position);
            agent.SetDestination(pos);
        }

        public void EnbaleNpc(bool state)
        {
            if (state)
            {
                agent.enabled = true;
                isEnabled = true;
                StartCoroutine(RandomMove());
            }
            else
            {
                StopAllCoroutines();
                agent.isStopped = true;
                isEnabled = false;
                agent.enabled = false;
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                KillNpc();
            }
            if (isEnabled)
            {
                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    character.Move(agent.desiredVelocity*0.5f, false, false, isToy);
                }
                else
                {
                    character.Move(Vector3.zero, false, false, isToy);
                }
            }
            if (isToy)
            {
                if(StateBarNum == 6)
                {
                    Debug.Log("³ª¶û ");
                    if (agent.desiredVelocity == Vector3.zero)
                        childAnimator.SetInteger("State", 0);
                    else
                        childAnimator.SetInteger("State", 1);
                }
                else
                {
                    Debug.Log("Âü±ú");
                    if (agent.desiredVelocity == Vector3.zero)
                        npcAnimator.SetInteger("State", 0);
                    else
                        npcAnimator.SetInteger("State", 1);
                }
             
            }
        }

        IEnumerator RandomMove()
        {
            agent.SetDestination(KHS_ObjectManager.instance.GetComponent<KJY_NpcSpawn>().GetRandomPos());
            agent.isStopped = false;
            yield return new WaitForSeconds(Random.Range(10f, 20f));
            if (isEnabled)
            {
                if(isControlled)
                {
                    yield return new WaitUntil(() => agent.desiredVelocity == Vector3.zero);
                }
                StartCoroutine(RandomMove());
            }
        }
    }
}
