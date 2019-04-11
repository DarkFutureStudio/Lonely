using UnityEngine;

public class TriggerLever : EventTriggers
{
    public float m_Force = 30;
    public Transform m_AnchorPoint;
    public Transform m_VCam;

    private HingeJoint2D m_joint;
    private Rigidbody2D m_rigidbody;
    private bool m_trigger = false;

    public bool IsTriggered { get {return m_trigger;} }
    
    public override void OnTriggerEnter2D(Collider2D other)
    {
        EnteredTrigger(other);
    }
    public override void OnTriggerStay2D(Collider2D other)
    {
        EnteredTrigger(other);
    }
    public override void OnTriggerExit2D() {}
    private void EnteredTrigger(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetButtonDown("Interact"))
            {
                SetValues();
                //Trigger the lever
                m_rigidbody.AddForce(new Vector2(m_Force, 0), ForceMode2D.Impulse);
                m_trigger = true;
            }
            if (m_trigger)
                if (m_rigidbody.velocity == Vector2.zero)
                {
                    Destroy(m_joint);
                    Destroy(m_rigidbody);
                    m_VCam.GetChild(0).gameObject.SetActive(false);
                    m_VCam.GetChild(1).gameObject.SetActive(true);
                    Destroy(this);
                }
        }
    }
    private void SetValues()
    {
        //Refrences:
        m_rigidbody = gameObject.AddComponent<Rigidbody2D>();
        m_joint = gameObject.AddComponent<HingeJoint2D>();

        //rigidbody values:
        m_rigidbody.gravityScale = 0;
        m_rigidbody.mass = 2.16f;
        m_rigidbody.angularDrag = .31f;
        m_rigidbody.sleepMode = RigidbodySleepMode2D.StartAsleep;
        //limit angle
        JointAngleLimits2D angle = m_joint.limits;
        angle.max = 45;
        //Set joint at desire
        m_joint.limits = angle;
        m_joint.anchor = transform.InverseTransformPoint(m_AnchorPoint.position);
    }
}