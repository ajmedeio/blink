public class HeroAction : Action {

	public static readonly HeroAction Stand = new HeroAction ("Stand");
	public static readonly HeroAction MinusX = new HeroAction ("MinusX");
	public static readonly HeroAction MinusY = new HeroAction ("MinusY");
	public static readonly HeroAction MinusZ = new HeroAction ("MinusZ");
	public static readonly HeroAction PlusX = new HeroAction ("PlusX");
	public static readonly HeroAction PlusY = new HeroAction ("PlusY");
	public static readonly HeroAction PlusZ = new HeroAction ("PlusZ");
	public static readonly HeroAction MinusYRotate = new HeroAction ("MinusYRotate");
	public static readonly HeroAction PlusYRotate = new HeroAction ("PlusYRotate");
	public static readonly HeroAction ChangeHeroAngle = new HeroAction ("ChangeHeroAngle");
	public static readonly HeroAction ChangeCameraAngle = new HeroAction ("ChangeCameraAngle");
	public static readonly HeroAction ToggleRunWalk = new HeroAction ("ToggleRunWalk");

	public HeroAction(string name) : base(name) {
		
	}
}
