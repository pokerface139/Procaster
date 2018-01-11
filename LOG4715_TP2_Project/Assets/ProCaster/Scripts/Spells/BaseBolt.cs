using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBolt : MonoBehaviour {

    public abstract bool FromPlayer();

    public virtual void Destroy() {
        Destroy(this.gameObject);
    }

}
