namespace Players;
using Associacao;

public class Jogador : Associados
{
    // Atributos encapsulados
    private int Codigo;
    public int codigo { get => Codigo; set => Codigo = value; }

    // Método contrutor
    public Jogador(string nome, int idade, Posicao posicao){
        Codigo = GerarCodigoUnico();
        this.Nome = nome;
        this.Idade = idade;
        this.Posicao = posicao;
    }

    // usando override em um metodo padrão C#
    public override string ToString()
    {
        return $"Código: {Codigo}, Nome: {Nome}, Idade: {Idade}, Posição: {Posicao}";
    }
}
