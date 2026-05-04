using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    // Estados possíveis para as personagens
    public enum statusNPC { Idle = 0, Patrol = 1, Attack = 2, Death = 3 }

    // Pontos de Patrulha
    public Transform[] pontos;

    // Variaveis que vem de classes
    private Animator animator;
    public Transform olhos;
    public GameObject Player;
    NavMeshAgent agent;
    public Vida vida;
    public statusNPC status;

    // Indica se o NPC ataca o player
    public bool enemy = true;
    public int proxPonto = 0;
    public float distMinima = 1;
    public float speed = 3;
    public float distAtaca = 1;
    public int tiraVida = 40;
    public float distVisao = 50;
    public float angVisao = 90;
    public float tempoEspera = 5;
    public float tempoAEspera = 0;
    public float intervaloAtual = 0;
    public float intervaloAtacar = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        animator = GetComponent<Animator>();
        vida = GetComponent<Vida>();
        Player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        tempoAEspera = tempoEspera;
    }

    void deathStatus()
    {
        agent.isStopped = true;
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        status = statusNPC.Death;
    }

    void idleStatus()
    {
        agent.isStopped = true;
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        status = statusNPC.Idle;
        if (animator != null)
            animator.SetFloat("Movimento", 0); // Animação de ficar parado
    }

    void patrolStatus()
    {
        // Verificar se existe pontos para patrulhar
        if (pontos.Length == 0)
        {
            idleStatus();
            return;
        }
        if (agent.isOnNavMesh)
            agent.isStopped = false;
        agent.speed = speed;
        // Verificar se pode passar ao ponto seguinte
        if (Vector3.Distance(transform.position, pontos[proxPonto].position) < distMinima)
        {
            // Se tiver 1 ponto, vai ficar parado
            if (pontos.Length == 1)
            {
                idleStatus();
                return;
            }
            // Passa para o próximo ponto
            proxPonto++;
            if (proxPonto >= pontos.Length)
                proxPonto = 0;
        }
        // Definir o ponto para onde se move
        agent.SetDestination(pontos[proxPonto].position);

        // Definir a animação de andar
        if (animator != null)
            animator.SetFloat("Movimento", 0.5f); // Animação de andar
    }

    void attackStatus()
    {
        agent.isStopped = false;
        agent.speed = speed * 1.5f;
        if (animator != null)
            animator.SetFloat("Movimento", 0.5f); // Animação para andar

        // Rodar para o Player (ignorando o Y)
        Vector3 direction = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);
        transform.LookAt(direction);

        if (Vector3.Distance(transform.position, Player.transform.position) < distAtaca)
        {
            // Perto o suficiente para atacar
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            if (Time.time > intervaloAtual)
            {
                intervaloAtual = Time.time + intervaloAtacar;
                if (animator != null)
                    animator?.SetTrigger("Attack"); // Animação para atacar
                Player.GetComponent<Vida>().perdeVida(tiraVida);
            }
        }
        else
        {
            // Perseguir o player
            agent.isStopped = false;
            agent.SetDestination(Player.transform.position);
            if (animator != null)
                animator.SetFloat("Movimento", 1); // Animação de correr
        }
    }
    bool vePlayer()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) > distVisao)
        {
            return false;
        }

        return Utils.CanYouSeeThis(olhos, Player.transform, "Player", angVisao, distVisao);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent == null)
        {
            Debug.Log("Falta o NavMeshAgent!");
            return;
        }

        if (vida != null && vida.isDead)
        {
            deathStatus();
            return;
        }
        switch (status)
        {
            case statusNPC.Idle:
                idleStatus();
                if (enemy && vePlayer())
                    status = statusNPC.Attack;
                break;
            case statusNPC.Patrol:
                patrolStatus();
                if (enemy && vePlayer())
                    status = statusNPC.Attack;
                break;
            case statusNPC.Attack:
                if (enemy == false)
                {
                    status = statusNPC.Patrol;
                    return;
                }

                if (vePlayer())
                {
                    attackStatus();
                    tempoAEspera = tempoEspera;

                }
                else
                {
                    tempoAEspera -= Time.deltaTime;
                    if (tempoAEspera < 0)
                    {
                        patrolStatus();
                        tempoAEspera = tempoEspera;
                    }
                }
                break;
        }
    }
}