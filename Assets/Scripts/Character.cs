using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float speed = 6.0f;
    CharacterController characterController;
    float moveValue = 0f;

    [Header("Animation")]
    [SerializeField] Animator animatorController;
    KeyCode inputRunning = KeyCode.LeftShift, inputBoxing = KeyCode.E;
    float boxingWeight = 0f;
    int armsLayerIndex;

    bool isBoxing = false;

    void Start()
    {
        armsLayerIndex = animatorController.GetLayerIndex("Arms");
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleBoxing();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");

        if (Input.GetButton("Horizontal"))
        {
            Vector3 move = transform.right * x + transform.forward;
            characterController.Move(move * speed * Time.deltaTime);

            // controlando valor de animacao da corrida
            if (Input.GetKey(inputRunning)) { moveValue = Mathf.MoveTowards(moveValue, 1f, Time.deltaTime); }
            else { moveValue = Mathf.MoveTowards(moveValue, 0.25f, Time.deltaTime); }

            animatorController.SetFloat("Move", moveValue);
        }
        else
        {
            moveValue = Mathf.MoveTowards(moveValue, 0f, Time.deltaTime * 5f);
            animatorController.SetFloat("Move", moveValue);
        }
    }

    void HandleBoxing()
    {
        if (Input.GetKeyDown(inputBoxing) && !isBoxing)
        {
            isBoxing = true;
            animatorController.Play("Boxing", armsLayerIndex, 0f);
            StartCoroutine(BoxingLayerWeightRoutine());
        }

        // transicao suave visualmente entre layers
        float targetWeight = isBoxing ? 1f : 0f;
        boxingWeight = Mathf.Lerp(boxingWeight, targetWeight, Time.deltaTime * 5f);
        animatorController.SetLayerWeight(armsLayerIndex, boxingWeight);
    }

    IEnumerator BoxingLayerWeightRoutine()
    {
        // esperando o tempo da animacao
        AnimatorClipInfo[] clips = animatorController.GetCurrentAnimatorClipInfo(armsLayerIndex);
        if (clips.Length > 0) { yield return new WaitForSeconds(clips[0].clip.length); }
        isBoxing = false;

        int currentAnimationBaseLayer = animatorController.GetCurrentAnimatorClipInfoCount(0);
        animatorController.CrossFade(currentAnimationBaseLayer, 0.1f, 0);
    }
}