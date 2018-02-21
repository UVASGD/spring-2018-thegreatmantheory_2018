interface ICommandable {
	/* * 
	 * void SetCommand
	 * 
	 * @param comm: The type of command being sent
	 * 
	 * @return true if behavior was affected, false if not.
	 * 
	 */
	bool SetCommand (LeafKey comm, int priority);
}