using PROJETO_FUTEBOL.controller;
using View.Jogos;
using View.Partidas;
using View.Associados;
using View.Principal;
using Projeto_futebol.Util;

namespace View.Times;

public class ViewMenuTimes
{
    public TimeController timeController = new TimeController();
    public void ExibirMenuTimes()
    {
        while (true)
        {
            string titulo = "GESTÃO DE TIMES";
            string[] opcoes = {
                "1 - Gerar Times por Ordem de Chegada",
                "2 - Gerar Times por Posição Equilibrada",
                "3 - Gerar Times pelo Critério do Grupo",
                "4 - Desfazer Times do Jogo",
                "5 - Ver Jogos e Times Formados",
                "0 - Voltar"
            };
            Utils.ExibirJanela(titulo, opcoes, ConsoleColor.Cyan, 70);
            Utils.ExibirJanela("Escolha uma opção:", Array.Empty<string>(), ConsoleColor.Cyan, 70);
            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    timeController.GerarTimesPorOrdemDeChegada();
                    break;
                case "2":
                    timeController.GerarTimesPorPosicaoEquilibrada();
                    break;
                case "3":
                    timeController.GerarTimesPorCriterioDoGrupo();
                    break;
                case "4":
                    timeController.DesfazerTimesDoJogo();
                    break;
                case "5":
                    timeController.ExibirTimesFormados();
                    break;
                case "0":
                    return; // Sai do menu
                default:
                    Utils.MensagemErro("Opção inválida! Pressione qualquer tecla para tentar novamente.", 70);
                    Console.ReadKey();
                    break;
            }
        }
    }
}