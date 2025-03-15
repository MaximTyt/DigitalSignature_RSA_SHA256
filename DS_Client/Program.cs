using DS_Lib;
using Microsoft.Extensions.DependencyInjection;

namespace DS_Client
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // ��������� ���������� DI
            var services = new ServiceCollection();
            ConfigureServices(services);

            // �������� ���������� �����
            var serviceProvider = services.BuildServiceProvider();

            // ������ ������� �����
            var form = serviceProvider.GetRequiredService<ClientForm>();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            Application.Run(form);
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            // ����������� ������������
            services.AddTransient<IDS_Client_Logic, DS_Client_Logic>();
            services.AddTransient<IDS, DS>(); // ������������ ��������� � ��� ����������
            services.AddTransient<ClientForm>(); // ������������ �����
        }
    }
}