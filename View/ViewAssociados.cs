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
            string titulo = "GESTÃO DE ASSOCIADOS";
            string[] opcoes = {
                "1 - Cadastrar Associado",
                "2 - Listar Associados",
                "3 - Atualizar Associado",
                "4 - Excluir Associado",
                "0 - Voltar"
            };
            Utils.ExibirJanela(titulo, opcoes, ConsoleColor.Magenta, 70);
            Utils.ExibirJanela("Escolha uma opção:", Array.Empty<string>(), ConsoleColor.Magenta, 70);
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
                    Utils.MensagemErro("Opção inválida! Pressione qualquer tecla para tentar novamente", 70);
                    Console.ReadKey();
                    break;
            }
        }
    }
}