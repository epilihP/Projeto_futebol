using PROJETO_FUTEBOL.controller;
using View.Jogos;
using View.Associados;
using View.Times;
using View.Principal;
using View.Partidas;
using Projeto_futebol.Util;

public class Menu
{
    private ViewMenuPrincial viewPrincipal = new ViewMenuPrincial();
    private ViewMenuAssociados viewAssociados = new ViewMenuAssociados();
    private ViewMenuJogos viewJogos = new ViewMenuJogos();
    private ViewMenuTimes viewTimes = new ViewMenuTimes();
    private ViewMenuPartidas viewPartidas = new ViewMenuPartidas();

    public void ExibirMenu()
    {
        while (true)
        {
            viewPrincipal.ExibirMenuPrincipal();
            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    viewAssociados.ExibirMenuAssociados();
                    break;
                case "2":
                    viewJogos.ExibirMenuJogos();
                    break;
                case "3":
                    viewTimes.ExibirMenuTimes();
                    break;
                case "4":
                    viewPartidas.ExibirMenuPartidas();
                    break;
                case "0":
                    Utils.ExibirJanela("Obrigado por usar o sistema!", Array.Empty<string>(), ConsoleColor.Green, 70);
                    return;
                default:
                    Utils.MensagemErro("Opção inválida! Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                    break;
            }
            // Limpa a tela após cada operação
            Console.Clear();
        }
    }
}
