using System.Collections;
using UnityEngine;
using QFramework;

namespace PlatformShoot
{
    public class FixedEmplacement : PlatformShootGameController
    {
        private enum E_State
        {
            Idle, TakeAim, Shoot, CoolDown
        }

        private E_State state;
        private float radius = 7;
        private Transform target;

        private void Start()
        {
            state = E_State.Idle;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private IEnumerator TakeAim()
        {
            yield return new WaitForSeconds(2);
            state = E_State.Shoot;
        }

        private IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(1);
            state = E_State.Idle;
        }

        private void FixedUpdate()
        {
            switch(state)
            {
                case E_State.Idle:
                    target = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"))?.transform;
                    if(target != null)
                    {
                        state = E_State.TakeAim;
                        StartCoroutine(TakeAim());
                    }
                    break;
                case E_State.TakeAim:
                    break;
                case E_State.Shoot:
                    state = E_State.CoolDown;
                    this.GetSystem<IObjectPoolSystem>().Get("Item/Bullet", o =>
                    {
                        o.transform.localPosition = transform.position;
                        o.GetComponent<Bullet>().InitDir(target.position - transform.position);
                    });
                    StartCoroutine(CoolDown());
                    break;
                case E_State.CoolDown:
                    break;
            }
        }

        private void OnRenderObject()
        {
            if(target == null) return;
            if(state == E_State.TakeAim)
            {
                GL.PushMatrix();
                GL.Begin(GL.LINES);
                GL.Vertex(transform.position);
                GL.Vertex(target.position);
                GL.End();
                GL.PopMatrix();
            }
        }
    }
}
