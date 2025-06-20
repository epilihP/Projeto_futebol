namespace Associacao;

using ICodigos;
using System.Collections.Generic;

// Enumeração de Futsal
public enum Posicao
{
    Atacante = 1,
    Defesa = 2,
    Goleiro = 3,
}

public class Associados : ICodigos<int>
{
    // Atributs encapsulados
    protected string Nome = string.Empty;
    protected int Idade;
    protected Posicao Posicao;
    public int Id { get; set; }
    public int Pontos { get; set; } // Pontuação do associado

    public string nome { get => Nome; set => Nome = value; }

    public int idade { get => Idade; set => Idade = value; }
    public Posicao posicao { get => Posicao; set => Posicao = value; }

    // Métodos
    public int GerarCodigoUnico(HashSet<int> codigosExistentes) // HashSet cria uma instancia na memória
    {
        Random rand = new Random(); // cria objeto para para gerar numero aleatorio
        int RA;
        do
        {
            RA = rand.Next(100000, 1000000); // 6 dígitos
        } while (codigosExistentes.Contains(RA)); // para sae código não existir
        return RA; // RA = Registro de Associado
    }
}