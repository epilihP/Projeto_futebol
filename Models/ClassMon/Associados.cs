using System.Diagnostics.Contracts;

namespace Associacao;

// enumeração de futsal
public enum Posicao
{
    AtacanteDireito = 1,
    AtacanteEsquerdo = 2,
    DefesaDireita = 3,
    DefesaEsquerda = 4,
    Goleiro = 5
}

public abstract class Associados
{
    // Encapsulamento
     protected string Nome;
     protected int Idade;
     protected Posicao Posicao;

     public string nome { get => Nome; set => Nome = value; }
     public int idade { get => Idade; set => Idade = value; }
     public Posicao posicao { get => Posicao; set => Posicao = value; }
}
