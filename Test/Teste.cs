using System.Collections.Generic;
using System;
using Jogos;

class Program
{
    static void Main(string[] args)
    {
        // Instancia um jogo (ajuste os valores conforme necessário)
        var jogo = new GerenciadorDeJogos();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== MENU FUTEBOL ===");
            Console.WriteLine($"Local: {jogo.Local}");
            Console.WriteLine($"Tipo de campo: {jogo.TipoCampo}");
            Console.WriteLine($"Status da lista de interessados: {(jogo.ListaDeInteressadosDisponivel() ? "OPEN" : "CLOSE")}");
            Console.WriteLine("1 - Entrar na lista de interessados");
            Console.WriteLine("2 - Ver lista de interessados");
            Console.WriteLine("3 - Ver associados");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");
            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    if (jogo.ListaDeInteressadosDisponivel())
                    {
                        Console.Write("Digite o RA do jogador para entrar na lista: ");
                        if (int.TryParse(Console.ReadLine(), out int ra))
                        {
                            jogo.AdicionarInteressado(ra);
                            Console.WriteLine("Adicionado com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("RA inválido!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("A lista está fechada! Não é possível adicionar interessados.");
                    }
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    break;
                case "2":
                    jogo.ExibirInteressados();
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    break;
                case "3":
                    jogo.ExibirAssociados();
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Opção inválida!");
                    Console.ReadKey();
                    break;
            }
        }
    }
}