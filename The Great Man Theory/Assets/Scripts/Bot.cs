using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Mover {

    //protected ICommander commander;

    protected override void SetMover() {
        switch (body.unitType) {
            case UnitType.Pike:
                behavior = new MeleeBehavior(body, this, -100, 1.5f, 3, 8, 1.5f, 3, 0.4f, 8);
                break;
            case UnitType.Arquebus:
                behavior = new RangedBehavior(body, this, -100);
                break;
            default:
                behavior = new MeleeBehavior(body, this, -100, 1.5f, 3, 8, 1.5f, 3, 0.4f, 8);
                break;

        }
    }

    /*public bool SetCommand(LeafKey key, int priority) {
        return behavior.SetCommand(key, priority);
    } */

    /*public GameObject GetGameObject() {
        return gameObject;
    }

    public void SetCommander(ICommander comm) {
        commander = comm;
    }

    public ICommander GetCommander() {
        return commander;
    } */
}