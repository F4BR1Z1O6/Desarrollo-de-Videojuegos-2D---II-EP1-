using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlDeMovimientoYSalto : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movimiento")]
    public float velocidadDeMovimiento = 5f;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [SerializeField] private bool enSuelo;
    public float fuerzaDeSalto = 10f; 

    [Header("Animacion")]
    private Animator animator;

    private int colisionesConEnemigo = 0;
    private int colisionesNecesarias = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        Mover(movimientoHorizontal);
        movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento;
        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            Saltar();
        }
    }

    private void FixedUpdate()
    {
        animator.SetBool("enSuelo", enSuelo);
    }

    void Saltar()
    {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaDeSalto);
    }

    private void Mover(float mover)
    {
        Vector3 velocidadObjetivo = new Vector2(mover * velocidadDeMovimiento, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, velocidadObjetivo, ref velocidad, 0.05f);
        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
        else if (collision.gameObject.CompareTag("Enemigo"))
        {
            colisionesConEnemigo++;

            if (colisionesConEnemigo >= colisionesNecesarias)
            {
                Destroy(gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}




