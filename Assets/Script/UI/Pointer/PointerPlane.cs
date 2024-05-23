using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PointerPlane : MonoBehaviour
{
    GameObject Plane;
    GameObject place;
    GameObject trans;
    GameObject prop;
    GameObject place_text;
    GameObject trans_text;
    GameObject prop_text;
    Image place_image;
    Image trans_image;
    Image prop_image;

    public Material hightlight;
    public Material normal;

    public List<Vector2> IconDir = new List<Vector2> { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1)};


    private Vector2 pos;

    public Vector2 CursorDir = Vector2.zero;

    private bool _show = false;
    private float ani_time = 0f;
    public bool Show
    {
        set
        {
            _show = value;
            if (_show)
            {
                ani_time = 0;
            }
            else
            {
                ani_time = 0.35f;
            }
        }
        get
        {
            return _show;
        }
    }

    public void PlaneButtonPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Show = !Show;
            Debug.Log("Pressed");
            // get mouse position on canvas
            Vector2 mousePos = Mouse.current.position.ReadValue();
            // move the plane to the mouse position
            if (Show)
            {
                Plane.transform.position = mousePos;
                pos = mousePos;
            }
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Plane = transform.GetChild(0).gameObject;
        Plane.SetActive(false);
        place = Plane.transform.GetChild(0).gameObject;
        trans = Plane.transform.GetChild(1).gameObject;
        prop = Plane.transform.GetChild(2).gameObject;
        place_text = Plane.transform.GetChild(3).gameObject;
        trans_text = Plane.transform.GetChild(4).gameObject;
        prop_text = Plane.transform.GetChild(5).gameObject;
        place_image = place.GetComponent<Image>();
        trans_image = trans.GetComponent<Image>();
        prop_image = prop.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_show)
        {
            ani_time += Time.deltaTime * 4;
            if (!Plane.activeSelf)
            {
                Plane.SetActive(true);
            }

            CursorDir = Mouse.current.position.ReadValue() - pos;

            if (CursorDir.magnitude > 0.2f * transform.localScale.x)
            {
                int i = 0;
                foreach (var dir in IconDir)
                {
                    if (Mathf.Abs(Vector2.Angle(CursorDir, dir)) < 45)
                    {
                        break;
                    }
                    i++;
                }
                switch (i)
                {
                    case 0:
                        place_image.material = hightlight;
                        trans_image.material = normal;
                        prop_image.material = normal;
                        break;
                    case 1:
                        place_image.material = normal;
                        trans_image.material = hightlight;
                        prop_image.material = normal;
                        break;
                    case 2:
                        place_image.material = normal;
                        trans_image.material = normal;
                        prop_image.material = hightlight;
                        break;
                    default:
                        place_image.material = normal;
                        trans_image.material = normal;
                        prop_image.material = normal;
                        break;
                }
            }
            else
            {
                place_image.material = normal;
                trans_image.material = normal;
                prop_image.material = normal;
            }

        }
        else
        {
            ani_time -= Time.deltaTime * 8;
            if (Plane.activeSelf && Plane.transform.localScale.x < 0.05f)
            {
                Plane.SetActive(false);
            }
        }
        Plane.transform.localScale = Vector3.one * UICurve.BounceCurve(ani_time);
    }
}
