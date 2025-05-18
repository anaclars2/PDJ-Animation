using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    [SerializeField] float speed = 6.0f;
    CharacterController characterController;
    [SerializeField] Animator animatorController;
    [SerializeField] KeyCode InputRun;
    [SerializeField] KeyCode InputPunch;
    float value = 0;

    void Start() { characterController = GetComponent<CharacterController>(); }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        if (Input.GetButton("Horizontal"))
        {
            Vector3 move = transform.right * x + transform.forward;
            characterController.Move(move * speed * Time.deltaTime);

            // animation
            if (Input.GetKey(InputRun)) { value += 0.05f; }
            else if (x >= 0.5f) { value = 0.25f; }
            animatorController.SetFloat("Move", value);
        }
        else
        {
            animatorController.SetFloat("Move", 0f);
            Debug.Log(0);
        }
    }
}