namespace Generics;
using IPartidaGenerica; //pegando a interface
using Players;
using Associacao;
//ideia central é usar a primeira parte da tarefa para implementar em partidas, aqui estamos usando o generico para adicionar os nomes dos associados interessados,
//da para implemantar outras coisas caso queira testar isso tambem porem com outro tipo ja que esse foi feito com string, é só herdar a Interface
// vou comentar em ingles e port algumas coisas para treino tambem e facil compreensão
//I found out that it's possible to inherit(herdar) one class and multiple interfaces//descobri que é possivel herdar 1 classe e multiplas interface

public class GenericPlayer : Associados, ICaixaObjeto<string?> 
{
    public List<string> ListaDeNomes {get; set;} = new List<string>(); //list for names of "associados" // lista para os nomes de associados
    
    public static int QuantidadeDeInteressados { get; set; } = 0;
    public void Guardar(string nome) //Method used to add a name to the list of player present in "associados"/metodo usado para adicionar o nome para a lista de jogadores que estão em associados
    { 
        Nome = nome;
        ListaDeNomes.Add(nome);
        if (nome! == null)
        {
            QuantidadeDeInteressados++; // só pra contar quantos interessados tem

        }
        else
        {
            Console.WriteLine("Não teve nome inserido!");
            return;
        }
        

    }

    
   
   
}

