using PROJETO_FUTEBOL.controller;
using View.Jogos;
using View.Partidas;
using View.Associados;
using View.Principal;

namespace View.Times;

public class ViewMenuTimes
{
     public bool MetodoExiste(object obj, string nomeMetodo)
    {
        return obj.GetType().GetMethod(nomeMetodo) != null;
    }
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
                    if (MetodoExiste(timeController, "GerarTimesPorPosicaoEquilibrada"))
                        timeController.GerarTimesPorPosicaoEquilibrada();
                    else
                    {
                        Console.WriteLine("Função ainda não implementada!");
                        Console.ReadKey();
                    }
                    break;
                case "3":
                    if (MetodoExiste(timeController, "GerarTimesPorCriterioDoGrupo"))
                        timeController.GerarTimesPorCriterioDoGrupo();
                    else
                    {
                        Console.WriteLine("Função ainda não implementada!");
                        Console.ReadKey();
                    }
                    break;
                case "4":
                    if (MetodoExiste(timeController, "DesfazerTimesDoJogo"))
                        timeController.DesfazerTimesDoJogo();
                    else
                    {
                        Console.WriteLine("Função ainda não implementada!");
                        Console.ReadKey();
                    }
                    break;
                case "5":
                    if (MetodoExiste(timeController, "ExibirTimesFormados"))
                        timeController.ExibirTimesFormados();
                    else
                    {
                        Console.WriteLine("Função ainda não implementada!");
                        Console.ReadKey();
                    }
                    break;
                case "0":
                    return; // Sai do menu
                default:
                    Console.WriteLine("Opção inválida! Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                    break;
            }
        }
    }
}