using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandable {
	/* * 
	 * void SetCommand
	 * 
	 * @param comm: The type of command being sent
	 * 
	 * @return true if behavior was affected, false if not.
	 * 
	 */
	bool SetCommand (LeafKey comm, int priority);

	/* *
	 * GameObject GetGameObject
	 * 
	 * @return the gameobject this is associated with
	 * 
	 */
	GameObject GetGameObject();

	/* *
	 * void SetCommander
	 * 
	 * @param 
	 * 
	 */
	void SetCommander (ICommander commander);
}

public interface ICommander {
	Transform FindOfficer ();
	Transform FindMedic ();
	Transform FindTarget ();
	float SquadRadius ();
}