using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
	public Grid parentGrid;
	public CubeIndex index;
    public Turret turret = null;

	public static Vector3 Corner(Vector3 origin, float radius, int corner, HexOrientation orientation){
		float angle = 60 * corner;
		if(orientation == HexOrientation.Pointy)
			angle += 30;
		angle *= Mathf.PI / 180;
		return new Vector3(origin.x + radius * Mathf.Cos(angle), 0.0f, origin.z + radius * Mathf.Sin(angle));
	}

	public static void GetHexMesh(float radius, HexOrientation orientation, ref Mesh mesh) {
		mesh = new Mesh();

		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Vector2> uvs = new List<Vector2>();

		for (int i = 0; i < 6; i++)
			verts.Add(Corner(Vector3.zero, radius, i, orientation));

		tris.Add(0);
		tris.Add(2);
		tris.Add(1);
		
		tris.Add(0);
		tris.Add(5);
		tris.Add(2);
		
		tris.Add(2);
		tris.Add(5);
		tris.Add(3);
		
		tris.Add(3);
		tris.Add(5);
		tris.Add(4);

		//UVs are wrong, I need to find an equation for calucalting them
		uvs.Add(new Vector2(0.5f, 1f));
		uvs.Add(new Vector2(1, 0.75f));
		uvs.Add(new Vector2(1, 0.25f));
		uvs.Add(new Vector2(0.5f, 0));
		uvs.Add(new Vector2(0, 0.25f));
		uvs.Add(new Vector2(0, 0.75f));

		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.uv = uvs.ToArray();

		mesh.name = "Hexagonal Plane";

		mesh.RecalculateNormals();
	}
}

#region Struct
[System.Serializable]
public struct OffsetIndex {
	public int row;
	public int col;

	public OffsetIndex(int row, int col){
		this.row = row; this.col = col;
	}
}
[System.Serializable]
public struct CubeIndex {
	public int x;
	public int y;
	public int z;

	public CubeIndex(int x, int y, int z){
		this.x = x; this.y = y; this.z = z;
	}

	public CubeIndex(int x, int z) {
		this.x = x; this.z = z; this.y = -x-z;
	}

	public static CubeIndex operator+ (CubeIndex one, CubeIndex two){
		return new CubeIndex(one.x + two.x, one.y + two.y, one.z + two.z);
	}

	public override bool Equals (object obj) {
		if(obj == null)
			return false;
		CubeIndex o = (CubeIndex)obj;
		if((System.Object)o == null)
			return false;
		return((x == o.x) && (y == o.y) && (z == o.z));
	}

	public override int GetHashCode () {
		return(x.GetHashCode() ^ (y.GetHashCode() + (int)(Mathf.Pow(2, 32) / (1 + Mathf.Sqrt(5))/2) + (x.GetHashCode() << 6) + (x.GetHashCode() >> 2)));
	}

	public override string ToString () {
		return string.Format("[" + x + "," + y + "," + z + "]");
	}
}
#endregion