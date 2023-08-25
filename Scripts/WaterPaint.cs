using UnityEngine;


namespace Es.InkPainter.Sample
{
	[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
	public class WaterPaint : MonoBehaviour
	{
		[SerializeField]
		private Brush brush = null;

		[SerializeField] private Rigidbody rb;


		void Start(){
			Destroy(this.gameObject,3);
		}

		public void ShotFly(Vector3 vector){
        	rb.AddForce(vector,ForceMode.Impulse);
    	}


		public void OnCollisionEnter(Collision collision)
		{
			Vector3 hitpos = collision.contacts[0].point;
			var canvas = collision.contacts[0].otherCollider.GetComponent<InkCanvas>();
			if(canvas != null){
				canvas.Paint(brush, hitpos);
			}
			
			Destroy(this.gameObject);
		}
	}
}