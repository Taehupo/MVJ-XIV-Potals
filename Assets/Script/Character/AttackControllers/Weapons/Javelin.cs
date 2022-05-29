using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Javelin : MonoBehaviour
{

    #region Members

    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private float lifeTime = 2f;

    protected ContactFilter2D contactFilter = new();

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        LayerMask enemyLayer = CharacterManager.Instance.attackLayerMask;
        contactFilter.SetLayerMask(enemyLayer);

        bool isFlipped = CharacterManager.Instance.SpriteManager.IsFlipped();
        Vector3 rot = new Vector3(0f, isFlipped ? 180f : 0f, 0f);
        transform.eulerAngles = rot;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * (isFlipped ? 1: -1), 0));
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        List<Collider2D> hitTargets = new List<Collider2D>();
        Physics2D.OverlapCollider(GetComponent<BoxCollider2D>(), contactFilter, hitTargets);
        foreach (Collider2D hitTarget in hitTargets)
        {
            //Debug.Log("Attacking " + hitTarget.name + " !");
            hitTarget.GetComponent<HealthManager>().TakeHit(2, this.gameObject);
        }
    }
}
