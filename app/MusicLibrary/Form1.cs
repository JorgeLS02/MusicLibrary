namespace MusicLibrary
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {


            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Enter your username and password.");
                return;
            }

            try
            {
                using var connection = DatabaseConnection.CreateConnection(
                    txtUsername.Text.Trim(),
                    txtPassword.Text
                );

                connection.Open();

                using var command = new MySql.Data.MySqlClient.MySqlCommand(
                    "SELECT CURRENT_ROLE();",
                    connection
                );

                string role = Convert.ToString(
                    command.ExecuteScalar()
                ) ?? "NONE";

                MessageBox.Show(
                    "Connected successfully!\nActive role: " + role,
                    "Music Library"
                );
                connection.Close();

                using (MainForm mainForm = new MainForm(connection))
                {
                    this.Hide();

                    mainForm.ShowDialog();

                    this.Show();
                }

            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show(
                    "Connection failed. Check your username and password.",
                    "Connection Error"
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Music Library"
                );
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
