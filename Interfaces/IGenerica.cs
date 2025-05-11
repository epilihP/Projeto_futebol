namespace IGenerica;

// usei o T para um tipo de parametro que não foi pre-determiado, podendo usar o generico para salvar algo durante o projeto
public interface ICaixa<T>{
    void Guardar(T a);
}

public interface ICaixaObjeto
{
    public object Valor {get;set;}
}