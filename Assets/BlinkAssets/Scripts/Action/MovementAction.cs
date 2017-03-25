public class MovementAction : Action {

	public static readonly MovementAction Stand = new MovementAction ("Stand");
	public static readonly MovementAction MinusX = new MovementAction ("MinusX");
	public static readonly MovementAction MinusY = new MovementAction ("MinusY");
	public static readonly MovementAction MinusZ = new MovementAction ("MinusZ");
	public static readonly MovementAction PlusX = new MovementAction ("PlusX");
	public static readonly MovementAction PlusY = new MovementAction ("PlusY");
	public static readonly MovementAction PlusZ = new MovementAction ("PlusZ");
	public static readonly MovementAction MinusYRotate = new MovementAction ("MinusYRotate");
	public static readonly MovementAction PlusYRotate = new MovementAction ("PlusYRotate");
	public static readonly MovementAction ChangeHeroAngle = new MovementAction ("ChangeHeroAngle");
	public static readonly MovementAction ChangeCameraAngle = new MovementAction ("ChangeCameraAngle");
	public static readonly MovementAction ToggleRunWalk = new MovementAction ("ToggleRunWalk");

	public MovementAction(string name) : base(name) {
		
	}
}
