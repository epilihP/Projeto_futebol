using PROJETO_FUTEBOL.controller;

namespace PROJETO_FUTEBOL.Util 
{
    public class Menu
    {
        private JogadorController jogadorController;


        public Menu()
        {
       
            jogadorController = new JogadorController();
        }

        public void ExibirMenuPrincipal()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("--- Sistema de Gestão de Futebol ---");
        Console.WriteLine("=====================================");
        Console.WriteLine("1 - Gestão de Jogadores");
        Console.WriteLine("2 - Gestão de Jogos");
        Console.WriteLine("3 - Gestão de Times");
        Console.WriteLine("0 - Sair do Sistema");
        Console.WriteLine("=====================================");
        Console.Write("Escolha uma opção: ");

        string opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                ExibirMenuJogadores();
                break;
            case "2":
                ExibirMenuJogos(); // Chame o novo submenu
                break;
            case "3":
                // ExibirMenuTimes(); // Implemente depois
                Console.WriteLine("\nGestão de Times ainda não implementada.");
                Console.ReadKey();
                break;
            case "0":
                Console.WriteLine("\nObrigado por usar o sistema!");
                return;
            default:
                Console.WriteLine("\nOpção inválida!");
                Console.ReadKey();
                break;
        }
    }
}

        private JogoController jogoController = new JogoController(); // Adicione isso no início da classe Menu

// jogos
        private void ExibirMenuJogos()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Gestão de Jogos ---");
                Console.WriteLine("1 - Cadastrar Jogo");
                Console.WriteLine("2 - Listar Jogos");
                Console.WriteLine("3 - Atualizar Jogo");
                Console.WriteLine("4 - Excluir Jogo");
                Console.WriteLine("0 - Voltar ao Menu Principal");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        jogoController.CadastrarJogo();
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
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nOpção inválida!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // jogadores
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
                Console.WriteLine("0 - Voltar ao Menu Principal");
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
                        Console.WriteLine("\nOpção inválida!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}