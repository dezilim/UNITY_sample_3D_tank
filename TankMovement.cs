using UnityEngine;

public class TankMovement : MonoBehaviour
{
    // start from player 1 
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;    
    // control pitch of audio 
    public float m_PitchRange = 0.2f;

    // etc horizontal, vertical
    // inputs are configured in input manager, different players have different axis, get as a string
    private string m_MovementAxisName;     // vertical
    private string m_TurnAxisName;         // horizontal
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;         

    // initialise reference for rigidbody
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>(); 
    }

    // kinematic means no forces will be applied but physics allowed. On enable we want to move the tank
    // make the object kinematic if you don't want it to just bounce off after being hit by something...
    
    private void OnEnable ()
    {
        // TURN OFF KINEMATIC TO DRIVE TANK
        m_Rigidbody.isKinematic = false;
        // Reset values to 0
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }
    // switch tank on: not kinematic so you can drive it 

    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        // setup axes: etc player 1 : Horizontal1
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        // don't make reference with getComponent or else unity takes the first one by default 
        m_OriginalPitch = m_MovementAudio.pitch; //m_MovementAudio here will be the name of the specific audio source
    }
    

    private void Update()
    {
        // runs every frame : store input and fix updates (stores movement)
        // Store the player's input and make sure the audio for the engine is playing.
        // movement and turn axes were defined in start
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        // when tank just having a bit of input going to be idle
        if(Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            //check audio source 
            if(m_MovementAudio.clip == m_EngineDriving)
            {
                // playing the wrong clip 
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }

        }
        else // driving around
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // playing the wrong clip 
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();

            }
        }

    }


    private void FixedUpdate()
    {
        // input values are configured in unity 
        // Move and turn the tank. (update the values)
        Move();
        Turn();

    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        // Conversion 
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);

    }
}
