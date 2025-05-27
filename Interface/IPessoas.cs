namespace ICodigos;

public interface ICodigos<T>
{
    T GerarCodigoUnico(HashSet<T> codigosExistentes);
}

// Interface em gernerics :)