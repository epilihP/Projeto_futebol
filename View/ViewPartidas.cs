using PROJETO_FUTEBOL.controller;
using View.Jogos;
using View.Associados;
using View.Times;
using View.Principal;
using Projeto_futebol.Util;

namespace View.Partidas;

public class ViewMenuPartidas
{
    public PartidaController partidaController = new PartidaController();
    public void ExibirMenuPartidas()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Gestão de Partidas ---");
            Console.WriteLine("1 - Registrar Nova Partida");
            Console.WriteLine("2 - Listar Partidas");
            Console.WriteLine("3 - Classificação de Associados");
            Console.WriteLine("0 - Voltar");
            Console.Write("Escolha uma opção: ");
            string? opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    partidaController.RegistrarPartida();
                    break;
                case "2":
                    partidaController.ListarPartidas();
                    break;
                case "3":
                    partidaController.ExibirClassificacaoAssociados();
                    break;
                case "0":
                    return;
                default:
                    Utils.MensagemErro("Opção inválida!");
                    Console.ReadKey();
                    break;
            }
        }
    }
}