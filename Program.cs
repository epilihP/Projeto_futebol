using Players;
using Associacao;
using generics;
        
        // Console.WriteLine("Teste 1: Adicionar jogador");
        // Jogador.AdicionarJogador(new Jogador("Philipe", 25, Posicao.AtacanteDireito));
        // Jogador.AdicionarJogador(new Jogador("Alisson", 22, Posicao.Goleiro));
        // Console.WriteLine("Jogadores adicionados com sucesso!");

        // Console.WriteLine("\nTeste 2: Listar jogadores");
        // var jogadores = Jogador.ListarJogadores();
        // foreach (var jogador in jogadores)
        // {
        //     Console.WriteLine(jogador.ToString());
        // }

        // Console.WriteLine("\nTeste 3: Atualizar jogador");
        // if (jogadores.Count > 0)        
        // {
        //     var primeiroJogador = jogadores[0];
        //     bool atualizado = Jogador.AtualizarJogador(primeiroJogador.codigo, "Carlos Silva", 26, Posicao.Goleiro);
        //     Console.WriteLine(atualizado ? "Jogador atualizado com sucesso!" : "Falha ao atualizar jogador.");
        // }

        // Console.WriteLine("\nTeste 4: Remover jogador");
        // if (jogadores.Count > 1)
        // {
        //     var segundoJogador = jogadores[1];
        //     bool removido = Jogador.RemoverJogador(segundoJogador.codigo);
        //     Console.WriteLine(removido ? "Jogador removido com sucesso!" : "Falha ao remover jogador.");
        // }

        // Console.WriteLine("\nTeste 5: Listar jogadores após remoção");
        // jogadores = Jogador.ListarJogadores();
        // foreach (var jogador in jogadores)
        // {
        //     Console.WriteLine(jogador.ToString());
        // }

        // Testando com um valor inteiro
        CaixaObjeto caixaInt = new CaixaObjeto();
        caixaInt.Valor = 42;
        Console.WriteLine($"Valor inteiro: {caixaInt.Valor}");

        // Testando com uma string
        CaixaObjeto caixaString = new CaixaObjeto();
        caixaString.Valor = "Olá, mundo!";
        Console.WriteLine($"Valor string: {caixaString.Valor}");

        // Tentando acessar sem cast
        try
        {
            int valorSemCast = (int)caixaInt.Valor; // Funciona
            Console.WriteLine($"Valor sem cast (int): {valorSemCast}");

            string erroSemCast = (string)caixaInt.Valor; // Gera InvalidCastException
            Console.WriteLine($"Valor sem cast (string): {erroSemCast}");
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine($"Erro de cast: {ex.Message}");
        }
        