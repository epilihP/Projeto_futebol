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
            string titulo = "GESTÃO DE PARTIDAS";
            string[] opcoes = {
                "1 - Registrar Nova Partida",
                "2 - Listar Partidas",
                "3 - Classificação de Associados",
                "0 - Voltar"
            };
            Utils.ExibirJanela(titulo, opcoes, ConsoleColor.Green, 70);
            Utils.ExibirJanela("Escolha uma opção:", Array.Empty<string>(), ConsoleColor.Green, 70);
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
                    Utils.MensagemErro("Opção inválida!", 70);
                    Console.ReadKey();
                    break;
            }
        }
    }
}