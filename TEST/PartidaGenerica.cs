// namespace Generics;
// using IGenerica; //pegando a interface
// using Players;
// using Associacao;

// public class GenericPlayer : Associados, ICaixa<string?> 
// {
//     public List<string> ListaDeNomes {get; set;} = new List<string>(); //list for names of "associados" // lista para os nomes de associados
    
//     public static int QuantidadeDeInteressados { get; set; } = 0;
//     public void Guardar(string nome) //Method used to add a name to the list of player present in "associados"/metodo usado para adicionar o nome para a lista de jogadores que estão em associados
//     { 
//         Nome = nome;
//         ListaDeNomes.Add(nome);
//         if (nome! == null)
//         {
//             QuantidadeDeInteressados++; // só pra contar quantos interessados tem

//         }
//         else
//         {
//             Console.WriteLine("Não teve nome inserido!");
//             return;
//         }
//     } 
// }