using PROJETO_FUTEBOL.controller;
using View.Associados;
using View.Jogos;
using View.Partidas;
using View.Times;
using Projeto_futebol.Util;

namespace View.Principal;

public class ViewMenuPrincial
{
    public void ExibirMenuPrincipal()
    {
        string titulo = "FUTEBOL DO BABA";
        string[] opcoes = {
            "1 - Gestão de Associados",
            "2 - Gestão de Jogos",
            "3 - Gestão de Times",
            "4 - Gestão de Partidas",
            "0 - Sair"
        };
        Utils.ExibirJanela(titulo, opcoes, ConsoleColor.Cyan, 70);
        Utils.ExibirJanela("Escolha uma opção:", Array.Empty<string>(), ConsoleColor.Cyan, 70);
    }
}