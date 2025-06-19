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
        var timeController = new TimeController(); // Instancia o controlador de times

        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Gestão de Times ---");
            Console.WriteLine("1 - Gerar Times por Ordem de Chegada");
            Console.WriteLine("2 - Gerar Times por Posição Equilibrada");
            Console.WriteLine("3 - Gerar Times pelo Critério do Grupo");
            Console.WriteLine("4 - Desfazer Times do Jogo");
            Console.WriteLine("5 - Ver Jogos e Times Formados");
            Console.WriteLine("0 - Voltar");
            Console.Write("Escolha uma opção: ");
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
                    Utils.MensagemErro("Opção inválida! Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                    break;
            }
        }
    }
}