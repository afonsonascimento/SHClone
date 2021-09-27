
using System;

namespace VRTK.Examples
{
    using UnityEngine;

    public class GunShoot : MonoBehaviour
    {
        public VRTK_InteractableObject linkedObject;
        public GameObject projectile;
        public Transform projectileSpawnPoint;
        public float projectileSpeed = 1000f;
        public float projectileLife = 5f;
        public Rigidbody gunRigidbody;

        protected virtual void OnEnable()
        {
            linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

            if (linkedObject != null)
            {
                linkedObject.InteractableObjectUsed += InteractableObjectUsed;
                linkedObject.InteractableObjectGrabbed += GunGrabbed;
                linkedObject.InteractableObjectUngrabbed += GunNotGrabbed;
            }
        }

        protected virtual void OnDisable()
        {
            if (linkedObject != null)
            {
                linkedObject.InteractableObjectUsed -= InteractableObjectUsed;
                linkedObject.InteractableObjectGrabbed -= GunGrabbed;
                linkedObject.InteractableObjectUngrabbed -= GunNotGrabbed;

            }
        }

        protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
        {
            FireProjectile();
        }

        protected virtual void FireProjectile()
        {
            if (projectile != null && projectileSpawnPoint != null)
            {
                GameObject clonedProjectile = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                Rigidbody projectileRigidbody = clonedProjectile.GetComponent<Rigidbody>();
                float destroyTime = 0f;
                if (projectileRigidbody != null)
                {
                    projectileRigidbody.AddForce(clonedProjectile.transform.forward * projectileSpeed);
                    destroyTime = projectileLife;
                }
                Destroy(clonedProjectile, destroyTime);
            }
        }

        private void GunGrabbed(object sender, InteractableObjectEventArgs e)
        {
            gunRigidbody.interpolation = RigidbodyInterpolation.None;
        }

        private void GunNotGrabbed(object sender, InteractableObjectEventArgs e)
        {
            gunRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

    }
}