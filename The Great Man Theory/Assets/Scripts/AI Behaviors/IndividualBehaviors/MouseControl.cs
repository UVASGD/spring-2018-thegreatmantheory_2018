using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : Mover {

    Camera cam;


    protected override void SetMover() {
        cam = Camera.main;
    }


    protected override void GetInput() {
        target = cam.ScreenToWorldPoint(Input.mousePosition);

        //DASHING
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Dash();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            dash = MoveState.end;
        }
        else if (dashTimer <= 0) {
            dash = MoveState.end;
        }

        //BRACING
        if (Input.GetMouseButtonDown(braceButton)) {
            brace = MoveState.start;
        }
        else if (Input.GetMouseButtonUp(braceButton)) {
            brace = MoveState.end;
        }

        //HOLDING
        if (Input.GetMouseButtonDown(holdButton) && hasArms) {
            hold = MoveState.start;
        }
        else if (Input.GetMouseButtonUp(holdButton) && hasArms) {
            hold = MoveState.end;
        }
    }
}


