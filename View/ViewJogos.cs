using PROJETO_FUTEBOL.controller;
using View.Associados;
using View.Partidas;
using View.Times;
using View.Principal;
using Projeto_futebol.Util;

namespace View.Jogos;

public class ViewMenuJogos
{
    public JogoController jogoController = new JogoController();
    public void ExibirMenuJogos()
    {
        while (true)
        {
            string titulo = "GESTÃO DE JOGOS";
            string[] opcoes = {
                "1 - Agendar Jogo",
                "2 - Listar Jogos",
                "3 - Atualizar Jogo",
                "4 - Excluir Jogo",
                "5 - Gerenciar Interessados",
                "0 - Voltar"
            };
            Utils.ExibirJanela(titulo, opcoes, ConsoleColor.Yellow, 70);
            Utils.ExibirJanela("Escolha uma opção:", Array.Empty<string>(), ConsoleColor.Yellow, 70);
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
                    Utils.MensagemErro("Opção inválida! Pressione qualquer tecla para tentar novamente", 70);
                    Console.ReadKey();
                    break;
            }
        }
    }
}
