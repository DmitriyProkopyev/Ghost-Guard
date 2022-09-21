namespace Ghost_Guard.Models.Infrastructure;

public interface IDataFormatter<TBasic, TFormatted>
{
    TFormatted Format(TBasic input);

    TBasic UnFormat(TFormatted input);
}
