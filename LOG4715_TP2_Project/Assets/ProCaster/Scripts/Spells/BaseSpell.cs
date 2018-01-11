using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpell {


    public Sprite SpriteImage { get; set; }

    virtual public void ImmediateCast() { }
    virtual public void Cast() { }
    virtual public void UpdateSpell() { }
    virtual public void Cancel() {}
}
