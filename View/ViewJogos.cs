using PROJETO_FUTEBOL.controller;
using View.Associados;
using View.Partidas;
using View.Times;
using View.Principal;

namespace View.Jogos;

public class ViewMenuJogos
{
    public JogoController jogoController = new JogoController();
    public void ExibirMenuJogos()
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
            string? opcao = Console.ReadLine();

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

}
