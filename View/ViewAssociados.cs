using PROJETO_FUTEBOL.controller;
using View.Jogos;
using View.Partidas;
using View.Times;
using View.Principal;
using Projeto_futebol.Util;

namespace View.Associados;


public class ViewMenuAssociados
{
    public JogadorController jogadorController = new JogadorController();
    public void ExibirMenuAssociados()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Gestão de Associados ---");
            Console.WriteLine("1 - Cadastrar Associado");
            Console.WriteLine("2 - Listar Associados");
            Console.WriteLine("3 - Atualizar Associado");
            Console.WriteLine("4 - Excluir Associado");
            Console.WriteLine("0 - Voltar");
            Console.Write("Escolha uma opção: ");
            string? opcao = Console.ReadLine();

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
                    Utils.MensagemErro("Opção inválida! Pressione qualquer tecla para tentar novamente");
                    Console.ReadKey();
                    break;
            }
        }
    }
}