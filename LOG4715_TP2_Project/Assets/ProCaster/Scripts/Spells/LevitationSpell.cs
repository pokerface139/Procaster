using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevitationSpell : BaseSpell
{
    public enum State
    {
        Unselected,
        Selected
    }
    private Transform LastHoveredObject { get; set; }
    private Plane _CollisionPlane;
    private Transform _Player { get; set; }
    private Transform _Emitter { get; set; }
    public State _CurrentState { get; set; }
    public Transform _selectedObject { get; set; }

    public float toVel = 2.5f;
    public float maxVel = 15.0f;
    public float maxForce = 40.0f/100;
    public float gain = 5f;

    public float incrRotation=0.01f;

    // Use this for initialization
    public LevitationSpell(Transform player, Transform wand)
    {
        _CurrentState = State.Unselected;
        _Player = player;
        _Emitter = wand.Find("SpellEmitter");
        _CollisionPlane = new Plane(new Vector3(1,0,0),0);
        SpriteImage = Resources.Load<Sprite>("Levitation");
    }

    // Update is called once per frame
    public override void UpdateSpell() {

        if (_CurrentState.Equals(State.Selected))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (_CollisionPlane.Raycast(ray, out rayDistance))
            {
                Vector3 rayPos = ray.GetPoint(rayDistance);
                rayPos.x = _Player.position.x;

                Vector3 dist = rayPos - _selectedObject.position;

                Vector3 tgtVel = Vector3.ClampMagnitude(toVel * dist, maxVel);
                // calculate the velocity error
                Vector3 error = tgtVel - _selectedObject.gameObject.GetComponent<Rigidbody>().velocity;
                // calc a force proportional to the error (clamped to maxForce)
                Vector3 force = Vector3.ClampMagnitude(gain * error, maxForce);
                _selectedObject.gameObject.GetComponent<Rigidbody>().AddForce(force);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                Quaternion quat = new Quaternion();
                _selectedObject.gameObject.GetComponent<Rigidbody>().AddTorque(-incrRotation, 0, 0);

            }
            else if (Input.GetKey(KeyCode.E))
            {
                _selectedObject.gameObject.GetComponent<Rigidbody>().AddTorque(incrRotation, 0, 0);

            }
            else
            {
                _selectedObject.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3();
            }
        }
        else
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.tag.Equals("Levitable"))
                {
                    //on highlight l'objet (COMING SOON)
                    if (LastHoveredObject != null)
                    {
                        SetMaterialAlpha(LastHoveredObject, 1f);
                    }
                    LastHoveredObject = hit.transform;

                    SetMaterialAlpha(LastHoveredObject, 0.5f);
                    Debug.Log("transparents");
                }else if (LastHoveredObject != null)
                {
                    SetMaterialAlpha(LastHoveredObject, 1f);


                    LastHoveredObject = null;
                }
            }
        }
    }

    private void SetMaterialAlpha(Transform obj, float alpha)
    {
        Material mat = obj.GetComponent<MeshRenderer>().material;
        Color color = mat.color;
        mat.color = new Color(color.r, color.g, color.b, alpha);
    }
    //
    public override void ImmediateCast()
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Floor");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_CurrentState == State.Unselected && Physics.Raycast(ray, out hit,100f,mask))
        {
            if (hit.transform.tag.Equals("Levitable"))
            {
                _selectedObject = hit.transform;
                _selectedObject.GetComponent<Rigidbody>().mass = 0.01f;
                
                _selectedObject.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionX;
                _CurrentState = State.Selected;
            }
      
        }
        else if(_CurrentState == State.Selected)
        {

            _selectedObject.GetComponent<Rigidbody>().mass = 100f;

            _selectedObject.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionX;

            SetMaterialAlpha(_selectedObject, 1f);


            _CurrentState = State.Unselected;
            _selectedObject = null;


        }
    }

    public override void Cancel()
    {
        if (_CurrentState == State.Selected)
        {
            _selectedObject.GetComponent<Rigidbody>().useGravity = true;
            _selectedObject.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionX;
            _selectedObject.GetComponent<Rigidbody>().mass = 100f;

            SetMaterialAlpha(_selectedObject, 1f);


            _CurrentState = State.Unselected;
            _selectedObject = null;
        }
    }


}
