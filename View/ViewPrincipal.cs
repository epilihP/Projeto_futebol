using PROJETO_FUTEBOL.controller;
using View.Associados;
using View.Jogos;
using View.Partidas;
using View.Times;

namespace View.Principal;


public class ViewMenuPrincial
{
    public void ExibirMenuPrincipal()
    {
       
        Console.Clear();
        Console.WriteLine("===== SISTEMA DE ORGANIZAÇÃO DE JOGOS DE FUTEBOL =====");
        Console.WriteLine("1 - Gestão de Associados");
        Console.WriteLine("2 - Gestão de Jogos");
        Console.WriteLine("3 - Gestão de Times");
        Console.WriteLine("4 - Gestão de Partidas");
        Console.WriteLine("0 - Sair");
        Console.Write("Escolha uma opção: ");
            
    
    }
}