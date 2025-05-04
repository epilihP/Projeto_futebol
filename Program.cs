using Players;
using Associacao;
        
        Console.WriteLine("Teste 1: Adicionar jogador");
        Jogador.AdicionarJogador(new Jogador("Philipe", 25, Posicao.AtacanteDireito));
        Jogador.AdicionarJogador(new Jogador("Alisson", 22, Posicao.Goleiro));
        Console.WriteLine("Jogadores adicionados com sucesso!");

        Console.WriteLine("\nTeste 2: Listar jogadores");
        var jogadores = Jogador.ListarJogadores();
        foreach (var jogador in jogadores)
        {
            Console.WriteLine(jogador.ToString());
        }

        Console.WriteLine("\nTeste 3: Atualizar jogador");
        if (jogadores.Count > 0)        
        {
            var primeiroJogador = jogadores[0];
            bool atualizado = Jogador.AtualizarJogador(primeiroJogador.codigo, "Carlos Silva", 26, Posicao.Goleiro);
            Console.WriteLine(atualizado ? "Jogador atualizado com sucesso!" : "Falha ao atualizar jogador.");
        }

        Console.WriteLine("\nTeste 4: Remover jogador");
        if (jogadores.Count > 1)
        {
            var segundoJogador = jogadores[1];
            bool removido = Jogador.RemoverJogador(segundoJogador.codigo);
            Console.WriteLine(removido ? "Jogador removido com sucesso!" : "Falha ao remover jogador.");
        }

        Console.WriteLine("\nTeste 5: Listar jogadores após remoção");
        jogadores = Jogador.ListarJogadores();
        foreach (var jogador in jogadores)
        {
            Console.WriteLine(jogador.ToString());
        }