public class Action {

	public string name = "";

	public Action(string name) {
		this.name = name;
	}

	public override string ToString() {
		return name;
	}

	public override bool Equals(object a) {
		if (a == null || GetType () != a.GetType ())
			return false;
		return name.Equals (((Action) a).name);
	}

	public override int GetHashCode () {
		return name.GetHashCode ();
	}
}
