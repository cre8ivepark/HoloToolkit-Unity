﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Microsoft.MixedReality.Toolkit.Experimental.Joystick
{
    /// <summary>
    /// Example script to demonstrate joystick control in sample scene
    /// </summary>
    public class JoystickController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The large or small game object that receives manipulation by the joystick.")]
        private GameObject objectToManipulate = null;

        public GameObject ObjectToManipulate
        {
            get => objectToManipulate;
            set => objectToManipulate = value;
        }

        [SerializeField]
        [Tooltip("A TextMeshPro object that displays joystick values.")]
        private TextMeshPro debugText = null;

        [SerializeField]
        [Tooltip("The joystick mesh that gets rotated when this control is interacted with.")]
        private GameObject joystickVisual = null;

        [SerializeField]
        [Tooltip("The mesh + collider object that gets dragged and controls the joystick visual rotation.")]
        private GameObject grabberVisual = null;

        [SerializeField]
        [Tooltip("Toggles on / off the GrabberVisual's mesh renderer because it can be dragged away from the joystick visual, it kind of breaks the illusion of pushing / pulling a lever.")]
        private bool showGrabberVisual = true;

        [SerializeField]
        [Tooltip("The speed at which the JoystickVisual and GrabberVisual move / rotate back to a neutral position.")]
        [Range(1, 20)]
        private int reboundSpeed = 5;

        [SerializeField]
        [Tooltip("How sensitive the joystick reacts to dragging left / right. Customize this value to get the right feel for your scenario.")]
        [Range(50, 300)]
        private int sensitivityLeftRight = 100;

        [SerializeField]
        [Tooltip("How sensitive the joystick reacts to pushing / pulling. Customize this value to get the right feel for your scenario.")]
        [Range(50, 300)]
        private int sensitivityForwardBack = 150;

        public enum JoystickMode { Move, Rotate, Scale }
        public JoystickMode mode = JoystickMode.Move;

        [SerializeField]
        [Tooltip("The distance multiplier for joystick input. Customize this value to get the right feel for your scenario.")]
        [Range(0.01f, 1f)]
        private float multiplierMove = 0.03f;

        [SerializeField]
        [Tooltip("The rotation multiplier for joystick input. Customize this value to get the right feel for your scenario.")]
        [Range(0.1f, 3.0f)]
        private float multiplierRotate = 1.1f;

        [SerializeField]
        [Tooltip("The scale multiplier for joystick input. Customize this value to get the right feel for your scenario.")]
        [Range(0.001f, 0.1f)]
        private float multiplierScale = 0.01f;

        Vector3 startPosition;
        Vector3 joystickGrabberPosition;
        Vector3 joystickVisualRotation;
        const int joystickVisualMaxRotation = 80;
        bool isDragging = false;


        private void Start()
        {
            startPosition = grabberVisual.transform.position;
            if(grabberVisual != null)
            {
                grabberVisual.GetComponent<MeshRenderer>().enabled = showGrabberVisual;
            }
        }
        void Update()
        {
            if (!isDragging)
            {
                // when dragging stops, move joystick back to idle
                if(grabberVisual != null)
                {
                    grabberVisual.transform.position = Vector3.Lerp(grabberVisual.transform.position, startPosition, Time.deltaTime * reboundSpeed);
                }
            }
            calculateJoystickRotation();
            applyJoystickValues();
        }
        void calculateJoystickRotation()
        {
            joystickGrabberPosition = grabberVisual.transform.position - startPosition;
            // Left Right
            joystickVisualRotation.z = Mathf.Clamp(-joystickGrabberPosition.x * sensitivityLeftRight,-joystickVisualMaxRotation, joystickVisualMaxRotation);
            // Forward Back
            joystickVisualRotation.x = Mathf.Clamp(joystickGrabberPosition.z * sensitivityForwardBack,-joystickVisualMaxRotation, joystickVisualMaxRotation);
            if (joystickVisual != null)
            {
                joystickVisual.transform.localRotation = Quaternion.Euler(joystickVisualRotation);
            }
        }
        void applyJoystickValues()
        {
            if(ObjectToManipulate != null)
            {
                if (mode == JoystickMode.Move)
                {
                    ObjectToManipulate.transform.position += (joystickGrabberPosition * multiplierMove);
                    if (debugText != null)
                    {
                        debugText.text = ObjectToManipulate.transform.position.ToString();
                    }
                }
                else if (mode == JoystickMode.Rotate)
                {
                    Vector3 newRotation = ObjectToManipulate.transform.rotation.eulerAngles;
                    // only take the horizontal axis from the joystick
                    newRotation.y += (joystickGrabberPosition.x * multiplierRotate);
                    newRotation.x = 0;
                    newRotation.z = 0;
                    ObjectToManipulate.transform.rotation = Quaternion.Euler(newRotation);
                    if (debugText != null)
                    {
                        debugText.text = ObjectToManipulate.transform.rotation.eulerAngles.ToString();
                    }
                }
                else if (mode == JoystickMode.Scale)
                {
                    // TODO: Clamp above zero
                    Vector3 newScale = new Vector3(joystickGrabberPosition.x, joystickGrabberPosition.x, joystickGrabberPosition.x) * multiplierScale;
                    ObjectToManipulate.transform.localScale += newScale;
                    if (debugText != null)
                    {
                        debugText.text = ObjectToManipulate.transform.localScale.ToString();
                    }
                }
            }
        }
        /// <summary>
        /// The ObjectManipulator script uses this to determine when the joystick is grabbed.
        /// </summary>
        public void StartDrag()
        {
            isDragging = true;
        }
        /// <summary>
        /// The ObjectManipulator script uses this to determine when the joystick is released.
        /// </summary>
        public void StopDrag()
        {
            isDragging = false;
        }
        /// <summary>
        /// Set the joystick to control movement only.
        /// </summary>
        public void JoystickMode_Move()
        {
            mode = JoystickMode.Move;
        }
        /// <summary>
        /// Set the joystick to control rotation only.
        /// </summary>
        public void JoystickMode_Rotate()
        {
            mode = JoystickMode.Rotate;
        }
        /// <summary>
        /// Set the joystick to control scale only.
        /// </summary>
        public void JoystickMode_Scale()
        {
            mode = JoystickMode.Scale;
        }
    }
}
