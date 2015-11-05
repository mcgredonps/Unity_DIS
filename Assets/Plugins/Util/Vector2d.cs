using System;


public struct Vector2d {
	
	public double x;
	public double y;
	
	// -------------------------------------------------------------------------
	public Vector2d(double xValue, double yValue){
		
		x = xValue;
		y = yValue;
	}
	
	// -------------------------------------------------------------------------
	public Vector2d(string xValue, string yValue){
		
		double.TryParse(xValue, out x);
		double.TryParse(yValue, out y);
	}
	
	// -------------------------------------------------------------------------
	public double Magnitude() {
		
		return Math.Sqrt(x * x + y * y);
	}
	
	// -------------------------------------------------------------------------
	public void Normalize() {
		
		double magnitude = Magnitude();
		
		if (magnitude > 0.0) {
			
			double inv = 1.0 / magnitude;
			x *= inv;
			y *= inv;
		}		
	}
	
	// -------------------------------------------------------------------------
	public static double Dot(Vector2d left, Vector2d right) {
		
		return left.x * right.x + left.y * right.y;
	}
	
	// -------------------------------------------------------------------------
	public override string ToString() {
		return "(" + x.ToString() + ", " + y.ToString() + ")";
	}
	
	// -------------------------------------------------------------------------
	public static Vector2d operator+(Vector2d left, Vector2d right) {
		
		Vector2d resultVector;
		resultVector.x = left.x + right.x;
		resultVector.y = left.y + right.y;
		
		return resultVector;
	}
	
	// -------------------------------------------------------------------------
	public static Vector2d operator-(Vector2d left, Vector2d right) {
		
		Vector2d resultVector;
		resultVector.x = left.x - right.x;
		resultVector.y = left.y - right.y;
		
		return resultVector;
	}
	
	// -------------------------------------------------------------------------
	public static Vector2d operator*(double left, Vector2d right) {
		
		Vector2d resultVector;
		resultVector.x = right.x * left;
		resultVector.y = right.y * left;
		
		return resultVector;
	}
	
	// -------------------------------------------------------------------------
	public static Vector2d operator*(Vector2d left, double right) {
		
		Vector2d resultVector;
		resultVector.x = left.x * right;
		resultVector.y = left.y * right;
		
		return resultVector;
	}	
};