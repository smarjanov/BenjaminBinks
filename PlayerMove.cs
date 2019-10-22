using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance;

    [Header ("Movement settigns")]
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private float movementSpeed;
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpForce;
    private CharacterController con;
    private bool isJumping;

    [Header ("Player stats")]
    public float healthPoints;
    public RawImage bloodSplatter;
    public GameObject deathPanel;
    public Text healthPointsT;
    public Text deathText;


    private void Awake()
    {
        instance = this;
        con = GetComponent<CharacterController>();
    }

    private void Start()
    {
        bloodSplatter.enabled = false;
        deathPanel.SetActive(false);
        deathText.text = "";
    }

    private void Update()
    {
        PlayerMovement();
        healthPointsT.text = "HEALTH: "+healthPoints;
    }

    void PlayerMovement()
    {
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        con.SimpleMove(forwardMovement + rightMovement);

        if (Input.GetKeyDown(KeyCode.LeftShift)){
            movementSpeed = movementSpeed * 2;
        }
        else if  (Input.GetKeyUp(KeyCode.LeftShift)){
            movementSpeed = movementSpeed / 2;
        }

        Jump();

    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump")&& !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        } 
    }

    IEnumerator JumpEvent()
    {
        con.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce1 = jumpFallOff.Evaluate(timeInAir);
            con.Move(Vector3.up * jumpForce1 * jumpForce * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        }
        while (!con.isGrounded && con.collisionFlags != CollisionFlags.Above);

        con.slopeLimit = 45.0f;
        isJumping = false;

    }

    public void TakeDmg(float dmgAmount)
    {
        healthPoints -= dmgAmount;
        if (healthPoints <= 0f)
        {
            Die();
        }
    }

    public void HealPlayer(float healAmount)
    {
        if(healAmount < 100) { 
        healthPoints += healAmount;
        if(healthPoints > 100)
        {
            healthPoints = 100;
        }
        }
    }

    void Die()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        #region End Game Display
        int score = GameManager.instance.score;
        GameManager.instance.PauseGame();
        deathPanel.SetActive(true);
        if(score <= 1000)
        {
            deathText.text ="Total score: " + score + " = PATHETIC!";
        }
        else if (score <= 2000)
        {
            deathText.text = "Total score: " + score + " = WEAK!";
        }
        else if (score <= 3000)
        {
            deathText.text = "Total score: "+ score + " = OKAY";
        }
        else if (score <= 4000)
        {
            deathText.text = "Total score: " + score + " = BETTER";
        }
        else if (score <= 5000)
        {
            deathText.text = "Total score: " + score + " = FANTASTIC!";
        }
        #endregion
    }
}
