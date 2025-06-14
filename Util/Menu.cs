using PROJETO_FUTEBOL.controller;

public class Menu
{
    private JogadorController jogadorController = new JogadorController();
    private JogoController jogoController = new JogoController();
    //private TimeController timeController = new TimeController();
    //private PartidaController partidaController = new PartidaController();

    public void ExibirMenuPrincipal()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== SISTEMA DE ORGANIZAÇÃO DE JOGOS DE FUTEBOL =====");
            Console.WriteLine("1 - Gestão de Jogadores");
            Console.WriteLine("2 - Gestão de Jogos");
            Console.WriteLine("3 - Gestão de Times");
            Console.WriteLine("4 - Gestão de Partidas");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    ExibirMenuJogadores();
                    break;
                case "2":
                    ExibirMenuJogos();
                    break;
                case "3":
                    ExibirMenuTimes();
                    break;
                case "4":
                    //ExibirMenuPartidas();
                    break;
                case "0":
                    Console.WriteLine("\nObrigado por usar o sistema!");
                    return;
                default:
                    Console.WriteLine("Opção inválida! Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ExibirMenuJogadores()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Gestão de Jogadores ---");
            Console.WriteLine("1 - Cadastrar Jogador");
            Console.WriteLine("2 - Listar Jogadores");
            Console.WriteLine("3 - Atualizar Jogador");
            Console.WriteLine("4 - Excluir Jogador");
            Console.WriteLine("0 - Voltar");
            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    jogadorController.CadastrarJogador();
                    break;
                case "2":
                    jogadorController.ListarJogadores();
                    break;
                case "3":
                    jogadorController.AtualizarJogador();
                    break;
                case "4":
                    jogadorController.ExcluirJogador();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Opção inválida! Pressione qualquer tecla para tentar novamente");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ExibirMenuJogos()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Gestão de Jogos ---");
            Console.WriteLine("1 - Agendar Jogo");
            Console.WriteLine("2 - Listar Jogos");
            Console.WriteLine("3 - Atualizar Jogo");
            Console.WriteLine("4 - Excluir Jogo");
            Console.WriteLine("5 - Gerenciar Interessados");
            Console.WriteLine("0 - Voltar");
            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    jogoController.AgendarJogo();
                    break;
                case "2":
                    jogoController.ListarJogos();
                    break;
                case "3":
                    jogoController.AtualizarJogo();
                    break;
                case "4":
                    jogoController.ExcluirJogo();
                    break;
                case "5":
                    jogoController.GerenciarInteressados();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Opção inválida! Pressione qualquer tecla para tentar novamente");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private bool MetodoExiste(object obj, string nomeMetodo)
    {
        return obj.GetType().GetMethod(nomeMetodo) != null;
    }

    private void ExibirMenuTimes()
    {
        var timeController = new TimeController(); // Instancia o controlador de times

        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Gestão de Times ---");
            Console.WriteLine("1 - Gerar Times por Ordem de Chegada");
            Console.WriteLine("2 - Gerar Times por Posição Equilibrada");
            Console.WriteLine("3 - Gerar Times pelo Critério do Grupo");
            Console.WriteLine("0 - Voltar");
            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();

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
                    if (MetodoExiste(timeController, "GerarTimesPeloCriterioDoGrupo"))
                        timeController.GerarTimesPeloCriterioDoGrupo();
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

//     private void ExibirMenuPartidas()
//     {
//         while (true)
//         {
//             Console.Clear();
//             Console.WriteLine("--- Gestão de Partidas ---");
//             Console.WriteLine("1 - Registrar Nova Partida");
//             Console.WriteLine("2 - Listar Partidas");
//             Console.WriteLine("3 - Registrar Resultado");
//             Console.WriteLine("0 - Voltar");
//             Console.Write("Escolha uma opção: ");
//             string opcao = Console.ReadLine();

//             switch (opcao)
//             {
//                 case "1":
//                     partidaController.RegistrarPartida();
//                     break;
//                 case "2":
//                     partidaController.ListarPartidas();
//                     break;
//                 case "3":
//                     partidaController.RegistrarResultado();
//                     break;
//                 case "0":
//                     return;
//                 default:
//                     Console.WriteLine("Opção inválida!");
//                     Console.ReadKey();
//                     break;
//             }
//         }
//     }
// }