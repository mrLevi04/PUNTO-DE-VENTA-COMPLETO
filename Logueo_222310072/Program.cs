namespace Logueo_222310072
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Crear fondo negro
            BACKGROUND fondo = new BACKGROUND();
            fondo.Show();

            // Crear y mostrar formulario principal
            Form1 principal = new Form1();
            principal.TopMost = true;
       
            // Mostrar el formulario principal como ventana principal de la app
            Application.Run(principal);
        }
    }
}   