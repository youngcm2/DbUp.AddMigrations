namespace DbUp.AddMigrations
{
    interface IProviderMigrations<in TOptions>
    {
        int Add(TOptions options);
    }
}