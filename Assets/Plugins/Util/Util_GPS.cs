using UnityEngine;
using System;
using System.Collections;

using OpenDis.Dis1998;

/// <summary>
/// This class provides static method to help convert between WGS84(latlon) and XYZ(meters)
/// </summary>
public class Util_GPS {
	
	static double RAD_TO_DEGREES = 180.0 / Math.PI;
	static double MAJOR_AXIS = 6378137.000;
	static double MINOR_AXIS = 6356752.314245;
	
	// -------------------------------------------------------------------------
	static public void GetMetersPerDegree(ref Vector2d originLatLon, out double metersPerDegreeLat, out double metersPerDegreeLon) {
		
		// calculations based on http://fmepedia.safe.com/articles/How_To/Calculating-accurate-length-in-meters-for-lat-long-coordinate-systems
		// and event better look at javascript source here http://www.csgnetwork.com/degreelenllavcalc.html
		
		double PI_DIV_180 = 3.1415926535897930 / 180.0;
		double rlat = originLatLon.x * PI_DIV_180;
		//double rlon = originLatLon.y * PI_DIV_180;
		
		metersPerDegreeLat =  111132.92 - 559.82 * Math.Cos(2.0 * rlat) + 1.175 * Math.Cos(4.0 * rlat) -0.0023 * Math.Cos(6.0 * rlat);
		metersPerDegreeLon =  111412.84 * Math.Cos(rlat) - 93.5 * Math.Cos(3.0 * rlat) + 0.118 * Math.Cos(5.0 * rlat);
	}
	
	// -------------------------------------------------------------------------
	static public float GetRelativeMeterDistanceBetweenLatLonVectors(ref Vector2d originLatLon, ref Vector2d otherLatLon) {
		
		double metersPerDegreeLat;
		double metersPerDegreeLon;
		
		GetMetersPerDegree(ref originLatLon, out metersPerDegreeLat, out metersPerDegreeLon);
		
		double relativeLat = otherLatLon.x - originLatLon.x;
		double relativeLon = otherLatLon.y - originLatLon.y;
		
		float relativeMetersNorth = (float)(relativeLat * metersPerDegreeLat);
		float relativeMetersEast = (float)(relativeLon * metersPerDegreeLon);
		
		return Mathf.Sqrt(relativeMetersNorth * relativeMetersNorth + relativeMetersEast * relativeMetersEast);
	}
	
	// -------------------------------------------------------------------------
	static public float GetRelativeBearingBetweenLatLonVectors(ref Vector2d originLatLon, ref Vector2d otherLatLon) {
		
		double metersPerDegreeLat = 0.0;
		double metersPerDegreeLon = 0.0;
		
		GetMetersPerDegree(ref originLatLon, out metersPerDegreeLat, out metersPerDegreeLon);	
		
		Vector2d relativeMeters = otherLatLon - originLatLon;
		relativeMeters.x *= metersPerDegreeLat;
		relativeMeters.y *= metersPerDegreeLon;
		relativeMeters.Normalize();
		
		Vector2d north = new Vector2d(1.0, 0.0);
		Vector2d east = new Vector2d(0.0, 1.0);			
		
		double angle = RAD_TO_DEGREES * Math.Acos(Vector2d.Dot(north, relativeMeters));
		double sign = (Vector2d.Dot(relativeMeters, east) > 0.0) ? 1.0: -1.0;
			
		return (float)(360.0f + angle * sign) % 360.0f;	
	}

	// -------------------------------------------------------------------------
	static public void GeocentricToGeodetic(double x, double y, double z, out double lat, out double lon, out double alt) {
		
		double a = MAJOR_AXIS;
		double b = MINOR_AXIS;
		
		double e2  = 1.0 - (b * b) / (a * a);   // 1st eccentricity sqrd
	    double ed2 = (a * a) / (b * b) - 1.0;   // 2nd eccentricity sqrd
	    double a2  = a * a;
	    double b2  = b * b;
	    double z2  = z * z;
	    double e4  = e2 * e2;
	    double r2  = x * x + y * y;
	    double r   = Math.Sqrt(r2);

	    double E2 = a2 - b2;
	
	    double F = 54.0 * b2 * z2;
	
	    double G = r2 + (1.0 - e2) * z2 - e2 * E2;
	
	    double C = e4 * F * r2 / (G * G * G);
	
	    double S = Math.Pow(1.0 + C + Math.Sqrt(C * C + 2.0 * C) , 1.0 / 3.0);
	
	    double t = S + 1.0 / S + 1.0;
	
	    double P = F / (3.0 * t * t * G * G);
	
	    double Q = Math.Sqrt(1.0 + 2.0 * e4 * P);
	
	    double r0 = -(P * e2 * r) / (1.0 + Q) + Math.Sqrt(0.5 * a2 * (1.0 + 1.0 / Q) - (P * (1 - e2) * z2) / (Q * (1.0 + Q)) - 0.5 * P * r2);
	
	    t = r - e2 * r0;
	    double U = Math.Sqrt(t * t + z2);
	    double V = Math.Sqrt(t * t + (1.0 - e2) * z2);
	
	    t = b2 / (a * V);
	
	    alt = U * (1.0 - t);
	    lat = Math.Atan2(z + ed2 * t * z , r);
	    lon = Math.Atan2(y, x);

	    // convert to degrees
	    lat = RAD_TO_DEGREES * lat;
	    lon = RAD_TO_DEGREES * lon;
	}
}
